namespace OpenLatino.MapServer.Domain.Entities.Filters
{
    public interface IFilter
    {
        bool CanGet(object item);

        object Get();
    }
}
