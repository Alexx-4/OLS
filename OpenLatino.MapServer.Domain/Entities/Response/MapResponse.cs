using System;
using System.IO;
using OpenLatino.MapServer.Domain.Map.Render;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    public class MapResponse : IResponse
    {
        public IMapImage Tile { get; set; }

        public string contentType => "image/png";

        public Stream GetImage()
        {
            return Tile.GetImage();
        }

        public object GetResponseContent()
        {
            throw new NotImplementedException();
        }

        public bool HasImage()
        {
           return true;
        }
    }
}
