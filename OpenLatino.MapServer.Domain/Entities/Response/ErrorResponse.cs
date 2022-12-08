using System.IO;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    public class ErrorResponse : IErrorResponse
    {
        public ErrorResponse(string message)
        {
            Message = message;
        }
        public string Message { get; }

        public string contentType => "text/json";

        public Stream GetImage()
        {
            throw new System.NotImplementedException();
        }

        public object GetResponseContent()
        {
            throw new System.NotImplementedException();
        }

        public bool HasImage()
        {
            throw new System.NotImplementedException();
        }
    }
}