namespace OpenLatino.MapServer.Domain.Entities.Filters
{
    public class DumbFilter:IFilter
    {
        public object Item { get; set; }
        public bool CanGet(object item)
        {
            Item = item;
            return true;
        }

        public object Get()
        {
            return Item;
        }
    }
}
