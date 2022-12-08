using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Entities.Auxiliars
{
    public class SpatialGeometriesRequests
    {
        //For the range query
        public int IdGeometry { get; set; }
        public string GeometryText { get; set; }
        public EnclousureType Clousure { get; set; }
        public string SetOperation { get; set; }

        public string boundingBox { get; set; }

        //For the Advanced Query
        public string Property { get; set; }
        public string Value { get; set; }
        public string Function { get; set; }
    }
}
