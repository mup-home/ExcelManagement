using System.Collections.Generic;

namespace OMP.BL.ExcelManagement.Entities
{
    public class SheetRow
    {
        public int RowNumber { get; set; }
        public List<Column> Data { get; set; }

        public SheetRow()
        {
            Data = new List<Column>();
        }
    }
}