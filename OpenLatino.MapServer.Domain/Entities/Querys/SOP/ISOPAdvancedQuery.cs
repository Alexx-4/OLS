using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLatino.MapServer.Domain.Entities.Querys.SOP;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;

namespace OpenLatino.MapServer.Domain.Entities.Querys.WMS
{
    public interface ISOPAdvancedQuery : ISOPSpatialQuery
    {
        string[] Properties { get; set; }
        string[] Values { get; set; }
        string[] SetOperations { get; set; }
        string[] Functions { get; set; }
    }
}
