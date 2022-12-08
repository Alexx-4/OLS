using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Entities;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using OpenLatino.Core.Domain;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.Filters;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys;
using System.Data.SqlTypes;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Infrastucture.SQL.DataSource
{
    public class ProviderSQL : ISQLProviderService
    {
        public string BoundingBoxField { get; set; }
        public string ConnectionString { get; set; }
        public string GeoField { get; set; }
        public int ID { get; set; }
        public string PkField { get; set; }
        public string Table { get; set; }
        public IEnumerable<IFilter> Filter { get; set; } = new List<IFilter>() { new DumbFilter() };
        public IEnumerable<Layer> Layers { get; set; }
        public IEnumerable<object> ExecuteYield(IQuery query)
        {
            string q = query.GetQuery().ToString();
            List<GeometryWithFeatures> result = new List<GeometryWithFeatures>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                var geoStringList = conn.Query(q, query.TypesReturn, query.ConverterFunction, splitOn: query.splitOn);
                var columns = query.InfoColums.Split(',');

                foreach (object[] item in geoStringList)
                {
                    var data = new Dictionary<string, object>();
                    data.Add("IdGeometry", item[0]);
                    data.Add("GeometryText", item[1]);
                    for (int i = 2; i < item.Length; i++)
                        data.Add(columns[i - 2], item[i]);
                    var wkt = new WKTReader();
                    var geom = wkt.GetGeometry(data["GeometryText"].ToString());
                    geom.Attributes = data;
                    var gwf = new GeometryWithFeatures();
                    gwf.Geom = geom;
                    gwf.Features = new List<Feature>();
                    foreach (var item1 in data)
                        gwf.Features.Add(new Feature() { NameFeature = item1.Key, ValueFeature = item1.Value is null ? "" : item1.Value.ToString() });
                    yield return gwf;
                }
            }
        }
        public IEnumerable<object> Execute(IQuery query)
        {
            string q = query.GetQuery().ToString();
            List<GeometryWithFeatures> result = new List<GeometryWithFeatures>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                var geoStringList = conn.Query(q, query.TypesReturn, query.ConverterFunction, splitOn: query.splitOn);
                var columns = query.InfoColums.Split(',');

                foreach (object[] item in geoStringList)
                {
                    var data = new Dictionary<string, object>();
                    data.Add("IdGeometry", item[0]);
                    data.Add("GeometryText", item[1]);
                    for (int i = 2; i < item.Length; i++)
                        data.Add(columns[i - 2], item[i]);
                    var wkt = new WKTReader();
                    var geom = wkt.GetGeometry(data["GeometryText"].ToString());
                    geom.Attributes = data;
                    var gwf = new GeometryWithFeatures();
                    gwf.Geom = geom;
                    gwf.Features = new List<Feature>();
                    foreach (var item1 in data)
                        gwf.Features.Add(new Feature() { NameFeature = item1.Key, ValueFeature = item1.Value is null? "":item1.Value.ToString()});
                    result.Add(gwf);
                }
                return result;
            }
        }

        public async Task<IEnumerable<object>> ExecuteAsync(IQuery query)
        {
            string q = query.GetQuery().ToString();
            List<GeometryWithFeatures> result = new List<GeometryWithFeatures>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                var geoStringList = await conn.QueryAsync(q, query.TypesReturn, query.ConverterFunction, splitOn: query.splitOn);
                var columns = query.InfoColums.Split(',');

                foreach (object[] item in geoStringList)
                {
                    var data = new Dictionary<string, object>();
                    data.Add("IdGeometry", item[0]);
                    data.Add("GeometryText", item[1]);
                    for (int i = 2; i < item.Length; i++)
                    {
                        Type _type = item[i] != null ? item[i].GetType() : typeof(object);
                        if (_type.Equals(typeof(string)))  data.Add(columns[i - 2], item[i].ToString().ToLower());
                        else data.Add(columns[i - 2], item[i]);
                    }
                        
                    var wkt = new WKTReader();
                    var geom = wkt.GetGeometry(data["GeometryText"].ToString());
                    geom.Attributes = data;
                    var gwf = new GeometryWithFeatures();
                    gwf.Geom = geom;
                    gwf.Features = new List<Feature>();
                    foreach (var item1 in data)
                        gwf.Features.Add(new Feature() { NameFeature = item1.Key, ValueFeature = item1.Value is null ? "" : item1.Value.ToString() });
                    result.Add(gwf);
                }
                return result;
            }
        }
    }
}