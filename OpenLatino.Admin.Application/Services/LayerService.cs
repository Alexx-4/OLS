using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Internationalization;
using OpenLatino.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenLatino.Admin.Application.Services
{
    public class LayerService:CRUD_Service<Layer>, ILayerHelper
    {
        private IRepository<ProviderInfo> providerRepo;
        private IRepository<ProviderTranslations> provTransRepo;
        private IRepository<LayerTranslation> layerTransRepo;
        private IRepository<VectorStyle> stylesRepo;

        public LayerService(IUnitOfWork unitOfWork):base(unitOfWork)
        {
            this.providerRepo = unitOfWork.Set<ProviderInfo>();
            this.provTransRepo = unitOfWork.Set<ProviderTranslations>();
            this.layerTransRepo = unitOfWork.Set<LayerTranslation>();
            this.stylesRepo = unitOfWork.Set<VectorStyle>();
        }

        public IEnumerable<Layer> GetAll()
        {
            return repository.GetAll(l => true, true, l => l.LayerTranslations);
        }

        public IEnumerable<Layer> GetLayersInWorkspace(WorkSpace workSpace)
        {
            return repository.GetAll(ll => workSpace.LayerWorkspaces.Any(lw => lw.LayerId == ll.Id), true, l=>l.LayerTranslations);
        }

        public Layer GetLayer(int id)
        {
            return repository.GetAll(l => l.Id == id,true,l=>l.LayerTranslations).FirstOrDefault();
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return unitOfWork;
        }

        public CRUD_Service<Layer> GetCRUD()
        {
            return this;
        }

        public IDictionary<int, string> GetProviderNameById()
        {
            var providers = providerRepo.GetAll(pr => true, true, pr => pr.ProviderTranslations);
            IDictionary<int, string> result = new Dictionary<int, string>();
            foreach (var item in providers)
            {
                result[item.Id] = item.ProviderTranslations.FirstOrDefault()?.Name ?? "";
            }
            return result;
        }

        public IEnumerable<LayerFullInfo> GetFullList()
        {
            var result = from trans in (from lt in layerTransRepo.GetAll(ltr => true, true)
                         group lt by lt.EntityId into transGroup
                         select transGroup)
                         join layer in repository.GetAll(l => true, true, l => l.ProviderInfo.ProviderTranslations, l => l.VectorStyles) on trans.Key equals layer.Id
                         select new LayerFullInfo() { LayerTranslations = trans.ToList(), Layer = layer };

            return result;
        }

        public IEnumerable<VectorStyle> GetLayerStyles(int id)
        {
            return stylesRepo.GetAll(sty => sty.Layers.Any(ly => ly.LayerId == id), true);
        }

        public IEnumerable<VectorStyle> GetAllStyles()
        {            
            return stylesRepo.GetAll(st => true, true).ToList();
        }

        public bool CreateLayer(Layer layer, string[] vectorStyles, string name, string description)
        {
            IRepository<LayerStyle> layerStyleRepo = unitOfWork.Set<LayerStyle>();
            
            if (repository.GetAll(ly => ly.ProviderInfoId == layer.ProviderInfoId && ly.Order == layer.Order, true).FirstOrDefault() != null)
                return false;

            repository.Add(layer);
            unitOfWork.SaveChanges();

            var styles = new List<VectorStyle>();
            foreach (var item in vectorStyles)
            {
                var st = stylesRepo.GetAll(s => s.Name == item).FirstOrDefault();

                layerStyleRepo.Add(new LayerStyle { LayerId = layer.Id, VectorStyleId = st.Id });
            }

            layerTransRepo.Add(new LayerTranslation { EntityId = layer.Id, LanguageId = 1033, Name = name, Description = description });
            unitOfWork.SaveChanges();
            return true;
        }

        public bool EditLayer(Layer layer, string[] vectorStyles, string name, string description)
        {
            IRepository<LayerStyle> layerStyleRepo = unitOfWork.Set<LayerStyle>();
           
            if (repository.GetAll(ly => ly.ProviderInfoId == layer.ProviderInfoId && ly.Order == layer.Order && ly.Id != layer.Id, true).FirstOrDefault() != null)
                return false;

            var oldLayer = repository.GetAll(ly => ly.Id == layer.Id, true, ly => ly.VectorStyles).FirstOrDefault();
            foreach (var item in oldLayer.VectorStyles)
            {
                layerStyleRepo.Remove(item);
            }
            unitOfWork.SaveChanges();
                
            repository.Modify(layer);
            unitOfWork.SaveChanges();

            var styles = new List<VectorStyle>();
            foreach (var item in vectorStyles)
            {
                var st = stylesRepo.GetAll(s => s.Name == item).FirstOrDefault();

                layerStyleRepo.Add(new LayerStyle { LayerId = layer.Id, VectorStyleId = st.Id });
            }
            unitOfWork.SaveChanges();

            var translation = layerTransRepo.GetAll(t => t.EntityId == layer.Id).FirstOrDefault();

            translation.Name = name;
            translation.Description = description;

            layerTransRepo.Modify(translation);
            unitOfWork.SaveChanges();

            return true;
            
        }

        public void deleteLayer(int id)
        {
            Layer _layer = (Layer)GetById(id);
            repository.Remove(_layer);
            unitOfWork.SaveChanges();

        }
    }
}
