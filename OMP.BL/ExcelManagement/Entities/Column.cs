using System;
using System.Collections.Generic;

namespace OMP.BL.ExcelManagement.Entities
{
    public class Column
    {
        public string Name { get; set; }
        
        public string DtoProperty { get; set; }

        public string DbProperty { get; set; }

        public string ExcelColumnName { get; set; }

        public int Number { get; set; }

        public string ColumnType { get; set; }

        public bool IsPrimaryKey { get; set; }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public bool Mandatory { get; set; }

        public object Value { get; set; }

        public List<string> ValidationRules { get; set; }
    }
}
