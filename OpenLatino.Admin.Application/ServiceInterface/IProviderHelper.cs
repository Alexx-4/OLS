using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Models;
using System.Collections.Generic;

namespace OpenLatino.Admin.Application.ServiceInterface
{
    public interface IProviderHelper
    {
        ProviderInfo DetailedInfo(int id);
        IEnumerable<ProviderFullInfo> FullInfoList();
        IUnitOfWork GetUnitOfWork();
        CRUD_Service<ProviderInfo> GetCRUD();

        bool createProvider(string name, string description, ProviderInfo provider);
        void deleteProvider(int id);
        bool editProvider(string name, string description, ProviderInfo provider);
        IEnumerable<object> getProviderInfo(int layerId, string table = null, string connString = null);
    }
}
