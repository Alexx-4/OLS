using Microsoft.EntityFrameworkCore;
using OpenLatino.Admin.Application.Utils;
using OpenLatino.Core.Domain.Models;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Enums;
using OpenLatino.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using OpenLatino.Core.Domain.Services;

namespace OpenLatino.Admin.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> clientRepository;
        private readonly IRepository<ClientWorkSpaces> clientWorkRepo;
        private readonly WorkspaceService workspaceService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenService;

        public ClientService(IUnitOfWork unitOfWork, ITokenService tokenService=null)
        {
            this.unitOfWork = unitOfWork;
            clientRepository = unitOfWork.Set<Client>();
            clientWorkRepo = unitOfWork.Set<ClientWorkSpaces>();
            this.workspaceService = new WorkspaceService(unitOfWork);
            this.tokenService = tokenService;
        }

        public IEnumerable<Client> GetAll()
        {
            return clientRepository.GetAll();
        }

        public IEnumerable<Client> GetAllClientsForUser(string userId)
        {
            return clientRepository.GetAll().Where(c=>c.ApplicationUserId == userId);
        }

        public IEnumerable<Client> GetAllClientsForUser(ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return clientRepository.GetAll(c => c.ApplicationUserId == userId, true, c => c.ClientWorkSpaces);
        }

        public IEnumerable<Client> GetClientsInWorkspace(WorkSpace workSpace)
        {
            return clientRepository.GetAll(cl => workSpace.CLientWorkSpaces.Any(cw => cw.ClientId == cl.Id), true);
        }

        public IEnumerable<WorkSpace> WorkSpacesForCLient(string id)
        {
            var client = FindClientAllIncluded(id);
            if (client == null)
                return null;
            return workspaceService.GetWorkspacesForClient(client) ?? new List<WorkSpace>();
        }

        public Tuple<string, appCredentials> Register(ClientModel clientModel, string userId)
        {
            var client = GetAll().FirstOrDefault(c => c.Name == clientModel.Name);

            if (client != null)
                return new Tuple<string, appCredentials>($"The client {clientModel.Name} already exist",null);

            clientModel.AllowedOrigen = ""; //presindible
            clientModel.Password = "1234"; //esto es un campo que, por ahora no se usa
            var createdClient = AddClient(clientModel, userId);
            if (createdClient == null)
                return new Tuple<string, appCredentials>("Creation error", null);

            //Generate token
            var token = tokenService.GetTokenFor(createdClient);
            UpdateAccessKey(createdClient.ClientId, token);

            return new Tuple<string, appCredentials>(null, new appCredentials { AccessKey = token, UpdateKey = createdClient.UpdateKey, AppId = createdClient.ClientId });
        }

        public ClienFullInfo GetClienFullInfo(string id)
        {            
            var client = FindClientAllIncluded(id);
            if (client == null)
                return null;
            var workspaces = WorkSpacesForCLient(id);
            var aux = new ClienFullInfo()
            {
                ClientId = client.Id,
                AccessKey = client.AccessKey,
                Active = client.Active,
                ApplicationType = client.ApplicationType.ToString(),
                Name = client.Name,
                UpdateKey = client.UpdateKey,
                workSpaces = workspaces.ToList(),
                WorkspacesIds = workspaces.Select(w => w.Id).ToList()
            };
            return aux;
        }

        public ClienFullInfo GetClientFullForEdit(string id, ClaimsPrincipal user)
        {
            var info = GetClienFullInfo(id);
            info.workSpaces = workspaceService.GetWorkSpacesForUser(user).ToList();
            info.workSpaces.Add(workspaceService.GetWorkSpace(1));
            return info;
        }

        public ClientCredentialModel AddClient(ClientModel clientModel, string userId)
        {
            var clientId = Guid.NewGuid().ToString("N").Replace('+','1'); //para que no genere guids con ese signo, porque trae problemas con las URL
            var now = DateTime.UtcNow;
            var updateKey = getRandomUpdateKey();

            var date = now.Add(Helper.TIcketLifeTime);

            var client = new Client
            {
                Id = clientId,
                Name = clientModel.Name,
                Active = true,
                AllowedOrigin = clientModel.AllowedOrigen,
                ApplicationType = clientModel.ApplicationType == ApplicationType.CommunApp.ToString() ? ApplicationType.CommunApp : ApplicationType.ConfidentialApp,
                ExpirationDate = date.ToFileTimeUtc(),
                Password = clientModel.Password,
                ApplicationUserId = userId,
                UpdateKey = updateKey
            };

            client = clientRepository.Add(client);
            clientWorkRepo.Add(new ClientWorkSpaces() { ClientId = client.Id, WorkSpaceId = 1 });

            foreach (var item in clientModel.WorkspacesIds??new List<int>())
            {
                clientWorkRepo.Add(new ClientWorkSpaces() { ClientId = client.Id, WorkSpaceId = item });
            }

            return ((DbContext)unitOfWork).SaveChanges() > 0 ?
                new ClientCredentialModel
                {
                    ClientId = client.Id, Name = client.Name, Active = client.Active,
                    UpdateKey = updateKey, AllowedOrigen = client.AllowedOrigin,
                    ExpirationDate = client.ExpirationDate
                } : null;
        }

        public string getRandomUpdateKey()
        {
            return Guid.NewGuid().ToString("N");
        }

        public Client UpdateAccessKey(string clientId, string newAccess)
        {
            var client = FindClient(clientId);
            client.AccessKey = newAccess;
            clientRepository.Modify(client);
            clientRepository.UnitOfWork.SaveChanges();
            return client;
        }

        public Client Update_UpdateKey(string clientId, string newUpdate)
        {
            var client = FindClient(clientId);
            client.UpdateKey = newUpdate;
            clientRepository.Modify(client);
            clientRepository.UnitOfWork.SaveChanges();
            return client;
        }

        public void Update(ClienFullInfo clientFullInfo)
        {
            var requested = clientRepository.Find(clientFullInfo.ClientId);
            requested.Name = clientFullInfo.Name;
            requested.Active = clientFullInfo.Active;
            requested.ApplicationType = (clientFullInfo.ApplicationType == Core.Domain.Enums.ApplicationType.ConfidentialApp.ToString()) ? Core.Domain.Enums.ApplicationType.ConfidentialApp : Core.Domain.Enums.ApplicationType.CommunApp;
            //Now update clientWorkspaces
            var old = clientWorkRepo.GetAll(cw => cw.ClientId == clientFullInfo.ClientId);
            //remove Old
            foreach (var item in old)
            {
                if(item.WorkSpace.Name != "common" )
                    clientWorkRepo.Remove(item);
            }
            //add New
            foreach (var item in clientFullInfo.WorkspacesIds ?? new List<int>())
            {
                clientWorkRepo.Add(new ClientWorkSpaces() { ClientId = clientFullInfo.ClientId, WorkSpaceId = item});
            }
            unitOfWork.SaveChanges();
        }

        public Client FindClient(string clientId)
        {
            return clientRepository.Find(clientId);
        }

        public Client FindClientAllIncluded(string clientId)
        {
            return clientRepository.GetAll(c => c.Id == clientId, true, c => c.ClientWorkSpaces).FirstOrDefault();
        }

        public Client GetRequestingClient(string token)
        {
            return clientRepository.GetAll(c => c.Id == TokenService.GetClientId(token), true, c => c.ClientWorkSpaces).FirstOrDefault();
        }

        public bool ClientHasWorkspace(Client client, int idWorkSpace)
        {
            return client.ClientWorkSpaces.Any(cw => cw.WorkSpaceId == idWorkSpace);
        }

        //borrar
        public bool ClientHasWorkspace(Client client, string WorkSpaceName)
        {
            return client.ClientWorkSpaces.Any(cw => cw.WorkSpace.Name == WorkSpaceName);
        }

        public int GetWorkSpaceIdForClient(Client client, string WorkSpaceName)
        {
            return client.ClientWorkSpaces.Where(cw => cw.WorkSpace.Name == WorkSpaceName).Select(cw => cw.WorkSpaceId).FirstOrDefault();
        }

        public void Remove(string clientId)
        {
            var client = clientRepository.Find(clientId);
            clientRepository.Remove(client);
            unitOfWork.SaveChanges();
        }
    }
}
