using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using OpenLatino.MapServer.Domain.Map.Filters.Enums;

namespace OpenLatino.MapServer.Domain.Map.Filters
{
    [Serializable]
    public class GeometryFilter : Filter
    {
        private readonly Filter _expression;
        private readonly Clause _clauseInfo;
        private readonly bool _boolean;

        //quitar los methodInfo como propiedades(Frankie)

        public GeometryFilter()
        {
            _boolean = true;
        }

        public GeometryFilter(Clause clauseInfo)
        {
            _clauseInfo = clauseInfo;
        }

        public GeometryFilter(Filter expression)
        {
            _expression = expression;
        }

        public override Expression GetExpression(ParameterExpression parameter)
        {
            if (_clauseInfo is null)
                return _expression.GetExpression(parameter);

            Expression left = Expression.Property(parameter, "Attributes");
            left = Expression.Property(left, "Item", Expression.Constant(_clauseInfo.Name));

            var value = Expression.Constant(_clauseInfo.Value);

            var notNull = Expression.NotEqual(left, Expression.Constant(null));
            Expression condition;

            switch (_clauseInfo.Operator)
            {
                case ComparisonOperator.Equal:
                    left = Expression.ConvertChecked(left, typeof(decimal));
                    condition = Expression.Equal(left, value);
                    break;

                case ComparisonOperator.GreaterThan:
                    left = Expression.ConvertChecked(left, typeof(decimal));
                    condition = Expression.GreaterThan(left, value);
                    break;

                case ComparisonOperator.GreaterThanOrEqual:
                    left = Expression.ConvertChecked(left, typeof(decimal));
                    condition = Expression.GreaterThanOrEqual(left, value);
                    break;

                case ComparisonOperator.LessThan:
                    left = Expression.ConvertChecked(left, typeof(decimal));
                    condition = Expression.LessThan(left, value);
                    break;

                case ComparisonOperator.LessThanOrEqual:
                    left = Expression.ConvertChecked(left, typeof(decimal));
                    condition = Expression.LessThanOrEqual(left, value);
                    break;

                case ComparisonOperator.NotEqual:
                    left = Expression.ConvertChecked(left, typeof(decimal));
                    condition = Expression.NotEqual(left, value);
                    break;

                case ComparisonOperator.Contains:
                    left = Expression.ConvertChecked(left, typeof(string));
                    MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                    condition = Expression.Call(left, containsMethod, value);
                    break;

                case ComparisonOperator.StartsWith:
                    MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                    left = Expression.ConvertChecked(left, typeof(string));
                    condition = Expression.Call(left, startsWithMethod, value);
                    break;

                case ComparisonOperator.EndsWith:
                    MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    left = Expression.ConvertChecked(left, typeof(string));
                    condition = Expression.Call(left, endsWithMethod, value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Expression.AndAlso(notNull, condition);
        }

        public override string ToString()
        {
            return _clauseInfo is null ?
                $"{_expression}" :
                $"{_clauseInfo.Source} {_clauseInfo.Name} {_clauseInfo.Operator} {_clauseInfo.Value}";
        }
    }
}