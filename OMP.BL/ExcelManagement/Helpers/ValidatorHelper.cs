using System.Collections.Generic;
using System.Linq;
using OMP.BL.ExcelManagement.Extensions;

namespace OMP.BL.ExcelManagement.Helpers
{
    public static class ValidatorHelper
    {
        public static void ValidateDuplicatedSheetRows(List<string> errors)
        {
            var sheets = ExcelManagementHelper.BookConfig.Sheets;
            foreach (var sheet in sheets.Keys)
            {
                var primariesKey = GetSheetPrimariesKey(sheet);
                var sheetData = sheets[sheet].Data;
                var duplicates  = sheetData.GetDuplicates(sheets[sheet].DataStartingRow, primariesKey, out var listAsKeyValues);
                string rowsWithDuplicateUniqueKeys = listAsKeyValues.GetDuplicateUniqueKeyRows(primariesKey, duplicates.Keys.ToList());
                foreach (var key in duplicates.Keys)
                {
                    errors.Add(MessageProvider.GetDuplicateRowMessage(sheets[sheet].SheetName, rowsWithDuplicateUniqueKeys, primariesKey.ToDelimitedString(", ")));
                } 
            }
        }

        private static List<string> GetSheetPrimariesKey(string sheet)
        {
            var sheetPrimariesKey = new List<string>();
            var sheetColumns = ExcelManagementHelper.BookConfig.SheetColumns;            
            sheetColumns.TryGetValue(sheet, out var columns);
            foreach (var columnName in columns.Keys)
            {
                if(columns[columnName].IsUniqueKey) {
                    sheetPrimariesKey.Add(columns[columnName].Name);
                }
            }
            return sheetPrimariesKey;
        }
    }
}