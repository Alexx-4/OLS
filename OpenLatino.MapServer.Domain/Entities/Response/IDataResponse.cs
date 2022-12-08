using System.Collections.Generic;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    public interface IDataResponse<T> : IResponse
    {
        IList<T> Data { get; set; }
    }
}
