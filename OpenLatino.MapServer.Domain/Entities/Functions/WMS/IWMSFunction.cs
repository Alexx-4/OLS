namespace OpenLatino.MapServer.Domain.Entities.Functions.WMS
{
    public interface IWMSFunction : IFunction
    {        
        string[] responseFormat { get; }
    }
}
