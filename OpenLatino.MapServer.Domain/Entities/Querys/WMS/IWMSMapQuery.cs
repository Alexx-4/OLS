using System;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;

namespace OpenLatino.MapServer.Domain.Entities.Querys.WMS
{
    public interface IWMSMapQuery : IWMSQuery
    {
        
        new string ProviderGeoField { get; set; }
        string ProviderTable { get; set; }
        string Crs { get; set; }
        new string InfoColums { get; set; }

    }
}
