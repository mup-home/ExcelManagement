using System.Collections.Generic;

namespace OMP.BL.ExcelManagement.Entities
{
    public class Workbook
    {
        public string Name { get; set; }

        public List<string> Sheets { get; set; }
    }
}
