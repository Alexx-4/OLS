using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Render
{
    public class MapImage : IMapImage, IDisposable
    {
        private Stream surface;

        private ImageFormat format = ImageFormat.Png;

        public MapImage(Stream surface) : this(surface, null)
        {

        }

        public MapImage(Stream surface, string formatImage)
        {
            surface.Seek(0, SeekOrigin.Begin);
            this.surface = new MemoryStream(Convert.ToInt32(surface.Length));
            surface.CopyTo(this.surface);
            this.surface.Seek(0, SeekOrigin.Begin);

            if (!string.IsNullOrWhiteSpace(formatImage))
                if (formatImage.Contains("png")) format = ImageFormat.Png;
                else if (formatImage.Contains("jpeg")) format = ImageFormat.Jpeg;
        }

        public void OverlapImage(IMapImage mapImage)
        {
            var externalBitmap = new Bitmap(mapImage.GetImage());
            var internalBitmap = new Bitmap(GetImage());
            var result = new Bitmap(internalBitmap.Width, internalBitmap.Height);
            var g = Graphics.FromImage(result);
            g.DrawImage(internalBitmap, 0, 0);
            g.DrawImage(externalBitmap, 0, 0);

            var newSurface = new MemoryStream();
            result.Save(newSurface, format);
            newSurface.Seek(0, SeekOrigin.Begin);
            var tmp = surface;
            surface = newSurface;

            Task.Factory.StartNew(() =>
            {
                g.Dispose();
                internalBitmap.Dispose();
                externalBitmap.Dispose();
                result.Dispose();
                tmp.Dispose();
            });
        }

        public Stream GetImage()
        {
            return surface;
        }

        public void Dispose()
        {
            surface.Dispose();
        }
    }
}
