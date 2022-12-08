using Dapper;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Filters;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Infrastructure.SQL.DataSource
{
    public class DBContextProvider : ISQLProviderService
    {
        public int ID { get => -1; set => throw new NotImplementedException(); }
        public string ConnectionString { get; set; }
        public string PkField { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Table { get; set; } //Realmente vamos a poden la base de datos
        public string GeoField { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string BoundingBoxField { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEnumerable<Layer> Layers { get => null; set => throw new NotImplementedException(); }
        public IEnumerable<IFilter> Filter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerable<object> Execute(IQuery query)
        {
            string q = query.GetQuery().ToString();
            var result = new List<Dictionary<string, object>>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                var res = conn.Query(q);
                foreach (var item in res)
                {
                    result.Add(new Dictionary<string, object>((IEnumerable<KeyValuePair<string, object>>)item));
                }
            }
            return result;
        }
        public IEnumerable<object> ExecuteYield(IQuery query)
        {
            string q = query.GetQuery().ToString();
            var result = new List<Dictionary<string, object>>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                var res = conn.Query(q);
                foreach (var item in res)
                {
                    yield return new Dictionary<string, object>((IEnumerable<KeyValuePair<string, object>>)item);
                }
            }
        }

        public async Task<IEnumerable<object>> ExecuteAsync(IQuery query)
        {
            string q = query.GetQuery().ToString();
            var result = new List<Dictionary<string, object>>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                var res = await conn.QueryAsync(q);
                foreach (var item in res)
                {
                    result.Add(new Dictionary<string, object>((IEnumerable<KeyValuePair<string, object>>)item));
                }
            }
            return result;
        }

    }
}
