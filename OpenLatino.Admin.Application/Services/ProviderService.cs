using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Internationalization;
using OpenLatino.Core.Domain.Models;
using System.Linq;
using System.Collections.Generic;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Infrastructure.SQL.Queries;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Infrastructure.SQL.DataSource;

namespace OpenLatino.Admin.Application.Services
{
    public class ProviderService : CRUD_Service<ProviderInfo>, IProviderHelper
    {
        private IRepository<ProviderTranslations> translationsRepo;
        private IRepository<Language> langRepo;
        private IRepository<Layer> layerRepo;

        public ProviderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            translationsRepo = this.unitOfWork.Set<ProviderTranslations>();
            langRepo = this.unitOfWork.Set<Language>();
            layerRepo = this.unitOfWork.Set<Layer>();
        }

        public IEnumerable<ProviderFullInfo> FullInfoList()
        {
            var infoProviders = (from trans in (from t in translationsRepo.GetAll(pt => true, true)
                                                group t by t.EntityId into transGroup
                                                select transGroup)
                                 join provider in repository.GetAll(pr => true, true) on trans.Key equals provider.Id
                                 select new ProviderFullInfo() { ProviderTranslation = trans.ToList(), Provider = provider }
                                );
            return infoProviders;
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return unitOfWork;
        }

        public ProviderInfo DetailedInfo(int id)
        {
            return repository.GetAll(pr => true, true, pr => pr.ProviderTranslations).FirstOrDefault(pr => pr.Id == id);
        }

        public CRUD_Service<ProviderInfo> GetCRUD()
        {
            return this;
        }

        public bool createProvider(string name, string description, ProviderInfo provider)
        {
            var eng = langRepo.GetAll(l => l.LanguageName == "English").FirstOrDefault();

            string type = "OpenLatino.MapServer.Infrastucture.SQL.DataSource.ProviderSQL, " +
                          "OpenLatino.MapServer.Infrastructure.SQL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            provider.Type = type;

            var newProvider = repository.Add(provider);
            unitOfWork.SaveChanges();

            translationsRepo.Add(new ProviderTranslations()
            {
                Name = name,
                Description = description,
                LanguageId = eng.ID,
                EntityId = newProvider.Id

            });
            unitOfWork.SaveChanges();

            return true;

        }

        public void deleteProvider(int id)
        {
            ProviderInfo provider = (ProviderInfo)GetById(id);
            Remove(provider);
            unitOfWork.SaveChanges();
        }

        public bool editProvider(string name, string description, ProviderInfo provider)
        {
            string type = "OpenLatino.MapServer.Infrastucture.SQL.DataSource.ProviderSQL, " +
                          "OpenLatino.MapServer.Infrastructure.SQL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            provider.Type = type;

            repository.Modify(provider);
            unitOfWork.SaveChanges();

            var translation = translationsRepo.GetAll(t => t.EntityId == provider.Id).FirstOrDefault();

            translation.Name = name;
            translation.Description = description;

            translationsRepo.Modify(translation);
            unitOfWork.SaveChanges();

            return true;
        }

        public IEnumerable<object> getProviderInfo(int layerId, string table = null, string connString = null)
        {
            if (connString == null)
            {
                Layer _layer = layerRepo.GetAll(l => l.Id == layerId, true, d => d.ProviderInfo).FirstOrDefault();
                connString = _layer.ProviderInfo.ConnectionString;
            }
            

            IWMSQuery query = new SQLWMSAlphaInfoQuery();

            if (table != null)
                query.InfoColums = table;

            IProviderService provider = new DBContextProvider()
            {
                ConnectionString = connString
            };

            var result = new List<object>();

            foreach (Dictionary<string, object> item in provider.Execute(query))
                result = result.Concat(item.Values).ToList();

            return result;
        }
    }
}
