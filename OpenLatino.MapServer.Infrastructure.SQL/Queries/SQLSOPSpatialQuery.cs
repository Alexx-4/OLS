using OpenLatino.MapServer.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenLatino.MapServer.Domain.Entities.Querys.SOP;
using OpenLatino.MapServer.Domain.Entities.FileData;
using OpenLatino.Core.Domain.Entities;

namespace OpenLatino.MapServer.Infrastucture.SQL.DataSource
{
    public class SQLSOPSpatialQuery : ISOPSpatialQuery
    {
        public string Crs { get; set; }
        public string Top { get; set; }
        public string ProviderGeoField { get; set; }
        public string ProviderPkField { get; set; }
        public string ProviderTable { get; set; }

        public string[] GeometryText { get; set; }

        public EnclousureType[] Clousure { get; set; }
        public string[] SetOperation { get; set; }

        public Type Provider { get; set; } = typeof(ProviderSQL);

        public Type[] TypesReturn { get; set; }

        public string splitOn { get; set; }

        public string InfoColums { get; set; }
        public Tuple<Point, Point> BoundingBox { get; set; }

        public object ConverterFunction(object[] parameters)
        {
            return parameters;
        }

        public object GetQuery()
        {
            WKTReader reader = new WKTReader();
            string query_result = "DECLARE ";

            //creating the Declare
            for (int i = 0; i < GeometryText.Length; i++)
            {
                if (i != GeometryText.Length - 1)
                    query_result += $@"@g{i} Geometry,";
                else query_result += $@"@g{i} Geometry ";

            }
            //creating the Set
            for (int i = 0; i < GeometryText.Length; i++)
            {
                Geometry current_geom = reader.GetGeometry(GeometryText[i]);

                if (current_geom is Circle)
                {
                    query_result += $@"SET @g{i}=Geometry::STGeomFromText('POINT({(current_geom as Circle).Center.X} {(current_geom as Circle).Center.Y})',{Crs}).STBuffer({(current_geom as Circle).Ratio}) ";
                }
                else if (current_geom is Polygon)
                {
                    query_result += $@"SET @g{i} = Geometry::STGeomFromText('{current_geom.ToWKTString()}',{ Crs}) ";
                }
                else if (current_geom is LineString)
                {
                    query_result += $@"SET @g{i} = Geometry::STGeomFromText('{current_geom.ToWKTString()}', { Crs}) ";
                }
                else if (current_geom is Point)
                {
                    query_result += $@"SET @g{i} = Geometry::STGeomFromText('POINT({(current_geom as Point).X} {(current_geom as Point).Y})',{ Crs}) ";
                }

            }


            //creating the query
            for (int i = 0; i < GeometryText.Length; i++)
            {
                Geometry current_geom = reader.GetGeometry(GeometryText[i]);
                EnclousureType current_enclousure = Clousure[i];
                string current_setOperation = SetOperation[i];
                string current_query = "";

                if (current_geom is Circle) current_query = GetCircleQuery(current_geom as Circle, current_enclousure, i);
                else if (current_geom is Polygon) current_query = GetPolygonQuery(current_geom as Polygon, current_enclousure, i);
                else if (current_geom is LineString) current_query = GetLineStringQuery(current_geom as LineString, current_enclousure, i);
                else if (current_geom is Point) current_query = GetPointQuery(current_geom as Point, i);

                if (i != GeometryText.Length - 1)
                    query_result += current_query + " " + current_setOperation + " ";
                else
                    query_result += current_query;

            }

            return query_result;
        }
        private string GetCircleQuery(Circle circle, EnclousureType clousure, int i)
        {
            switch (clousure)
            {
                case EnclousureType.In:
                    return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STContains({ProviderGeoField})=1";
                case EnclousureType.Out:
                    return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STIntersects({ProviderGeoField})=0";
                case EnclousureType.Over:
                    return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STBoundary().STIntersects({ProviderGeoField})=1";
                case EnclousureType.InOver:
                    return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STIntersects({ProviderGeoField})=1";
                case EnclousureType.OutOver:
                    return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STContains({ProviderGeoField})=0";
                default:
                    break;
            }
            return null;
        }
        private string GetPolygonQuery(Polygon polygon, EnclousureType clousure, int i)
        {
            switch (clousure)
            {
                case EnclousureType.In:
                    return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STContains({ProviderGeoField})=1";
                case EnclousureType.Out:
                    return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STIntersects({ProviderGeoField})=0";
                case EnclousureType.Over:
                    return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STBoundary().STIntersects({ProviderGeoField})=1";
                case EnclousureType.InOver:
                    return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STIntersects({ProviderGeoField})=1";
                case EnclousureType.OutOver:
                    return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STContains({ProviderGeoField})=0";
                default:
                    break;
            }
            return null;
        }
        private string GetLineStringQuery(LineString line, EnclousureType clousure, int i)
        {
            if (clousure == EnclousureType.In || clousure == EnclousureType.InOver || clousure == EnclousureType.Over)
            {
                return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STIntersects({ProviderGeoField})=1";
            }
            if (clousure == EnclousureType.Out)
            {
                return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STIntersects({ProviderGeoField})=0";
            }
            if (clousure == EnclousureType.OutOver)
            {
                return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable}";
            }
            return null;
        }
        private string GetPointQuery(Point point, int i)
        {
            return $@"SELECT {ProviderPkField},{ProviderGeoField}.ToString() as ogr_geometry,{InfoColums} FROM {ProviderTable} WHERE @g{i}.STIntersects({ProviderGeoField})=1";
        }

        public IEnumerable<object> GetGeometries<T>(IFileDataStructure<T> datastruct) where T : IFeature
        {
            throw new NotImplementedException();
        }
    }
}
