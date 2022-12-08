using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System.Collections.Generic;

namespace OpenLatino.MapServer.Domain.Entities.FenceBuilder
{
    /// <summary>
    /// Class for Create The Geometries give a range
    /// </summary>
    public sealed class ShapeFenceBuilderSingleton : IFenceBuilderSingleton
    {
        private ShapeFenceBuilderSingleton()
        {
            _diccBuilder = new Dictionary<BorderShape, Builder>();
            //Initialice();
        }

        public static ShapeFenceBuilderSingleton Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly ShapeFenceBuilderSingleton instance = new ShapeFenceBuilderSingleton();
        }

        private delegate Geometry Builder(string fence);

        private Dictionary<BorderShape, Builder> _diccBuilder;

        public Geometry Build(BorderShape fenceShape, string fence)
        {
            if (_diccBuilder.ContainsKey(fenceShape))
                return _diccBuilder[fenceShape](fence);
            return null;
        }

        private void Initialice()
        {

        }

        private Geometry BuildPolygon(string fence)
        {
            var pointlist = ParsePoints(fence);
            LinearRing extRing = new LinearRing(pointlist);
            return new Polygon(extRing);
        }

        private List<Point> ParsePoints(string points)
        {
            List<Point> result = new List<Point>();
            // Separa los puntos
            var listPoint = points.Split('*');
            foreach (var pointstring in listPoint)
            {
                //separo las componentes del punto por ;
                var pointstcomponentlist = pointstring.Split(';');
                double x = double.Parse(pointstcomponentlist[0]);
                double y = double.Parse(pointstcomponentlist[1]);
                result.Add(new Point(x, y));
            }
            return result;
        }

        private Geometry BuildCircle(string fence)
        {
            var circleComponent = fence.Split('*');
            var circlecenter = circleComponent[0].Split(';');
            double x = double.Parse(circlecenter[0]);
            double y = double.Parse(circlecenter[1]);
            var center = new Point(x, y);
            var ratio = double.Parse(circleComponent[1]);

            return new Circle(center, ratio);
        }
    }
}