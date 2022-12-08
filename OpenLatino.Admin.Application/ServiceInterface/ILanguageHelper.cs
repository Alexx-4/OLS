using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Internationalization;

namespace OpenLatino.Admin.Application.ServiceInterface
{
    public interface ILanguageHelper
    {
        CRUD_Service<Language> GetCRUD();
        bool IsRepeated(Language language);
    }
}
