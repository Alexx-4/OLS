using System;
using System.Collections.Generic;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;

namespace OpenLatino.MapServer.Domain.Map.Render
{
    public delegate VectorStyle CustomStyle(Geometry geometry,
        IEnumerable<KeyValuePair<Func<Geometry, bool>, VectorStyle>> filters);
}