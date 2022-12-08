using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Internationalization;
using System.Linq;

namespace OpenLatino.Admin.Application.Services
{
    public class LanguageService : CRUD_Service<Language>, ILanguageHelper
    {
        public LanguageService(IUnitOfWork unitOfWork): base(unitOfWork)
        {}

        public CRUD_Service<Language> GetCRUD()
        {
            return this;
        }

        public override void Create(Language obj)
        {
            if (obj.Default)
            {
                foreach (var item in repository.GetAll(l => l.LanguageName != obj.LanguageName))
                {
                    item.Default = false;
                    repository.Modify(item);
                }
            }
            repository.Add(obj);
            unitOfWork.SaveChanges();
        }

        public override void Update(Language obj)
        {
            if (obj.Default)
            {
                foreach (var item in repository.GetAll(l => l.LanguageName != obj.LanguageName))
                {
                    item.Default = false;
                    repository.Modify(item);
                }
            }
            repository.Modify(obj);
            unitOfWork.SaveChanges();
        }

        public bool IsRepeated(Language language)
        {
            return repository.GetAll(l => l.LanguageName == language.LanguageName, true).FirstOrDefault() != null;
        }
    }
}
