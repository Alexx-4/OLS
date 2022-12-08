namespace OpenLatino.MapServer.Domain.Entities.Querys.WMS
{
    public interface IWMSCapabilitiesQuery : IWMSQuery
    {
        string Service { get; set; }

        string Version { get; set; }

    }
}
