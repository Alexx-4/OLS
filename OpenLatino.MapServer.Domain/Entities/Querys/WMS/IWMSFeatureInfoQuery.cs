using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;

namespace OpenLatino.MapServer.Domain.Entities.Querys.WMS
{
    public interface IWMSFeatureInfoQuery : IWMSQuery
    {
        string Crs { get; set; }

        int Feature_Count { get; set; }

        Point Point { get; set; }

        new string ProviderGeoField { get; set; }

        new string ProviderPkField { get; set; }

        string ProviderTable { get; set; }

        new string InfoColums { get; set; }
    }
}
