using System;
using System.Collections.Generic;
using OMP.BL.ExcelManagement.Helpers;
using OMP.Shared;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationHelper.Init("CASA.ApplicationSettings.json");
            var errors = new List<string>();
            var bookConfig = ExcelManagementHelper.LoadWorkbookConfig("technical_orders", errors);
            if (errors.Count != 0)
            {
                Console.WriteLine("-- Errors --");
                errors.ForEach(e => Console.WriteLine(e));
            }
            else
            {
                Console.WriteLine("Book configuration loaded successfully!!");
                var bookData = ExcelManagementHelper.LoadBookData(@"D:\Proyectos\ExcelManagement\data\import\technical_orders.xlsx");
                ExcelManagementHelper.ProcessSheetData(bookData, bookConfig);
                ExcelManagementHelper.ValidateBook(bookConfig, errors);
                if (errors.Count > 0)
                {
                    // Console.Clear();
                    Console.WriteLine("-- Validation Errors --");
                    errors.ForEach(e => Console.WriteLine(e));
                }
            }
        }
    }
}
