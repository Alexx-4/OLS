using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Render
{
    public interface IMapImage
    {
        void OverlapImage(IMapImage mapImage);

        Stream GetImage();
    }
}
