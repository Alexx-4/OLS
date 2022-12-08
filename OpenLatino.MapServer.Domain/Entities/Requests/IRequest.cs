using OpenLatino.Core.Domain.Entities;
using System.Collections.Generic;

namespace OpenLatino.MapServer.Domain.Entities.Requests
{
    public interface IRequest
    {
        string this[string nameParam] { get; }

        bool HasParameter(string nameParam);

        object Body { get; set; }
    }
}
