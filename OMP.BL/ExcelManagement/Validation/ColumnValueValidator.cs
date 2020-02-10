using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Entities;
using System;

namespace OMP.BL.ExcelManagement.Validation
{
    public class ColumnValueValidator: PropertyValidator
    {
        private string _sheetName;
        private int _rowNumber;

	    public ColumnValueValidator(string sheetName, int rowNumber): base("Sheet: {SheetName} [Row: {RowNumber}, Column: {ColumnName}] -  Value: {ColumnValue}, length must be between {ColumnMinLength} and {ColumnMaxLength} characters.")
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
                
                if (rule.Equals("length", StringComparison.InvariantCultureIgnoreCase))
                {
                    isValidValue = HasValidLength(context, column);
                }
            }

            return isValidValue;
        }

        protected private bool HasValidLength(PropertyValidatorContext context, Column column)
        {
            if (column.ColumnType.Equals("string", StringComparison.InvariantCultureIgnoreCase)
                || column.ColumnType.Equals("number", StringComparison.InvariantCultureIgnoreCase))
            {
                string value = column.Value?.ToString() ?? "{empty}";
                if (value.Length < column.MinLength || value.Length > column.MaxLength) 
                {
                    context.MessageFormatter
                    .AppendArgument("SheetName", _sheetName)
                    .AppendArgument("RowNumber", _rowNumber)
                    .AppendArgument("ColumnName", column.ExcelColumnName)
                    .AppendArgument("ColumnValue", value)
                    .AppendArgument("ColumnMinLength", column.MinLength)
                    .AppendArgument("ColumnMaxLength", column.MaxLength);

                    return false;
                }
            }
            return true;
        }
    }
}