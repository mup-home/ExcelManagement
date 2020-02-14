using System;
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
                    throw new ArgumentException($"Error message for {errorType.ToString()} not defined yet");
            }
            return message;
        }

        internal static string GetDuplicateRowMessage(string sheetName, string rowsWithDuplicateUniqueKeys, string uniqueKeyColumns) => $"Sheet: {sheetName}, rows: [{rowsWithDuplicateUniqueKeys}]; has same values for unique key columns: [{uniqueKeyColumns}]";

        internal static string GetPropertyValidationErrorMessage() => GetBasePropertyValidatorError() + " - {ValidationErrorMessage}";

        internal static string GetLengthRuleError(string value, int minLength, int maxLength) => $"Value: {value}, length must be between {minLength} and {maxLength} characters.";

        internal static string GetBasePropertyValidatorError() => "Sheet: {SheetName} [Row: {RowNumber}, Column: {ColumnName}]";

        private static string GetBaseRuleValidationMessage(string sheetName, string propertyName) => $"Sheet: {sheetName}, Property: {propertyName}";

        internal static string GetInvalidJsonConfigFormat(string configType, string name, string message) => $"Invalid JSON format on file: {configType}_{name}.json: message: {message}";
        
        internal static string GetSheetConfigNotFound(string bookName, string sheetName) => $"Book: {bookName} - Config file for sheet: {sheetName}, not found.";

        internal static string GetJsonConfigFileNotFound(string configType, string name) => $"Configuration file: {configType}_{name}.json, not found";
    }
}