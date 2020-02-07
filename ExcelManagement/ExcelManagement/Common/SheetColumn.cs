using System;
using System.Collections.Generic;

namespace OMP.BL.ExcelManagement.Common
{
    public class SheetColumn
    {
        public string Name { get; set; }

        public string ExcelColumnName { get; set; }

        public int Number { get; set; }

        public Type ColumnType { get; set; }

        public bool IsPrimaryKey { get; set; }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public bool Mandatory { get; set; }

        public object Value { get; set; }

        public List<string> ValidationRules { get; set; }
    }
}
