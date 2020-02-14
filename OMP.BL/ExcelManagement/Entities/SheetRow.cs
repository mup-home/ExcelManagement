using System.Collections.Generic;

namespace OMP.BL.ExcelManagement.Entities
{
    public class SheetRow
    {
        public List<object> Data { get; set; }

        public SheetRow()
        {
            Data = new List<object>();
        }
    }
}