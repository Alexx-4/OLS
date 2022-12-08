using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLatino.Core.Domain.Models
{
    public class TematicViewModel
    {
        public int? tematicId { get; set; }
        public string tematicName { get; set; }
        public List<query> queries { get; set; }


        public class Condition
        {
            public string columnName { get; set; }
            public string _operator { get; set; }
            public string value { get; set; }
            public string logicOperator { get; set; }
        }

        public class query
        {
            public string styleName { get; set; }
            public List<Condition> conditions { get; set; }
            public string layerName { get; set; }
            public string tableName { get; set; }
        }
    }


}
