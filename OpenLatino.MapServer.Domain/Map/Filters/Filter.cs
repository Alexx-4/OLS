using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace OpenLatino.MapServer.Domain.Map.Filters
{
    [Serializable]
    public abstract class Filter : IFilter
    {
        public abstract Expression GetExpression(ParameterExpression parameter);
        
    }
}