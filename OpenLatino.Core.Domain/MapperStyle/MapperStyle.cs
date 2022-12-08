using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenLatino.Core.Domain.Entities;

namespace OpenLatino.Core.Domain.MapperStyle
{
    public static class MapperStyle
    {

        public static SysDrawingVectorStyle ToSysDrawingStyle(VectorStyle vectorStyle)
        {

            return new SysDrawingVectorStyle
            {
                Name = vectorStyle.Name,
                FillStyle = !(vectorStyle.Fill is null) ? new SolidBrush(Color.FromArgb(int.Parse(vectorStyle.Fill.Split(",")[0]),
                                                                                     int.Parse(vectorStyle.Fill.Split(",")[1]),
                                                                                     int.Parse(vectorStyle.Fill.Split(",")[2])))
                                                     : new SolidBrush(Color.White),

                LineStyle = !(vectorStyle.Line is null) ? new Pen(Color.FromArgb(int.Parse(vectorStyle.Line.Split(",")[0]),
                                                                              int.Parse(vectorStyle.Line.Split(",")[1]),
                                                                              int.Parse(vectorStyle.Line.Split(",")[2])))
                                                     : new Pen(Color.White),

                EnableOutLine = vectorStyle.EnableOutline,

                OutLineStyle = !(vectorStyle.OutlinePen is null) ? new Pen(Color.FromArgb(int.Parse(vectorStyle.OutlinePen.Split(",")[0]),
                                                                                       int.Parse(vectorStyle.OutlinePen.Split(",")[1]),
                                                                                       int.Parse(vectorStyle.OutlinePen.Split(",")[2])))
                                                              : new Pen(Color.White),

                PointBrush = !(vectorStyle.PointFill is null) ? new SolidBrush(Color.FromArgb(int.Parse(vectorStyle.PointFill.Split(",")[0]),
                                                                                           int.Parse(vectorStyle.PointFill.Split(",")[1]),
                                                                                           int.Parse(vectorStyle.PointFill.Split(",")[2])))
                                                           : new SolidBrush(Color.White),

                Image = null,

                ImageScale = vectorStyle.ImageScale,
                ImageRotacion = vectorStyle.ImageRotation,
                PointSize = vectorStyle.PointSize
            };
        }

        public static VectorStyle ToVectorStyle(SysDrawingVectorStyle sysDrawingVectorStyle)
        {
            MemoryStream ms = null;
            if (sysDrawingVectorStyle.Image != null)
            {
                ms = new MemoryStream();
                sysDrawingVectorStyle.Image.Save(ms, ImageFormat.Png);
            }

            return new VectorStyle
            {                
                Name = sysDrawingVectorStyle.Name,
                Fill = ((SolidBrush)sysDrawingVectorStyle.FillStyle).Color.Name,
                Line = sysDrawingVectorStyle.LineStyle.Color.Name,
                EnableOutline = sysDrawingVectorStyle.EnableOutLine,
                OutlinePen = sysDrawingVectorStyle.OutLineStyle.Color.Name,
                PointFill = ((SolidBrush)sysDrawingVectorStyle.PointBrush).Color.Name,
                ImageContent = ms?.ToArray(),
                ImageScale = sysDrawingVectorStyle.ImageScale,
                ImageRotation = sysDrawingVectorStyle.ImageRotacion

            };
        }
    }
}