using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.FenceBuilder;
using OpenLatino.MapServer.Domain.Entities.SpatialOperation.Clousere;
using OpenLatino.MapServer.Domain.Entities.SpatialOperation.Set;
using RBush;
using System.Collections.Generic;

namespace OpenLatino.MapServer.Domain.Entities.Querys.SOP
{
    public interface ISOPSpatialQuery : IQuery
    {
        string Crs { get; set; }

        string Top { get; set; }


        string ProviderTable { get; set; }


        string[] GeometryText { get; set; }

        EnclousureType[] Clousure { get; set; }

        string[] SetOperation { get; set; }

    }
}