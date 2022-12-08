using OpenLatino.Core.Domain;
using OpenLatino.MapServer.Domain.Entities;
using OpenLatino.MapServer.Domain.Map.OpenMath;
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLatino.Core.Domain.Entities;
using Brush = System.Drawing.Brush;

namespace OpenLatino.MapServer.Domain.Map.Render
{
    public interface ILayerRender : ICloneable, IDisposable
    {
        CustomStyle CustomStyle { get; set; }

        IEnumerable<KeyValuePair<Func<Geometry, bool>, VectorStyle>> Filters { get; set; }

        VectorStyle DefaultVectorStyle { get; set; }

        bool ImageRendered { get; set; }

        IMapImage GetImage();

        void AddElement(Element element);

        Brush Background { get; set; }

        int Height { get; set; }

        int Width { get; set; }

        string Format { get; set; }

        //VectorStyle Style { get; set; }

        Tuple<Point, Point> BBOX { get; set; }

        int[] Size { get; set; }

        bool Transparent { get; set; }

        LtnMatrix MatrixTransformationUp { get; set; }

        LtnMatrix MatrixTransformationDown { get; set; }
    }
}
