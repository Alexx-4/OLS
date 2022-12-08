using System;
using System.Linq.Expressions;
using OpenLatino.MapServer.Domain.Map.Filters.Enums;

namespace OpenLatino.MapServer.Domain.Map.Filters
{
    [Serializable]
    public class BinaryFilter : Filter
    {
        private readonly Filter _left;
        private readonly GeometryFilter _right;
        private readonly LogicalOperator _op;

        public BinaryFilter(Filter left, GeometryFilter right, LogicalOperator op)
        {
            _left = left;
            _right = right;
            _op = op;
        }

        public override Expression GetExpression(ParameterExpression parameter)
        {
            var left = _left.GetExpression(parameter);
            var right = _right.GetExpression(parameter);

            Expression expression;
            switch (_op)
            {
                case LogicalOperator.And:

                    expression = Expression.AndAlso(left, right);
                    return expression;

                case LogicalOperator.Or:

                    expression = Expression.OrElse(left, right);
                    return expression;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
       

        public override string ToString()
        {
            return $"{_left} {_right} {_op} ";
        }
    }
}