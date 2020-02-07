using OfficeOpenXml;
using OMP.BL.ExcelManagement.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            var package = new ExcelPackage(new FileInfo(@"D:\Docs\OMP\Excels\OPM_Excel.xlsx"));
            var excelWorksheets = DataLoadingHelper.GetExcelWorksheets(package);
            //foreach (var cell in package.Workbook.Worksheets["TechnicalOrders"].Cells)
            //{
            //    DataLoadingHelper.
            //} 
            Console.WriteLine($"Workbook has {excelWorksheets.Count} sheets!!");
            Console.ReadKey();
        }
    }
}
