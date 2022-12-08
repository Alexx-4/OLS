using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Internationalization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OpenLatino.Core.Domain.Models;
using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Infrastructure.SQL.Queries;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Infrastructure.SQL.DataSource;

namespace OpenLatino.Admin.Application.Services
{
    public class AlfaInfoService: CRUD_Service<AlfaInfo>, IAlfaInfoHelper
    {
        private IRepository<Layer> layerRepo;
        private IRepository<LayerTranslation> layerTransRepo;
        private IRepository<AlfaInfoTranslation> alfaInfTransRepo;
        private IRepository<Language> langRepo;

        public AlfaInfoService(IUnitOfWork unitOfWork):base(unitOfWork)
        {
            this.layerRepo = unitOfWork.Set<Layer>();
            this.layerTransRepo = unitOfWork.Set<LayerTranslation>();
            this.alfaInfTransRepo = unitOfWork.Set<AlfaInfoTranslation>();
            this.langRepo = unitOfWork.Set<Language>();
        }

        public bool CreateAlphaInfo(AlfaInfo _alphaInfo, string name, string description)
        {
            var eng = langRepo.GetAll(l => l.LanguageName == "English").FirstOrDefault();

            var newAlphaInfo = repository.Add(_alphaInfo);
            unitOfWork.SaveChanges();

            alfaInfTransRepo.Add(new AlfaInfoTranslation()
            {
                Name = name,
                Description = description,
                EntityId = newAlphaInfo.Id,
                LanguageId = eng.ID
            });
            unitOfWork.SaveChanges();

            return true;
        }

        public IEnumerable<AlfaFullInfo> FullList()
        {
           return (from trans in (from t in alfaInfTransRepo.GetAll(t=>true,true)
                            group t by t.EntityId into transGroup
                            select transGroup)
             join alfa in repository.GetAll(t=>true,true) on trans.Key equals alfa.Id
             select new AlfaFullInfo() { AlfaInfo = alfa, Translations = trans.ToList() });
        }

        public IEnumerable<Tuple<int,string>> GetLayerNameById()
        {
            return  layerRepo.GetAll(l => true, true, l => l.LayerTranslations)
                    .Select(l=> new Tuple<int, string>(l.Id, l.Name));
        }

        public override object GetById(int? id)
        {
            if (id == null)
                return null;
            return repository.GetAll(t=>t.Id == id,true, a => a.AlfaInfoTranslations).FirstOrDefault();
        }

        public CRUD_Service<AlfaInfo> GetCRUD()
        {
            return this;
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return this.unitOfWork;
        }

        public void deleteAlphaInfo(int id)
        {
            var _alfainfo = repository.Find(id);
            Remove(_alfainfo);
            unitOfWork.SaveChanges();
        }

        public bool EditAlphaInfo(AlfaInfo _alphaInfo, string name, string description)
        {
            repository.Modify(_alphaInfo);
            unitOfWork.SaveChanges();

            var translation = alfaInfTransRepo.GetAll(t => t.EntityId == _alphaInfo.Id).FirstOrDefault();

            translation.Name = name;
            translation.Description = description;

            alfaInfTransRepo.Modify(translation);
            unitOfWork.SaveChanges();

            return true;
        }
    }
}
