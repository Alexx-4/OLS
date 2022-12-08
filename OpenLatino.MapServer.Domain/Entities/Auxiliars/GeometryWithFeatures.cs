using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Entities.Auxiliars
{
    public class GeometryWithFeatures : Element
    {
        public string ParentSetOperation { get; set; }
        public string IdGeomContainerForSpatialQuery { get; set; }
        public string GeometryText { get; set; }
        public List<Feature> Features { get; set; }

        public Geometry Geom { get; set; }

        public GeometryWithFeatures()
        {
            Features = new List<Feature>();
        }

    }

    public class Feature
    {
        public string NameFeature { get; set; }
        public string ValueFeature { get; set; }
    }
}

