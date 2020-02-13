using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Entities;

namespace OMP.BL.ExcelManagement.Helpers
{
    public static class ValidatorHelper
    {
        public static void BuildContextMessage(PropertyValidatorContext context, string sheetName, int rowNumber, string columnName, string errorMessage)
        {
            context.MessageFormatter
                .AppendArgument("SheetName", sheetName)
                .AppendArgument("RowNumber", rowNumber)
                .AppendArgument("ColumnName", columnName)
                .AppendArgument("ValidationErrorMessage", errorMessage);
        }

        public static void ValidateSheetRows(Sheet sheet, List<string> errors)
        {

        }

        public static void ValidateDuplicatedSheetRows(List<string> errors)
        {
            var sheets = ExcelManagementHelper.BookConfig.Sheets;
            foreach (var sheet in sheets.Keys)
            {
                var sheetData = sheets[sheet].Data;
                var primariesKey = new List<string>();
                    var rows = new List<Dictionary<string, object>>();
                sheetData.ForEach(r =>
                {
                    var row = new Dictionary<string, object>();
                    /* row.Add("rowNumber", r.RowNumber);
                    r.Data.ForEach(c =>
                    {
                        if (c.IsPrimaryKey && !primariesKey.Any(i => i == c.Name))
                        {
                            primariesKey.Add(c.Name);
                        }
                        row.Add(c.Name, c.Value);
                    }); */
                    rows.Add(row);
                });


                rows.ForEach(r =>
                {
                    string rowKey = "";
                    foreach (var primaryKey in primariesKey)
                    {
                        rowKey += !string.IsNullOrEmpty(rowKey) ? "," : "";
                        rowKey += r[primaryKey].ToString();
                    }
                    r.Add("rowKey", rowKey);
                });

                var duplicates = rows
                    .SelectMany(d => d)
                    .Where(d => d.Key.Equals("rowKey"))
                    .GroupBy(d => d.Value)
                    .Where(x => x.Count() > 1)
                    .Select(v => v.Key.ToString())
                    .ToList();

                /* duplicates.ForEach(d => 
                {
                    var keys = d.Split(',');
                    Expression<Func<Column, bool>> predicate = Expression.Lambda<Func<Column, bool>>(c => c.);
                }); */
               
                /* primariesKey.ForEach(k =>
                {
                    var row = sheetData.Where(r => r.Data.Where(c => c.Name.Equals(k) && c.Value.ToString().Equals()));
                }); */
            }
        }
    }
}