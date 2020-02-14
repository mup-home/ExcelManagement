using System;
using System.Collections.Generic;

namespace OMP.BL.ExcelManagement.Entities
{
    public class SheetColumn
    {
        public string Name { get; set; }
        
        public string DbEntityProperty { get; set; }

        public string ExcelColumnName { get; set; }

        public int ColumnNumber { get; set; }

        public string ColumnType { get; set; }

        public bool IsUniqueKey { get; set; }

        public List<string> ValidationRules { get; set; }
    }
}
