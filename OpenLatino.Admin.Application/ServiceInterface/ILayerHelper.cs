using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Models;
using System.Collections.Generic;

namespace OpenLatino.Admin.Application.ServiceInterface
{
    public interface ILayerHelper
    {
        IEnumerable<Layer> GetAll();
        IEnumerable<Layer> GetLayersInWorkspace(WorkSpace workSpace);
        Layer GetLayer(int id);
        IUnitOfWork GetUnitOfWork();
        CRUD_Service<Layer> GetCRUD();
        IDictionary<int, string> GetProviderNameById();
        IEnumerable<LayerFullInfo> GetFullList();
        IEnumerable<VectorStyle> GetLayerStyles(int id);
        IEnumerable<VectorStyle> GetAllStyles();
        bool CreateLayer(Layer layer, string[] vectorStyles, string name, string description);
        bool EditLayer(Layer layer, string[] vectorStyles, string name, string description);
        void deleteLayer(int id);
    }
}
