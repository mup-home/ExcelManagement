using System;
using System.Linq;
using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Enums;
using OMP.BL.ExcelManagement.Exceptions;
using OMP.BL.ExcelManagement.Helpers;

namespace OMP.BL.ExcelManagement.Validation
{
    public class ObjectPropertyLengthValidator : PropertyValidator
    {
        private static string _sheetName = string.Empty;
        private static int _rowNumber;
        private static object rowObject = new object();

        public ObjectPropertyLengthValidator(string sheetName): base(MessageProvider.GetPropertyValidationErrorMessage())
        {
            if (!_sheetName.Equals(sheetName))
            {
                _sheetName = sheetName;
                _rowNumber = ExcelManagementHelper.BookConfig.Sheets[sheetName].DataStartingRow - 1;
            };
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var data = context.Instance as object;
            if (!rowObject.Equals(data)) {
                _rowNumber += 1;
                rowObject = data;
            }
            var propertyName = ObjectPropertyHelper.GetPropertyValue("Name", context.PropertyValue);            
            var propertyValue =  ObjectPropertyHelper.GetPropertyValueAsObject(propertyName, data);

            bool isValidValue = true;
            var sheetColumns = ExcelManagementHelper.BookConfig.SheetColumns[_sheetName];
            var column = sheetColumns.Values.FirstOrDefault(v => v.Name == propertyName);
            bool hasMandatoryRule = column?.ValidationRules.Any(r => r.StartsWith("mandatory", StringComparison.InvariantCultureIgnoreCase)) ?? false;
            bool hasLengthRule = column?.ValidationRules.Any(r => r.StartsWith("length", StringComparison.InvariantCultureIgnoreCase)) ?? false;
            if ((hasLengthRule && propertyValue != null)
                || (hasLengthRule && !hasMandatoryRule && propertyValue != null))
            {
                string rule = column.ValidationRules.Find(r => r.StartsWith("length", StringComparison.InvariantCultureIgnoreCase));
                isValidValue = HasValidLength(context, column, propertyValue, rule);
            }

            return isValidValue;
        }

        protected private bool HasValidLength(PropertyValidatorContext context, Column column, object propertyValue, string validationRule)
        {
            var LengthBoundaries = validationRule.Substring(validationRule.IndexOf('[')).Replace("[","").Replace("]","");
            
            if (column.ColumnType.Equals("string", StringComparison.InvariantCultureIgnoreCase)
                || column.ColumnType.Equals("number", StringComparison.InvariantCultureIgnoreCase))
            {
                string value = propertyValue?.ToString() ?? "{empty}";
                int minLength = GetArrayBoundary(LengthBoundaries, LengthBoundary.Minimum, _sheetName, column.Name);
                int maxLength = GetArrayBoundary(LengthBoundaries, LengthBoundary.Maximun, _sheetName, column.Name);
                if (value.Length < minLength || value.Length > maxLength) 
                {
                    string errorMessage = MessageProvider.GetLengthRuleError(value, minLength, maxLength);
                    ValidatorHelper.BuildContextMessage(context, _sheetName, _rowNumber, column.ExcelColumnName, errorMessage);
                    return false;
                }
            }
            return true;
        }

        protected private int GetArrayBoundary(string  LengthBoundaries, LengthBoundary boundary, string sheetName, string propertyName) {
            var boundaries = LengthBoundaries.Split(',');
            if (boundaries.Length > 2) 
            {
                throw new ExcelValidationException(ExcelValidationError.InvalidLengthRuleFormat, sheetName, propertyName);
            }
            try
            {
                if (boundary == LengthBoundary.Minimum) 
                {
                    return boundaries.Length == 2 ? int.Parse(boundaries[0]) : 0;
                }
                else
                {
                    return boundaries.Length == 2 ? int.Parse(boundaries[1]) : int.Parse(boundaries[0]);
                }
            }
            catch (System.Exception)
            {                
                throw new ExcelValidationException(ExcelValidationError.InvalidLengthRuleFormat, sheetName, propertyName);
            }            
        }
    }
}