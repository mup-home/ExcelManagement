using System.Collections.Generic;

namespace OMP.BL.ExcelManagement.Entities
{
    public class BookConfig
    {
        public Dictionary<string, Sheet> Sheets { get; set; }
        public Dictionary<string, Dictionary<string, SheetColumn>> SheetColumns { get; set; }

        public BookConfig()
        {
            Sheets = new Dictionary<string, Sheet>();
            SheetColumns = new  Dictionary<string, Dictionary<string, SheetColumn>>();
        }
    }
}