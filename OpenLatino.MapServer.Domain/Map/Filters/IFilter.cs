using System.Linq.Expressions;

namespace OpenLatino.MapServer.Domain.Map.Filters
{    
    public interface IFilter
    {
        Expression GetExpression(ParameterExpression parameter);
    }
}