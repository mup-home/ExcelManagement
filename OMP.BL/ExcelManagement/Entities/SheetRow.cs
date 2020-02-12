using System.Collections.Generic;

namespace OMP.BL.ExcelManagement.Entities
{
    public class SheetRow
    {
        public int RowNumber { get; set; }
        public List<object> Data { get; set; }

        public SheetRow()
        {
            Data = new List<object>();
        }
    }
}