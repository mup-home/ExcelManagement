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
        public static BookConfig BookConfig { get; }

        static ExcelManagementHelper()
        {
            BookConfig = new BookConfig();
            // BookConfig = LoadWorkBookConfig("technical_orders", errors);
        }

        public static Workbook GetBookConfig(string bookName, List<string> errors)
        {
            return JsonConfigHelper.ReadJsonConfig<Workbook>(bookName, errors);
        }

        public static void LoadWorkBookConfig(string bookName, List<string> errors)
        {
            var configPath = $"{ConfigurationHelper.GetKeyValue("SheetConfigPath")}";
            var workbook = JsonConfigHelper.ReadJsonConfig<Workbook>(bookName, errors);
            if (errors.Count == 0) {
                foreach (var sheet in workbook.Sheets)
                {
                    var sheetConfig = JsonConfigHelper.ReadJsonConfig<Sheet>(sheet, errors);
                    if (!string.IsNullOrEmpty(sheetConfig.Name)) 
                    {
                        BookConfig.Sheets.Add(sheet, sheetConfig);
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
                        BookConfig.SheetColumns.Add(sheet, sheetColumns);
                    }
                    else
                    {
                        errors.Add($"Book: {bookName} - Config file for sheet: {sheet}, not found.");
                    }
                }
            }
        }

        public static void ValidateBook(List<string> errors)
        {
            var sheetValidator = new SheetValidator();
            foreach (var sheet in BookConfig.Sheets.Keys)
            {
                var result = sheetValidator.Validate(BookConfig.Sheets[sheet]);
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

        public static void ProcessSheetData(ExcelPackage excelPackage, List<string> errors = null)
        {
            var sheets = excelPackage.Workbook.Worksheets.AsEnumerable();
            foreach (var sheetName in BookConfig.Sheets.Keys)
            {
                var sheet = BookConfig.Sheets[sheetName];
                var worksheet = sheets.FirstOrDefault(s => s.Name.Equals(sheet.SheetName, StringComparison.InvariantCulture));
                var columns = BookConfig.SheetColumns[sheetName];
                for (int i = sheet.DataStartingRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    var sheetRow = new Dictionary<string, object>();
                    foreach (var column in columns.Keys)
                    {
                        var columnConfig = columns[column];
                        object value = column == "sheet_row" ? new object() : worksheet.Cells[i, columnConfig.Number].Value;
                        if (column != "sheet_row")
                            sheetRow.Add(columnConfig.Name, value);
                    }
                    object sheetRowObject = sheetRow.ToAnonymousObject();
                    BookConfig.Sheets[sheetName].Data.Add(sheetRowObject);                    
                }               
            }            
        }
    }
}