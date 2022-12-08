using OpenLatino.MapServer.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Entities.FileData;
using OpenLatino.Core.Domain.Entities;

namespace OpenLatino.MapServer.Infrastucture.SQL.DataSource
{
    class SQLSOPAdvancedQuery : ISOPAdvancedQuery
    {
        public string[] Properties { get; set; }
        public string[] Values { get; set; }
        public string[] SetOperations { get; set; }
        public string[] Functions { get; set; }
        public Type Provider { get; set; } = typeof(ProviderSQL);
        public Type[] TypesReturn { get; set; }
        public string splitOn { get; set; }
        public string InfoColums { get; set; }
        public string ProviderGeoField { get; set; }
        public string ProviderPkField { get; set; }
        public string ProviderTable { get; set; }
        public Tuple<Point, Point> BoundingBox { get; set; }
        public string Crs { get; set; }
        public string Top { get; set; }
        public string[] GeometryText { get; set; }
        public EnclousureType[] Clousure { get; set; }
        public string[] SetOperation { get; set; }

        public object ConverterFunction(object[] parameters)
        {
            return parameters;
        }

        public IEnumerable<object> GetGeometries<T>(IFileDataStructure<T> datastruct) where T : IFeature
        {
            throw new NotImplementedException();
        }

        public object GetQuery()
        {


            string query = $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE ";
            

            for (int i = 0; i < Properties.Length; i++)
            {
                if (Values[i] == null)
                    continue;
                else if (Properties[i]=="IdGeometry")
                {
                    if (Functions[i] == "Equal")
                    {
                        query += $@"osm_id  = '{Values[i]}' {SetOperations[i]} ";
                    }
                    else if (Functions[i] == "Contains")
                    {
                        query += $@" CHARINDEX('{Values[i]}' , osm_id) = 1 {SetOperations[i]} ";
                    }
                }
                else if (Properties[i] == "Area")
                {
                    query += $@"{ProviderGeoField}.STArea()*10000000000 {Functions[i]} '{Values[i]}' {SetOperations[i]} ";
                }
                else if (Properties[i] == "Perimeter")
                {
                    query += $@"{ProviderGeoField}.STLength()*100000 {Functions[i]} '{Values[i]}' {SetOperations[i]} ";
                }
                else if(Functions[i]=="Contains")
                {
                    query += $@" CHARINDEX('{Values[i]}' , {Properties[i]}) = 1 {SetOperations[i]} ";
                }
                else if (Functions[i] == "=b")
                {
                    if(Values[i]=="true")
                        query += $@"{Properties[i]} = 1 {SetOperations[i]} ";
                    else
                        query += $@"{Properties[i]} = 0 {SetOperations[i]} ";
                }
                else
                {
                    query += $@"{Properties[i]} {Functions[i]} '{Values[i]}' {SetOperations[i]} ";
                }
            }


            return query.Substring(0,query.Length-5);

        }
    }
}
