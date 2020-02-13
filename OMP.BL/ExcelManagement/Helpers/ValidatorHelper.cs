using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Extensions;

namespace OMP.BL.ExcelManagement.Helpers
{
    public static class ValidatorHelper
    {
        public static void ValidateSheetRows(Sheet sheet, List<string> errors)
        {
        }

        public static void ValidateDuplicatedSheetRows(List<string> errors)
        {
            var sheets = ExcelManagementHelper.BookConfig.Sheets;
            foreach (var sheet in sheets.Keys)
            {
                var primariesKey = GetSheetPrimariesKey(sheet);
                var sheetData = sheets[sheet].Data;

                /* var item = sheetData.First();
                var param = Expression.Parameter(item.GetType(),"o");
                var exp1 = Expression.Property(param, item.GetType().GetProperty("identifier"));
                var exp2 = Expression.Property(param, item.GetType().GetProperty("revision")); */

                // MemberExpression member = Expression.Property(param, "identifier");
                //var uniqueKey = Expression.Lambda<Func<object, bool>>(member, param); 
                //var uniqueKey = Expression.Lambda<Func<object, bool>>(member, param); 
                var duplicates  = sheetData.GetDuplicates(primariesKey);
                string rowsWithDuplicateUniqueKeys = sheetData.GetDuplicateUniqueKeyRows(primariesKey, duplicates.Keys.ToList());
                foreach (var key in duplicates.Keys)
                {
                    var keys = key.Split(',');
                    errors.Add($"Sheet: {sheet}, rows: {keys[0]}, {rowsWithDuplicateUniqueKeys};  has same unique key values for columns: {primariesKey.ToString()}");
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
                if(columns[columnName].IsPrimaryKey) {
                    sheetPrimariesKey.Add(columns[columnName].Name);
                }
            }
            return sheetPrimariesKey;
        }
    }
}