using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.MapServer.Domain.Map.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenLatino.MapServer.Domain.Map.Filters.Enums;
using OpenLatino.MapServer.Domain.Map.Filters.Helpers;
using OpenLatino.Core.Domain.Models;
using System.Threading.Tasks;
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Infrastucture.SQL.DataSource;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using Unity;
using OpenLatino.MapServer.Infrastructure.SQL.Queries;
using OpenLatino.MapServer.Infrastructure.SQL.DataSource;

namespace OpenLatino.Admin.Application.Services
{
    public class TematicLayerService : CRUD_Service<TematicLayer>, ITematicLayerHelper
    {
        private IRepository<VectorStyle> stylesRepo;
        private IRepository<Layer> layersRepo;
        private IRepository<TematicType> filtersRepo;
        private IRepository<AlfaInfo> alfaInfoRepo;
        private IRepository<StyleConfig> styleConfigRepo;

        public TematicLayerService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.stylesRepo = unitOfWork.Set<VectorStyle>();
            this.layersRepo = unitOfWork.Set<Layer>();
            this.filtersRepo = unitOfWork.Set<TematicType>();
            this.alfaInfoRepo = unitOfWork.Set<AlfaInfo>();
            this.styleConfigRepo = unitOfWork.Set<StyleConfig>();
            
        }

        public IEnumerable<TematicLayer> GetAll()
        {
            return repository.GetAll(l => true);
        }


        public TematicLayer GetTematicLayer(int id)
        {
            return repository.GetAll(l => l.Id == id, true).FirstOrDefault();
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return unitOfWork;
        }

        public CRUD_Service<TematicLayer> GetCRUD()
        {
            return this;
        }
        public IEnumerable<Layer> GetLayersInWorkspace(WorkSpace workSpace)
        {

            return layersRepo.GetAll(ll => workSpace.LayerWorkspaces.Any(lw => lw.LayerId == ll.Id), true, l => l.LayerTranslations);
        }
        public IEnumerable<VectorStyle> GetTematicLayerStyles(int id)
        {
            return stylesRepo.GetAll(sty => sty.StyleConfiguration.Any(sc => sc.TematicLayerId == id), true);
        }

        public IEnumerable<Layer> GetTematicLayerLayers(int id)
        {
            return layersRepo.GetAll(l => l.StyleConfiguration.Any(sc => sc.TematicLayerId == id));
        }
        public IEnumerable<TematicType> GetTematicLayerFilters(int id)
        {
            return filtersRepo.GetAll(f => f.StyleConfiguration.Any(sc => sc.TematicLayerId == id), true);
        }
        public IEnumerable<Layer> GetAllLayers()
        {
            return layersRepo.GetAll(ly => true).ToList();
        }

        public IEnumerable<TematicType> GetAllFilters()
        {
            
            return filtersRepo.GetAll(fl => true).ToList();
        }
        public void DeleteTypesWithOutTematicLayer(TematicType item)
        {
            filtersRepo.Remove(item);
            unitOfWork.SaveChanges();
        }

        public virtual TematicType CreateTematicType(string filterName, string[] listFieldTable, string[] listOperators, string[] listValues, string[] listTypes, string[] queryList, TematicTypes _type)
        {
            List<GeometryFilter> listfilters = new List<GeometryFilter>();
            TematicType _filter = null;
            for (int i = 0; i < listFieldTable.Length; i++)
            {
                string[] fieldTable = listFieldTable[i].Split(',');
                string[] operators = listOperators[i].Split(',');
                var myenum = Enum.GetValues(typeof(ComparisonOperator));
                ComparisonOperator co = ComparisonOperator.Contains;
                for (int j = 0; j < operators.Length; j++)
                {
                    foreach (var item in myenum)
                    {
                        if (item.ToString() == operators[j])
                            co = (ComparisonOperator)item;
                    }
                    dynamic value = listValues[i];
                    if (listTypes[i] == "number")
                        value = decimal.Parse(listValues[i]);
                    var clause = new Clause
                    {
                        Source = InfoSource.Geometry,
                        Name = fieldTable[0],
                        Operator = co,
                        Value = value
                    };
                    var gFilter = new GeometryFilter(clause);
                    listfilters.Add(gFilter);
                }
            }

            if (listfilters.Count == 1)
            {
                _filter = filtersRepo.GetAll().Where(f => f.Name == filterName).FirstOrDefault();
                if (_filter != null)
                    return _filter;

                var _tematic = new TematicType();
                switch (_type)
                {
                    case TematicTypes.CategoryTematic:
                        _tematic = new TematicTypesClasification(); 
                        break;
                    case TematicTypes.QueryTematic:
                        _tematic = new TematicQuery();
                        break;
                    default:
                        break;
                }

                _tematic.Name = filterName;
                _tematic.Function = FilterSerializer.FilterExpressionToByteArray(listfilters[0]);

                _filter = filtersRepo.Add(_tematic);
                unitOfWork.SaveChanges();
            }
            else if(listfilters.Count > 1)
            {
                if (queryList.Length > 0)
                {
                    LogicalOperator lo = queryList[0] == "And" ? LogicalOperator.And : LogicalOperator.Or;
                    var binaryFilter = new BinaryFilter(listfilters[0], listfilters[1], lo);
                    for (int i = 2; i < listfilters.Count; i++)
                    {
                        lo = queryList[i-1] == "And" ? LogicalOperator.And : LogicalOperator.Or;
                        binaryFilter = new BinaryFilter(binaryFilter, listfilters[i], lo);
                    }

                    _filter = filtersRepo.GetAll().Where(f => f.Name == filterName).FirstOrDefault();
                    if (_filter != null)
                        return _filter;


                    TematicType _tematic = null;
                    switch (_type)
                    {
                        case TematicTypes.CategoryTematic:
                            _tematic = new TematicTypesClasification();
                            break;

                        case TematicTypes.QueryTematic:
                            _tematic = new TematicQuery();
                            break;
                        default:
                            break;
                    }

                    _tematic.Name = filterName;
                    _tematic.Function = FilterSerializer.FilterExpressionToByteArray(binaryFilter);

                    _filter = filtersRepo.Add(_tematic);
                    unitOfWork.SaveChanges();
                    
                }
            }
            return _filter;
        }

        public virtual bool DeleteTematicType(TematicLayer tematicLayer)
        {
            var configs = styleConfigRepo.GetAll(s => s.TematicLayerId == tematicLayer.Id).ToList();
            foreach(var item in configs)
                styleConfigRepo.Remove(item);

            unitOfWork.SaveChanges();
            return true;
        }

        public IDictionary<string, List<string>> getTablesColumns(Layer layer)
        {
            List<AlfaInfo> alfaInfos = alfaInfoRepo.GetAll(a=>a.LayerId == layer.Id).ToList();
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

            for (int i = 0; i < alfaInfos.Count; i++)
            {
                AlfaInfo info = alfaInfos[i];
                result[info.Table] = info.Columns.Split(",").ToList();
            }
            return result;
        }

        public bool createTematic(TematicViewModel tematic, TematicTypes _type)
        {
            TematicLayer newTematic;
            if (tematic.tematicId == null)
            {
                newTematic = repository.Add(new TematicLayer { Name = tematic.tematicName });
                unitOfWork.SaveChanges();
            }
            else
            {
                newTematic = repository.GetAll(l => l.Id == tematic.tematicId).FirstOrDefault();
            }

            foreach (TematicViewModel.query _query in tematic.queries)
            {
                Layer _layer = layersRepo.GetAll(null, true, l => l.LayerTranslations)
                                         .Where(l => l.LayerTranslations.FirstOrDefault().Name == _query.layerName)
                                         .FirstOrDefault();

                AlfaInfo _info = alfaInfoRepo.GetAll(ai => ai.Table == _query.tableName && ai.LayerId == _layer.Id).FirstOrDefault();
                VectorStyle _style = stylesRepo.GetAll(s => s.Name == _query.styleName).FirstOrDefault();

                List<string> listField = new List<string>();
                List<string> listOperators = new List<string>();
                List<string> listValues = new List<string>();
                List<string> listTypes = new List<string>();
                List<string> listQuery = new List<string>();

                string filterName = _query.layerName + " " + _query.tableName + ";";

                foreach(TematicViewModel.Condition _condition in _query.conditions)
                {
                    listField.Add(_condition.columnName + "," + _query.tableName);

                    filterName += (_condition.logicOperator != null) ? _condition.logicOperator + "," : "";
                    filterName += _condition.columnName + "," + _condition._operator
                                                        + "," + _condition.value
                                                        + ";";

                    switch (_condition._operator)
                    {
                        case "<":
                            listOperators.Add(ComparisonOperator.LessThan.ToString());
                            listTypes.Add("number");
                            break;
                        case "<=":
                            listOperators.Add(ComparisonOperator.LessThanOrEqual.ToString());
                            listTypes.Add("number");
                            break;
                        case "!=":
                            listOperators.Add(ComparisonOperator.NotEqual.ToString());
                            listTypes.Add("number");
                            break;
                        case "==":
                            listOperators.Add(ComparisonOperator.Equal.ToString());
                            listTypes.Add("number");
                            break;
                        case ">":
                            listOperators.Add(ComparisonOperator.GreaterThan.ToString());
                            listTypes.Add("number");
                            break;
                        case ">=":
                            listOperators.Add(ComparisonOperator.GreaterThanOrEqual.ToString());
                            listTypes.Add("number");
                            break;
                        case "Equal":
                            listOperators.Add(ComparisonOperator.Contains.ToString());
                            listTypes.Add("text");
                            break;
                        default:
                            listOperators.Add(_condition._operator);
                            listTypes.Add("text");
                            break;
                    }

                    listValues.Add(_condition.value.ToLower());

                    if (_condition.logicOperator != null)
                        listQuery.Add(_condition.logicOperator);

                }
                TematicType _filter = CreateTematicType(filterName.Remove(filterName.Length-1), listField.ToArray(), 
                                                                                                listOperators.ToArray(), 
                                                                                                listValues.ToArray(), 
                                                                                                listTypes.ToArray(), 
                                                                                                listQuery.ToArray(), _type);

                StyleConfig _config = new StyleConfig
                {
                    LayerId = _info.LayerId,
                    TematicLayerId = newTematic.Id,
                    StyleId = _style.Id,
                    TematicTypeId = _filter.Id
                };

                styleConfigRepo.Add(_config);
                unitOfWork.SaveChanges();
            }
            return true;
        }

        public (bool,List<object>) GetGeometryValue(Layer layer, string field)
        {
            IWMSQuery query = new SQLWMSCategoryTematic()
            {
                ProviderPkField = field,
                ProviderGeoField = layer.AlfaInfoes.First().Table
            };

            IProviderService provider = new DBContextProvider()
            {
                ConnectionString = layer.AlfaInfoes.First().ConnectionString
            };

            var temp = (provider.ExecuteYield(query));

            var categories = new List<object>();
            int dontCare = 0;
            bool isString = false;
            foreach (Dictionary<string, object> item  in temp)
            {
                if (!(int.TryParse(item.Values.First().ToString(), out dontCare)))
                    isString = true;
                    
               

                categories = categories.Concat(item.Values).ToList();
            }

            return (!isString, categories.ToList());

        }

        public IEnumerable<TematicInfo> GetStyleConfigs(string _tematic)
        {
            List<TematicInfo> configs = new List<TematicInfo>();
            var allConfigs = styleConfigRepo.GetAll(null, true, s => s.Layer,
                                                                    s => s.Style,
                                                                    s => s.TematicLayer,
                                                                    s => s.TematicType)
                                            .Where(s => s.TematicType.GetType().Name == $"{_tematic}Proxy");
                                               
            foreach (var item in allConfigs)
            {
                AlfaInfo _alphaInfo = alfaInfoRepo.GetAll(ai => ai.LayerId == item.LayerId).FirstOrDefault();
                Layer _layer = layersRepo.GetAll(l => l.Id == item.LayerId, true, l => l.LayerTranslations).FirstOrDefault();

                TematicInfo info = new TematicInfo()
                {
                    tematicId = item.TematicLayerId,
                    tematicName = item.TematicLayer.Name,

                    tematicType = _tematic,

                    layerId = item.LayerId,
                    layerName = _layer.LayerTranslations.FirstOrDefault().Name,
                    tableName = _alphaInfo.Table,

                     tematicTypeId = item.TematicTypeId,
                     tematicTypeName = item.TematicType.Name,

                     styleId = item.StyleId,
                     styleName = item.Style.Name
                };

                configs.Add(info);
            }
            return configs;
            
        }
        public void deleteNonUsableTematics()
        {
            var tematicLayers = GetAll();
            foreach (var item in tematicLayers)
                if (item.StyleConfiguration.Count == 0 || item.StyleConfiguration.Any(sc => sc.TematicType == null))
                    GetCRUD().Remove(item);
            var tematicTypes = GetAllFilters();
            foreach (var item in tematicTypes)
                if (item.StyleConfiguration.Count == 0 || item.StyleConfiguration.Any(sc => sc.TematicLayer == null))
                    DeleteTypesWithOutTematicLayer(item);
        }

        public bool EditTematic(TematicViewModel tematic, TematicTypes _type)
        {
            TematicLayer _tematic = repository.Modify(new TematicLayer { Id = (int)tematic.tematicId, Name = tematic.tematicName });
            unitOfWork.SaveChanges();

            return DeleteTematicType(_tematic) && createTematic(tematic, _type);


        }
    }

}
