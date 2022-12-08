using System;
using OpenLatino.MapServer.Domain.Map.Filters.Enums;

namespace OpenLatino.MapServer.Domain.Map.Filters
{
    [Serializable]
    public class FilterClause
    {
        public InfoSource Source { get; set; }

        public string Name { get; set; }

        public ComparisonOperator Operator { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            return $"{Source} {Name} {Operator} {Value}";
        }
    }

}