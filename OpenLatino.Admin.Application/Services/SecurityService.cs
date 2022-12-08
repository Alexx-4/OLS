using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenLatino.Admin.Application.Services
{
    public class SecurityService
    {
        private ClientService ClientServices;
        private IUnitOfWork unitOfWork;
        public SecurityService(IUnitOfWork unitOfWork)
        {
            this.ClientServices = new ClientService(unitOfWork);
            this.unitOfWork = unitOfWork;
        }

        public bool ClientCanAccess(int workspaceId, string token)
        {
            return ClientServices.ClientHasWorkspace(ClientServices.GetRequestingClient(token), workspaceId);
        }

        public bool ClientCanAccess(string workspaceName, string token)
        {
            return ClientServices.ClientHasWorkspace(ClientServices.GetRequestingClient(token), workspaceName);
        }

        public bool UserCanAccess(int workspaceId, string userId)
        {
            var test = unitOfWork.Set<ApplicationUser>();
            return unitOfWork.Set<ApplicationUser>().GetAll(u => u.UserId == userId && u.CreatedWorkSpaces.Any(cw => cw.Id == workspaceId), true).FirstOrDefault() != null;
        }

        public bool UserCanAccess(string clientId, string userId)
        {
            return unitOfWork.Set<ApplicationUser>().GetAll(u => u.UserId == userId && u.RegisterApps.Any(ra => ra.Id == clientId), true).FirstOrDefault() != null;
        }

        public bool IsClientOfUser(string clientId, string userId)
        {
            Client client = ClientServices.FindClientAllIncluded(clientId);
            return (client.ApplicationUserId == userId);
        }
    }
}
