using System.Collections.Generic;

namespace OMP.BL.ExcelManagement.Common
{
    public class Sheet
    {
        public string Name { get; set; }

        public int TotalColumns { get; set; }

        public bool ValidateFormat { get; set; }

        public List<SheetColumn> Rows { get; set; }
    }
}
