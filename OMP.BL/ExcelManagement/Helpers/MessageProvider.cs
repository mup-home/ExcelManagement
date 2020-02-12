using System;
using FluentValidation.Resources;
using OMP.BL.ExcelManagement.Enums;

namespace OMP.BL.ExcelManagement.Helpers
{
    public static class MessageProvider
    {
        internal static string GetErrorMessage(ExcelValidationError errorType, string sheetName, string propertyName = "")
        {
            string message = string.Empty;
            switch (errorType)
            {
                case ExcelValidationError.InvalidLengthRuleFormat:
                    message = $"{GetBaseRuleValidationMessage(sheetName, propertyName)} - Invalid length rule format.";
                    break;
                
                case ExcelValidationError.SheetWithoutData:
                    message = $"Sheet: {sheetName} - has not data.";
                    break;
                
                case ExcelValidationError.MandatoryFieldEmpty:
                    message = $"is mandatory.";
                    break;

                default:
                break;
            }
            return message;
        }

        internal static IStringSource GetDuplicateRowMessage()
        {
            throw new NotImplementedException();
        }

        internal static string GetPropertyValidationErrorMessage()
        {
            return GetBasePropertyValidatorError() + " - {ValidationErrorMessage}";
        }

        internal static string GetLengthRuleError(string value, int minLength, int maxLength)
        {
            return $"Value: {value}, length must be between {minLength} and {maxLength} characters.";
        }

        internal static string GetBasePropertyValidatorError() 
        {
            return "Sheet: {SheetName} [Row: {RowNumber}, Column: {ColumnName}]";
        }
        private static string GetBaseRuleValidationMessage(string sheetName, string propertyName)
        {
            return $"Sheet: {sheetName}, Property: {propertyName}";
        }
    }
}