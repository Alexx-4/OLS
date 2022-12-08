using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using Brushes = System.Drawing.Brushes;
using Brush = System.Drawing.Brush;
using Bitmap = System.Drawing.Bitmap;
using Graphics = System.Drawing.Graphics;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using GdiMatrix = System.Drawing.Drawing2D.Matrix;
using System.Drawing.Drawing2D;
using PointF = System.Drawing.PointF;
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using OpenLatino.MapServer.Domain.Map.OpenMath;
using OpenLatino.MapServer.Domain.Entities;
using OpenLatino.Core.Domain;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.MapperStyle;

namespace OpenLatino.MapServer.Domain.Map.Render
{
    public class LayerRender : ILayerRender
    {
        private class GeometryRenderFactory
        {
            private static IEnumerable<Func<Geometry, LayerRender, Func<Point, Point>, bool>> geoRenders = new Func<Geometry, LayerRender, Func<Point, Point>, bool>[]
            {
                    RenderPoint,
                    RenderLineString,
                    RenderGeometryCollection,
                    RenderPolygon
            };

            private static Point ToPaint(LtnMatrix matup, LtnMatrix matdown, Point p1, Point p2, Point p)
            {
                return (PosToRect(p, p1, p2) >= 0 ? matup : matdown).TransformPoint(p);
            }

            private static double PosToRect(Point p, Point p1, Point p2)
            {
                return ((p.Y - p1.Y) * (p2.X - p1.X)) - ((p2.Y - p1.Y) * (p.X - p1.X));
            }

            public static void Render(Geometry geo, LayerRender layer)
            {
                Func<Point, Point> transformation = p => ToPaint(layer.MatrixTransformationUp, layer.MatrixTransformationDown, layer.BBOX.Item1, layer.BBOX.Item2, p);

                bool flag = false;
                foreach (var item in geoRenders)
                    if (flag = item(geo, layer, transformation)) break;

                if (!flag)
                    throw new NotImplementedException("No existe un render implementado para el tipo de geometría entrado");
            }

            private static bool RenderPoint(Geometry p, LayerRender render, Func<Point, Point> transformation)
            {
                if (p.GeometryType != GeometryType.Point) return false;
                var point = p as Point;

                var vectorStyle = render.CustomStyle(p, render.Filters) ?? render.DefaultVectorStyle;

                var style = MapperStyle.ToSysDrawingStyle(vectorStyle);

                var g = render.gbit;

                var r = transformation(point);
                float[] coord = { Convert.ToSingle(r.X), Convert.ToSingle(r.Y) };

                var width = style.PointSize;
                var height = style.PointSize;
                var x = coord[0] - width / 2f;
                var y = coord[1] - height / 2f;
                var transform = g.Transform;
                var scale = style.ImageScale == 0 ? 1 : style.ImageScale;

                //var m = new GdiMatrix();
                //m.RotateAt(style.SymbolRotation, new PointF(Convert.ToSingle(x), Convert.ToSingle(y)));
                //m.Scale(scale, scale);

                //g.Transform = m;

                if (style.Image != null)
                    g.DrawImage(style.Image, x, render.Height - y, width, height);
                else
                    g.FillRectangle(style.PointBrush, x, render.Height - y, width, height);

                g.Transform = transform;

                return true;
            }

            private static IEnumerable<Point> TransformPoints(Func<Point, Point> transformation, params Point[] points)
            {
                foreach (var point in points)
                    yield return transformation(point);
            }

            private static bool RenderLineString(Geometry l, LayerRender render, Func<Point, Point> transformation)
            {
                if (l.GeometryType != GeometryType.LineString) return false;
                var lineString = l as LineString;

                var vectorStyle = render.CustomStyle(l, render.Filters) ?? render.DefaultVectorStyle;

                var style = MapperStyle.ToSysDrawingStyle(vectorStyle);

                var g = render.gbit;
                var gPath = new GraphicsPath();

                var lastPoint = transformation(lineString.StartPoint);
                for (int i = 1; i < lineString.NumPoints; i++)
                {
                    var currentPoint = transformation(lineString[i]);
                    gPath.AddLine(Convert.ToSingle(lastPoint.X), render.Height - Convert.ToSingle(lastPoint.Y), Convert.ToSingle(currentPoint.X), render.Height - Convert.ToSingle(currentPoint.Y));
                    lastPoint = currentPoint;
                }

                if (lineString.IsClosed)
                    gPath.CloseFigure();

                g.DrawPath(style.LineStyle, gPath);

                return true;
            }

            private static bool RenderGeometryCollection(Geometry c, LayerRender render, Func<Point, Point> transformation)
            {
                if (c.GeometryType != GeometryType.GeometryCollection) return false;
                var collection = c as GeometryCollection;

                for (int i = 0; i < collection.NumGeometries; i++)
                {
                    bool flag = false;
                    foreach (var func in geoRenders)
                        if (func(collection[i], render, transformation))
                        {
                            flag = true;
                            break;
                        }

                    if (!flag)
                        return false;
                }

                return true;
            }

            private static bool RenderPolygon(Geometry p, LayerRender render, Func<Point, Point> transformation)
            {
                if (p.GeometryType != GeometryType.Polygon) return false;
                var polygon = p as Polygon;

                var vectorStyle = render.CustomStyle(p, render.Filters) ?? render.DefaultVectorStyle;

                var style = MapperStyle.ToSysDrawingStyle(vectorStyle);

                var g = render.gbit;
                using (var gp = new GraphicsPath())
                {
                    var points = polygon.ExteriorRing.Points.Select(transformation).Select(pp => new PointF(Convert.ToSingle(pp.X), render.Height - Convert.ToSingle(pp.Y))).ToArray();
                    gp.AddPolygon(points);

                    foreach (var interiorRing in polygon)
                    {
                        points = interiorRing.Points.Select(transformation).Select(pp => new PointF(Convert.ToSingle(pp.X), render.Height - Convert.ToSingle(pp.Y))).ToArray();
                        gp.AddPolygon(points);
                    }

                    g.FillPath(style.FillStyle, gp);

                    if (style.EnableOutLine)
                    {
                        g.DrawPath(style.OutLineStyle, gp);
                    }
                }

                return true;
            }
        }
        private ConcurrentQueue<Element> q = new ConcurrentQueue<Element>();
        private MemoryStream surface = new MemoryStream();
        private object lockImage = new object();
        private object lockQueque = new object();

        public int[] Size { get; set; }

        public Tuple<Point, Point> BBOX { get; set; }

        public bool Transparent { get; set; }

        public Brush Background { get; set; }

        public int Width
        {
            get { return Size[0]; }
            set
            {
                if (value <= 0)
                    throw new InvalidDataException();
                Size[0] = value;
            }
        }

        public int Height
        {
            get { return Size[1]; }
            set
            {
                if (value <= 0)
                    throw new InvalidDataException();
                Size[1] = value;
            }
        }

        public bool ImageRendered { get; set; } = false;

        public string Format { get; set; }

        public OpenMath.LtnMatrix MatrixTransformationUp { get; set; }

        public OpenMath.LtnMatrix MatrixTransformationDown { get; set; }

        public CustomStyle CustomStyle { get; set; }
        public VectorStyle DefaultVectorStyle { get; set; }
        public IEnumerable<KeyValuePair<Func<Geometry, bool>, VectorStyle>> Filters { get; set; }

        private Bitmap bitmap;

        private Graphics gbit;

        private void InitializeBitmap()
        {
            if (bitmap == null)
            {
                bitmap = new Bitmap(Size[0], Size[1]);
                gbit = Graphics.FromImage(bitmap);

                if (Transparent)
                    gbit.FillRectangle(Brushes.Transparent, 0, 0, Width, Height);
                else
                    gbit.FillRectangle(Background, 0, 0, Width, Height);
            }
        }


        private void UpdateImage()
        {
            if (q.Count > 1) return;
            Task.Factory.StartNew(() => RenderImage());
        }
        private void RenderImage()
        {
            lock (lockImage)
            {
                Element currentElement;
                while (true)
                {
                    bool flag;
                    lock (lockQueque) { flag = q.TryDequeue(out currentElement); }
                    if (!flag) break;
                    GeometryRenderFactory.Render(currentElement as Geometry, this);
                    ImageRendered = true;
                }
            }
        }

        private ImageFormat GetFormat()
        {
            if (Format.Contains("png")) return ImageFormat.Png;
            else if (Format.Contains("jpeg")) return ImageFormat.Jpeg;
            else throw new InvalidOperationException("Invalid format");
        }

        public IMapImage GetImage()
        {
            lock (lockImage)
            {
                InitializeBitmap();  //puede quitarse!!!!!(Frankie)
                ImageRendered = true;
                bitmap.Save(surface, GetFormat());
                return new MapImage(surface);
            }
        }

        public object Clone()
        {
            return new LayerRender();
        }

        public void Dispose()
        {
            if (bitmap != null)
            {
                gbit.Dispose();
                bitmap.Dispose();
                surface.Dispose();
            }
        }

        public void AddElement(Element element)
        {
            InitializeBitmap();
            lock (lockQueque) { q.Enqueue(element); }
            UpdateImage();
        }
    }
}
