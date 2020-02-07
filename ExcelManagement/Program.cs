using OfficeOpenXml;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Enums;
using OMP.BL.ExcelManagement.Helpers;
using OMP.Shared;
using System;
using System.Collections.Generic;

namespace ExcelManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationHelper.Init("CASA.ApplicationSettings.json");
            var entityType = GenericHelper.FindTypeByAssembly("Sheet", "ExcelManagement");
  
            //michel();
            Console.ReadKey();
            //var package = new ExcelPackage(new FileInfo(@"D:\Docs\OMP\Excels\technical_orders.xlsx"));
            //var excelWorksheets = DataLoadingHelper.GetExcelWorksheets(package);
            //foreach (var cell in package.Workbook.Worksheets["TechnicalOrders"].Cells)
            //{
            //    DataLoadingHelper.
            //} 
            //Console.WriteLine($"Workbook has {excelWorksheets.Count} sheets!!");
        }

        public static object GetWorkbook(string bookName, List<string> errors)
        {
            object workbook = null;
            var bookConfig = JsonConfigHelper.ReadJsonConfig<Workbook>(bookName, errors);
            if (errors.Count == 0)
            {
                foreach (var sheetName in bookConfig.Sheets)
                {
                    var sheetConfig = JsonConfigHelper.ReadJsonConfig<Sheet>(sheetName, errors);
                    if (!string.IsNullOrEmpty(sheetConfig.Name))
                    {
                        foreach (var columnName in sheetConfig.Columns)
                        {
                            var columnConfig = JsonConfigHelper.ReadJsonConfig<Column>(columnName, errors);
                        }
                    }
                }
            }

            PrintErrors(errors);

            return workbook ?? null;
        }

        private static void PrintErrors(List<string> errors)
        {
            if (errors.Count > 0)
            {
                Console.WriteLine($"-- Errors --");
                errors.ForEach(e => Console.WriteLine(e));
            }
        }
    }
}
