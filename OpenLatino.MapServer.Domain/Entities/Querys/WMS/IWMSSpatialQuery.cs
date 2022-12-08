using OpenLatino.MapServer.Domain.Entities.Auxiliars;

namespace OpenLatino.MapServer.Domain.Entities.Querys.WMS
{
    public interface IWMSSpatialQuery : IWMSQuery
    {
        string Crs { get; set; }

        string Top { get; set; }

        new string ProviderGeoField { get; set; }

        string ProviderTable { get; set; }


        string[] GeometryText { get; set; }

        EnclousureType[] Clousure { get; set; }

        string[] SetOperation { get; set; }


        new string InfoColums { get; set; }

    }
}
