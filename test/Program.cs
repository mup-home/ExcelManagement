using System.Collections.Generic;
using OMP.BL.ExcelManagement.Helpers;
using OMP.Shared;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationHelper.Init("CASA.ApplicationSettings.json");
            var errors = new List<string>();
            ExcelManagementHelper.LoadWorkBookConfig("technical_orders", errors);
            if (errors.Count != 0)
            {
                Console.WriteLine("-- Errors --");
                errors.ForEach(e => Console.WriteLine(e));
            }
            else
            {
                Console.WriteLine("Book configuration loaded successfully!!");
                // var bookData = ExcelManagementHelper.LoadBookData(@"D:\Proyectos\ExcelManagement\data\import\technical_orders.ok.xlsx");
                var bookData = ExcelManagementHelper.LoadBookData(@"D:\Proyectos\ExcelManagement\data\import\technical_orders.xlsx");
                ExcelManagementHelper.ProcessSheetData(bookData);                
                ExcelManagementHelper.ValidateBook(errors);
                if (errors.Count > 0)
                {
                    Console.WriteLine("-- Validation Errors --");
                    errors.ForEach(e => Console.WriteLine(e));
                }
                else
                {
                    ValidatorHelper.ValidateDuplicatedSheetRows(errors);
                    if (errors.Count > 0)
                    {
                        Console.WriteLine("-- Validation Errors --");
                        errors.ForEach(e => Console.WriteLine(e));
                    }
                    else
                    {
                        Console.WriteLine("Book ready to import!!");
                    }
                }
            }
        }
    }
}
