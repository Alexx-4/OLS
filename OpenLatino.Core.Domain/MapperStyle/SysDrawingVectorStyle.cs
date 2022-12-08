using System.Drawing;

namespace OpenLatino.Core.Domain.MapperStyle
{
    public class SysDrawingVectorStyle
    {
        public string Name { get; set; }

        public Brush FillStyle { get; set; }

        public Pen LineStyle { get; set; }

        public bool EnableOutLine { get; set; }

        public Pen OutLineStyle { get; set; }

        public Brush PointBrush { get; set; }

        public Image Image { get; set; }

        public float PointSize { get; set; }

        public float ImageRotacion { get; set; }

        public float ImageScale { get; set; }
    }
}

