using OfficeOpenXml;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Enums;
using OMP.BL.ExcelManagement.Helpers;
using OMP.Shared;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> errors = new List<string>();
            ConfigurationHelper.Init("CASA.ApplicationSettings.json");
            var bookConfig = JsonConfigHelper.ReadJsonConfig<Workbook>("technical_orders", errors);
            foreach (var sheetName in bookConfig.Sheets)
            {
                Console.WriteLine($"Reading {sheetName} configuration.");
                var sheetConfig = JsonConfigHelper.ReadJsonConfig<Sheet>(sheetName, errors);
                if (!string.IsNullOrEmpty(sheetConfig.Name))
                {
                    foreach (var columnName in sheetConfig.Columns)
                    {
                        var columnConfig = JsonConfigHelper.ReadJsonConfig<Column>(columnName, errors);
                    }                        
                }
            }
            if (errors.Count > 0)
            {
                Console.WriteLine($"-- Errors --");
                errors.ForEach(e => Console.WriteLine(e));
            }
            Console.ReadKey();
            //var package = new ExcelPackage(new FileInfo(@"D:\Docs\OMP\Excels\technical_orders.xlsx"));
            //var excelWorksheets = DataLoadingHelper.GetExcelWorksheets(package);
            //foreach (var cell in package.Workbook.Worksheets["TechnicalOrders"].Cells)
            //{
            //    DataLoadingHelper.
            //} 
            //Console.WriteLine($"Workbook has {excelWorksheets.Count} sheets!!");
        }
    }
}
