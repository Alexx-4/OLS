using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenLatino.Admin.Application.Services
{
    public class ClienWorkspaceService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<ClientWorkSpaces> cwRepository;
        private IRepository<LayerWorkspaces> lwRepository;

        public ClienWorkspaceService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.cwRepository = unitOfWork.Set<ClientWorkSpaces>();
            this.lwRepository = unitOfWork.Set<LayerWorkspaces>();
        }

        public void Add(string clientId, int workspaceId)
        {
            cwRepository.Add(new ClientWorkSpaces() { ClientId = clientId, WorkSpaceId = workspaceId });
            unitOfWork.SaveChanges();
        }

        public void Remove(ClientWorkSpaces cw)
        {
            //Importante respetar el orden de las llaves
            cwRepository.Remove(cw);
            unitOfWork.SaveChanges();
        }

        public WorkSpace WorkSpaceForClient(string clientId, string workspaceName)
        {
            var workspace = cwRepository.GetAll(cw => cw.WorkSpace.Name == workspaceName && cw.ClientId == clientId, true, cw=>cw.WorkSpace.ServiceFunctions,cw => cw.WorkSpace.LayerWorkspaces).Select(cw=>cw.WorkSpace).FirstOrDefault();
            if (workspace == null)
                return null;

            //Rellenar su lista de Layer
            List<LayerWorkspaces> newList = new List<LayerWorkspaces>();
            foreach (var item in workspace.LayerWorkspaces)
            {
                var toAdd = lwRepository.GetAll(lw => lw.WorkSpaceId == item.WorkSpaceId && lw.LayerId == item.LayerId, true, lw => lw.Style).FirstOrDefault();
                newList.Add(toAdd);
            }
            workspace.LayerWorkspaces = newList;
            return workspace;
        }        
    }
}
