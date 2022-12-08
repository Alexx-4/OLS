using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OpenLatino.MapServer.Domain.Map.Filters.Helpers
{
    public static class FilterSerializer
    {
        // Convert an object to a byte array
        public static byte[] FilterExpressionToByteArray(IFilter filter)
        {
            if (filter == null)
                return null;

            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, filter);

            return ms.ToArray();
        }

        // Convert a byte array to an IFilterExpression
        public static IFilter ByteArrayToFilterExpression(byte[] bytes)
        {
            var memoryStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            if(bytes != null)
            {
                memoryStream.Write(bytes, 0, bytes.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
            }
            
            var filter = (IFilter)binForm.Deserialize(memoryStream);
            return filter;
        }
    }
}