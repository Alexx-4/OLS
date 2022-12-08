using System;
using System.Linq.Expressions;

namespace OpenLatino.MapServer.Domain.Map.Filters.Helpers
{
    public class FilterBuilder
    {
        public static Func<T, bool> GetFilterFunction<T>(IFilter filter)
        {
            var param = Expression.Parameter(typeof(T), "t");

            var expression = filter.GetExpression(param);

            return Expression.Lambda<Func<T, bool>>(expression, param).Compile();
        }
               
    }
}