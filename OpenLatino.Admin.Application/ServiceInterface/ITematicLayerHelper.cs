using System;
using System.Collections.Generic;
using System.Text;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Models;
using OpenLatino.MapServer.Domain.Map.Filters.Enums;
using Unity;

namespace OpenLatino.Admin.Application.ServiceInterface
{
    public interface ITematicLayerHelper
    {
        IEnumerable<TematicLayer> GetAll();
        IEnumerable<Layer> GetLayersInWorkspace(WorkSpace workSpace);
        TematicLayer GetTematicLayer(int id);
        IUnitOfWork GetUnitOfWork();
        CRUD_Service<TematicLayer> GetCRUD();
        IEnumerable<VectorStyle> GetTematicLayerStyles(int id);
        IEnumerable<Layer> GetTematicLayerLayers(int id);
        IEnumerable<TematicType> GetTematicLayerFilters(int id);
        IEnumerable<Layer> GetAllLayers();
        IEnumerable<TematicType> GetAllFilters();
        IDictionary<string, List<string>> getTablesColumns(Layer layer);
        bool createTematic(TematicViewModel tematic, TematicTypes _type);
        void DeleteTypesWithOutTematicLayer(TematicType item);
        TematicType CreateTematicType(string filterName, string[] listFieldTable, string[] listOperators, string[] listValues, string[] listTypes, string[] queryList, TematicTypes _type);
        bool DeleteTematicType(TematicLayer tematicLayer);
        (bool, List<object>) GetGeometryValue(Layer layer, string field);
        IEnumerable<TematicInfo> GetStyleConfigs(string _tematic);
        bool EditTematic(TematicViewModel tematic, TematicTypes _type);
        void deleteNonUsableTematics();
    }
}
