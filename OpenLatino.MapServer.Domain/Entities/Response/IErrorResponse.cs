namespace OpenLatino.MapServer.Domain.Entities.Response
{
    public interface IErrorResponse : IResponse
    {
        string Message { get; }
    }
}