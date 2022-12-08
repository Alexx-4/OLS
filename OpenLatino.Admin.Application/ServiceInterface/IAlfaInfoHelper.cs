using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Models;
using System;
using System.Collections.Generic;

namespace OpenLatino.Admin.Application.ServiceInterface
{
    public interface IAlfaInfoHelper
    {
        IEnumerable<AlfaFullInfo> FullList();
        IEnumerable<Tuple<int, string>> GetLayerNameById();
        CRUD_Service<AlfaInfo> GetCRUD();
        IUnitOfWork GetUnitOfWork();
        void deleteAlphaInfo(int id);
        bool CreateAlphaInfo(AlfaInfo entity, string name, string description);
        bool EditAlphaInfo(AlfaInfo entity, string name, string description);
    }
}
