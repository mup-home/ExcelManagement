using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Validation;
using OMP.Shared;
using OMP.Shared.Extensions;

namespace OMP.BL.ExcelManagement.Helpers
{
    public static class ExcelManagementHelper
    {
        public static Workbook GetBookConfig(string bookName, List<string> errors)
        {
            return JsonConfigHelper.ReadJsonConfig<Workbook>(bookName, errors);
        }

        public static BookConfig LoadWorkbookConfig(string bookName, List<string> errors)
        {
            var bookConfig = new BookConfig();
             var configPath = $"{ConfigurationHelper.GetKeyValue("SheetConfigPath")}";
            var workbook = JsonConfigHelper.ReadJsonConfig<Workbook>(bookName, errors);
            if (errors.Count == 0) {
                foreach (var sheet in workbook.Sheets)
                {
                    var sheetConfig = JsonConfigHelper.ReadJsonConfig<Sheet>(sheet, errors);
                    if (!string.IsNullOrEmpty(sheetConfig.Name)) 
                    {
                        bookConfig.Sheets.Add(sheet, sheetConfig);
                        var sheetColumns = new Dictionary<string, Column>();
                        foreach (var column in sheetConfig.Columns)
                        {
                            var columnConfig = JsonConfigHelper.ReadJsonConfig<Column>(column, errors);
                            if (!string.IsNullOrEmpty(columnConfig.Name)) {
                                sheetColumns.Add(column, columnConfig);
                            }
                            else
                            {
                                errors.Add($"Sheet: {sheet} - Config file for column: {column}, not found.");
                            }
                        }
                        bookConfig.SheetColumns.Add(sheet, sheetColumns);
                    }
                    else
                    {
                        errors.Add($"Book: {bookName} - Config file for sheet: {sheet}, not found.");
                    }
                }
            }
            return bookConfig;
        }

        public static void ValidateBook(BookConfig bookConfig, List<string> errors)
        {
            foreach (var sheet in bookConfig.Sheets.Keys)
            {
                var sheetValidator = new SheetValidator();
                var result = sheetValidator.Validate(bookConfig.Sheets[sheet]);
                if (!result.IsValid)
                {
                    result.Errors.OrderBy(e => e.Severity).ToList().ForEach(e => errors.Add(e.ErrorMessage));
                }
            }
        }

        public static ExcelPackage LoadBookData(string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                byte[] bin = File.ReadAllBytes(fileFullPath);
                using (MemoryStream stream = new MemoryStream(bin))
                return  new ExcelPackage(stream); 
            }
            throw new FileNotFoundException($"File: {fileFullPath}, not found");           
        }

        public static void ProcessSheetData(ExcelPackage excelPackage, BookConfig bookConfig, List<string> errors = null)
        {
            var sheets = excelPackage.Workbook.Worksheets.AsEnumerable();
            foreach (var sheetName in bookConfig.Sheets.Keys)
            {
                var sheet = bookConfig.Sheets[sheetName];
                var worksheet = sheets.FirstOrDefault(s => s.Name.Equals(sheet.Name, StringComparison.InvariantCulture));
                var columns = bookConfig.SheetColumns[sheetName];
                for (int i = sheet.DataStartingRow + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    var newRow = new SheetRow() { RowNumber = i };
                    foreach (var column in columns.Keys)
                    {
                        var columnConfig = columns[column];
                        var newColumnData = columnConfig.DeepClone();
                        newColumnData.Value = worksheet.Cells[i, newColumnData.Number].Value;
                        newRow.Data.Add(newColumnData);
                    }
                    bookConfig.Sheets[sheetName].Data.Add(newRow);                    
                }               
            }            
        }
    }
}