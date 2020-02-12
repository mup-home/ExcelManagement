using System;
using System.Linq;
using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Enums;
using OMP.BL.ExcelManagement.Exceptions;
using OMP.BL.ExcelManagement.Helpers;

namespace OMP.BL.ExcelManagement.Validation
{
    public class ObjectPropertyValidator : PropertyValidator
    {
        private string _sheetName;
        private int _rowNumber;

        public ObjectPropertyValidator(string sheetName): base(MessageProvider.GetPropertyValidationErrorMessage())
        {
            _sheetName = sheetName;
            _rowNumber = 1;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var data = context.Instance as object;
            var propertyName = context.PropertyValue.GetType().GetProperty("Name").GetValue(context.PropertyValue).ToString();
            var propertyValue = data.GetType().GetProperty(propertyName).GetValue(data);

            bool isValidValue = true;
            var sheetColumns = ExcelManagementHelper.BookConfig.SheetColumns[_sheetName];
            var column = sheetColumns.Values.FirstOrDefault(v => v.Name == propertyName); // TryGetValue(propertyName, out Column column);            
            foreach (var rule  in column?.ValidationRules)
            {
                if (rule.StartsWith("mandatory", StringComparison.InvariantCultureIgnoreCase))
                {
                    isValidValue = propertyValue != null ? !string.IsNullOrEmpty(propertyValue.ToString()) : false;
                    if (!isValidValue)
                    {
                        string errorMessage = MessageProvider.GetErrorMessage(ExcelValidationError.MandatoryFieldEmpty, _sheetName);
                        BuildContextMessage(context, column.ExcelColumnName, errorMessage);
                    }
                }
                if (rule.StartsWith("length", StringComparison.InvariantCultureIgnoreCase))
                {
                    isValidValue = HasValidLength(context, column, propertyValue, rule);
                }
                if (rule.Equals("primaryKey", StringComparison.InvariantCultureIgnoreCase)) 
                {

                }
            }

            return isValidValue;
        }

        protected private bool HasValidLength(PropertyValidatorContext context, Column column, object propertyValue, string validationRule)
        {
            //var LengthBoundaries = validationRule.Substring(validationRule.IndexOf('[') + 1, validationRule.Length);
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
                    BuildContextMessage(context, column.ExcelColumnName, errorMessage);
                    return false;
                }
            }
            return true;
        }

        private void BuildContextMessage(PropertyValidatorContext context, string columnName, string errorMessage)
        {
            context.MessageFormatter
                .AppendArgument("SheetName", _sheetName)
                .AppendArgument("RowNumber", _rowNumber)
                .AppendArgument("ColumnName", columnName)
                .AppendArgument("ValidationErrorMessage", errorMessage);
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