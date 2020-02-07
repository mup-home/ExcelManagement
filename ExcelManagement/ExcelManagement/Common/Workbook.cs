using System.Collections.Generic;

namespace OMP.BL.ExcelManagement.Common
{
    public class Workbook
    {
        public string Name { get; set; }
        public List<Sheet> Sheets { get; set; }
    }
}
