using System.Collections.Generic;
using System.Threading.Tasks;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Filters;
using OpenLatino.MapServer.Domain.Entities.Querys;

namespace OpenLatino.MapServer.Domain.Entities.Providers
{
    public interface IProviderService
    {
        int ID { get; set; }

        string ConnectionString { get; set; }

        string PkField { get; set; }

        string Table { get; set; }

        string GeoField { get; set; }

        string BoundingBoxField { get; set; }

        IEnumerable<Layer> Layers { get; set; }

        //como funciona lo de los filtros Darian
        IEnumerable<IFilter> Filter { get; set; }

        IEnumerable<object> Execute(IQuery query);
        Task<IEnumerable<object>> ExecuteAsync(IQuery query);
        IEnumerable<object> ExecuteYield(IQuery query);
    }
}
