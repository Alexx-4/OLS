using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenLatino.Admin.Application.Services
{
    public class LayerWorkspaceService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<LayerWorkspaces> layerWorkspaceRepository;
        private LayerService ls;

        public LayerWorkspaceService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.layerWorkspaceRepository = unitOfWork.Set<LayerWorkspaces>();
            this.ls = new LayerService(unitOfWork);
        }

        public VectorStyle GetVectorStyleOf(int layerId, int workspaceId)
        {
            return layerWorkspaceRepository.GetAll(lw => lw.LayerId == layerId && lw.WorkSpaceId == workspaceId, true, lw => lw.Style).FirstOrDefault().Style;
        }

        public IEnumerable<Layer> GetLayersOf(int Wid)
        {
            var workspace = unitOfWork.Set<WorkSpace>().GetAll(w => w.Id == Wid, true, w => w.LayerWorkspaces).SingleOrDefault();
            return ls.GetLayersInWorkspace(workspace);
        }

        public void Add(int workspaceId, int layerId)
        {
            layerWorkspaceRepository.Add(new LayerWorkspaces() { WorkSpaceId = workspaceId, LayerId = layerId });
            unitOfWork.SaveChanges();
        }
        public void Add(int workspaceId, List<int> layerIds)
        {
            foreach (var layerId in layerIds)
            {
                layerWorkspaceRepository.Add(new LayerWorkspaces() { WorkSpaceId = workspaceId, LayerId = layerId });
            }
            unitOfWork.SaveChanges();
        }

        public void Update(int workspaceId, IEnumerable<int> layersId)
        {
            var old = layerWorkspaceRepository.GetAll(lw => lw.WorkSpaceId == workspaceId);
           
            foreach (var item in old)
            {
                layerWorkspaceRepository.Remove(item);
            }
          
            foreach (var item in layersId)
            {
                layerWorkspaceRepository.Add(new LayerWorkspaces() { LayerId = item, WorkSpaceId = workspaceId });
            }
            unitOfWork.SaveChanges();
        }

        public void Update(int Wid, int Lid, int Sid)
        {
            var entity = layerWorkspaceRepository.Find(Lid, Wid);
            entity.VectorStyleId = Sid;
            unitOfWork.SaveChanges();
        }

        public void Remove(LayerWorkspaces lw)
        {
            layerWorkspaceRepository.Remove(lw);
            unitOfWork.SaveChanges();
        }

        public bool Contains(int idW, int idL)
        {
            //Importante respetar el orden de las llaves
            return layerWorkspaceRepository.Find(idL, idW)!=null;
        }
    }
}
