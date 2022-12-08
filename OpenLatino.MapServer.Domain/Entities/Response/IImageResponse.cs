using OpenLatino.MapServer.Domain.Map.Render;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    public interface IImageResponse : IResponse
    {
        IMapImage Tile { get; set; }
    }
}
