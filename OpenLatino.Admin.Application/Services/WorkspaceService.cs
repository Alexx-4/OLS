using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace OpenLatino.Admin.Application.Services
{
    public class WorkspaceService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<WorkSpace> workspaceRepository;
        private IRepository<ApplicationUser> userRepo;
        private IRepository<Client> clientRepo;
        private IRepository<LayerWorkspaces> layerWorkspaceRepo;
        private WorkspaceFunctionService workspaceFunctionServ;
        private LayerWorkspaceService layerWorkspaceService;

        public WorkspaceService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.workspaceRepository = unitOfWork.Set<WorkSpace>();
            this.userRepo = unitOfWork.Set<ApplicationUser>();
            this.clientRepo = unitOfWork.Set<Client>();
            this.layerWorkspaceRepo = unitOfWork.Set<LayerWorkspaces>();
            this.workspaceFunctionServ = new WorkspaceFunctionService(unitOfWork);
            this.layerWorkspaceService = new LayerWorkspaceService(unitOfWork);
        }

        public IEnumerable<object> GetAll(string userId = null)
        {
            List<object> result = new List<object>();

            var allWorkspaces = userId == null ? workspaceRepository.GetAll(null, false, w => w.User).ToList() :
                                                 workspaceRepository.GetAll(w => w.ApplicationUserId == userId).ToList();

            foreach (var item in allWorkspaces)
            {
                var _workspace = new
                {
                    Id = item.Id,
                    UserId = item.ApplicationUserId,
                    UserName = item.User is null ? "--(default workspace)--" : item.User.UserName,
                    Name = item.Name,
                    ClientApps = item.CLientWorkSpaces.Select(c => new
                    {
                        Id = c.ClientId,
                        Name = c.Client.Name
                    }),
                    Funcs = item.ServiceFunctions.Select(f => new
                    {
                        Id = f.FunctionId,
                        Name = f.Function.Name
                    }),
                    Layers = item.LayerWorkspaces.Select(l => new
                    {
                        Id = l.LayerId,
                        Name = l.Layer.LayerTranslations.ToList()[0].Name
                    })
                };
                result.Add(_workspace);
            }

            return result;
        }

        public IEnumerable<WorkSpace> GetWorkspacesForClient(string clientId)
        {
            return GetWorkspacesForClient(new ClientService(unitOfWork).FindClientAllIncluded(clientId));
        }

        public IEnumerable<WorkSpace> GetWorkspacesForClient(Client client)
        {
            return workspaceRepository.GetAll(w => client.ClientWorkSpaces.Any(cw => cw.WorkSpaceId == w.Id), true, cw => cw.LayerWorkspaces);
        }

        public IEnumerable<WorkSpace> GetWorkSpacesForUser(string userId)
        {
            return workspaceRepository.GetAll(cw => cw.ApplicationUserId == userId, true);
        }

        public IEnumerable<WorkSpace> GetWorkSpacesForUser(ClaimsPrincipal user)//cambiar para que no reciba nada
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return workspaceRepository.GetAll(cw => cw.ApplicationUserId == userId, true, cw => cw.CLientWorkSpaces, cw => cw.LayerWorkspaces);
        }

        public IEnumerable<WorkSpace> GetWorkSpacesForClient(string wName, string clientId)
        {
            return workspaceRepository.GetAll(w => w.Name == wName && w.CLientWorkSpaces.Any(cw => cw.ClientId == clientId), true);
        }

        public WorkSpace FillWorkspaceStyles(WorkSpace workSpace)
        {
            var result = new WorkSpace()
            {
                ApplicationUserId = workSpace.ApplicationUserId,
                CLientWorkSpaces = workSpace.CLientWorkSpaces,
                Default = workSpace.Default,
                ForAppType = workSpace.ForAppType,
                Id = workSpace.Id,
                LayerWorkspaces = new List<LayerWorkspaces>(),
                Name = workSpace.Name,
                User = workSpace.User,
                ServiceFunctions = workSpace.ServiceFunctions
            };
            foreach (var item in workSpace.LayerWorkspaces)
            {
                result.LayerWorkspaces.Add(new LayerWorkspaces()
                {
                    LayerId = item.LayerId,
                    VectorStyleId = item.VectorStyleId,
                    WorkSpaceId = item.WorkSpaceId,
                    Style = unitOfWork.Set<VectorStyle>().Find(item.VectorStyleId)
                });
            }
            return result;
        }

        public WorkSpace GetWorkSpace(int id, bool noTrack = true, bool includeLayersStyle = false)
        {
            var workspace = workspaceRepository.GetAll(w => w.Id == id, noTrack, w => w.LayerWorkspaces, w => w.ServiceFunctions, w => w.CLientWorkSpaces).FirstOrDefault();
            if (includeLayersStyle) //Rellenar su lista de Layer
            {
                List<LayerWorkspaces> newList = new List<LayerWorkspaces>();
                foreach (var item in workspace.LayerWorkspaces)
                {
                    var toAdd = layerWorkspaceRepo.GetAll(lw => lw.WorkSpaceId == item.WorkSpaceId && lw.LayerId == item.LayerId, true, lw => lw.Style).FirstOrDefault();
                    newList.Add(toAdd);
                }
                workspace.LayerWorkspaces = newList;
            }
            return workspace;
        }

        public WorkspaceFullInfo GetWorkspaceFullInfo(int id)
        {
            var requestedWorkspace = GetWorkSpace(id);
            LayerService layerService = new LayerService(unitOfWork);
            ClientService clientService = new ClientService(unitOfWork);

            var layers = layerService.GetLayersInWorkspace(requestedWorkspace);
            var clients = clientService.GetClientsInWorkspace(requestedWorkspace);
            var functions = workspaceFunctionServ.GetServiceFunctionsForWorkspace(requestedWorkspace);

            WorkspaceFullInfo fullInfo = new WorkspaceFullInfo()
            {
                Id = id,
                Name = requestedWorkspace.Name,
                ServiceFunctions = functions.ToList(),
                Clients = clients.ToList()
            };
            var _list = new List<LayerStyled>();
            foreach (var item in layers)
                _list.Add(new LayerStyled() { LayerName = item.Name, Style = layerWorkspaceService.GetVectorStyleOf(item.Id, id) });
            fullInfo.LayerStyled = _list;
            return fullInfo;
        }

        public int AddWorkspace(WorkSpace workSpace, string userId, List<int> layers, List<int> serviceFunctions)
        {
            workSpace.ApplicationUserId = userId;
            workspaceRepository.Add(workSpace);
            unitOfWork.SaveChanges();

            layerWorkspaceService.Add(workSpace.Id, layers);
            workspaceFunctionServ.Add(workSpace.Id, serviceFunctions);

            return workSpace.Id;

        }

        public void Update(WorkSpace workSpace)
        {
            workspaceRepository.Modify(workSpace);
            unitOfWork.SaveChanges();
        }

        public void editWorkspace(WorkspaceModel model)
        {
            var workspace = GetWorkSpace(model.Id, false);
            workspace.Name = model.Name;

            Update(workspace);
            layerWorkspaceService.Update(workspace.Id, model.LayerIds);
            workspaceFunctionServ.Update(workspace.Id, model.FunctionIds);

        }

       public void Remove(int workspaceId)
       {
            WorkSpace workspace = GetWorkSpace(workspaceId, false);
            workspaceRepository.Remove(workspace);
            unitOfWork.SaveChanges();
       }
    }
}
