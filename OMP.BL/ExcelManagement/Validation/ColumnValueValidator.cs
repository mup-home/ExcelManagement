using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Enums;
using OMP.BL.ExcelManagement.Exceptions;
using OMP.BL.ExcelManagement.Helpers;
using System;

namespace OMP.BL.ExcelManagement.Validation
{
    public class ColumnValueValidator: PropertyValidator
    {
        private string _sheetName;
        private int _rowNumber;

	    public ColumnValueValidator(string sheetName, int rowNumber): base(MessageProvider.GetPropertyValueMessage())
        { 
            _sheetName = sheetName;
            _rowNumber = rowNumber;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            bool isValidValue = true;
            var column = context.Instance as Column;
            foreach (var rule  in column.ValidationRules)
            {                
                if (rule.StartsWith("length", StringComparison.InvariantCultureIgnoreCase))
                {
                    isValidValue = HasValidLength(context, column, rule);
                }
                if (rule.Equals("primaryKey", StringComparison.InvariantCultureIgnoreCase)) 
                {

                }
            }

            return isValidValue;
        }

        protected private bool HasValidLength(PropertyValidatorContext context, Column column, string validationRule)
        {
            //var LengthBoundaries = validationRule.Substring(validationRule.IndexOf('[') + 1, validationRule.Length);
            var LengthBoundaries = validationRule.Substring(validationRule.IndexOf('[')).Replace("[","").Replace("]","");
            
            if (column.ColumnType.Equals("string", StringComparison.InvariantCultureIgnoreCase)
                || column.ColumnType.Equals("number", StringComparison.InvariantCultureIgnoreCase))
            {
                string value = column.Value?.ToString() ?? "{empty}";
                int minLength = GetArrayBoundary(LengthBoundaries, LengthBoundary.Minimum, _sheetName, column.Name);
                int maxLength = GetArrayBoundary(LengthBoundaries, LengthBoundary.Maximun, _sheetName, column.Name);
                if (value.Length < minLength || value.Length > maxLength) 
                {
                    context.MessageFormatter
                    .AppendArgument("SheetName", _sheetName)
                    .AppendArgument("RowNumber", _rowNumber)
                    .AppendArgument("ColumnName", column.ExcelColumnName)
                    .AppendArgument("ColumnValue", value)
                    .AppendArgument("ColumnMinLength", minLength)
                    .AppendArgument("ColumnMaxLength", maxLength);

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