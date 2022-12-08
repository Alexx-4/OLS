using System.IO;

//using Newtonsoft.Json;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    public interface IResponse
    {
        object GetResponseContent();

        bool HasImage();

        Stream GetImage();
        string contentType { get; }
    }
}
