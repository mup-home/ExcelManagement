using System;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Enums;
using OMP.BL.ExcelManagement.Helpers;

namespace OMP.BL.ExcelManagement.Exceptions
{
    public class ExcelValidationException: Exception
    {
        public ExcelValidationException(ExcelValidationError errorType,string sheetName, string propertyName): base(MessageProvider.GetErrorMessage(errorType, sheetName, propertyName))
        { }
    }
}