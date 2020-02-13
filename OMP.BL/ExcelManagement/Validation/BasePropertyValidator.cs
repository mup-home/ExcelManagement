using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Helpers;
using System;
using System.Linq;

namespace OMP.BL.ExcelManagement.Validation
{
    public class BasePropertyValidator : PropertyValidator
    {
        protected static string _sheetName = string.Empty;
        protected static int _rowNumber;
        protected static object rowObject = new object();

        protected object data;

        protected Column column;

        public BasePropertyValidator(string sheetName, string ErrorMessage):  base(ErrorMessage)
        {
            if (!_sheetName.Equals(sheetName))
            {
                _sheetName = sheetName;
                _rowNumber = ExcelManagementHelper.BookConfig.Sheets[sheetName].DataStartingRow - 1;
            };
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            throw new NotImplementedException();
        }

        protected void InitializeValidator(PropertyValidatorContext context, out string propertyName, out object propertyValue) 
        {
            var objectPropertyName = ObjectPropertyHelper.GetPropertyValue("Name", context.PropertyValue);
            data = context.Instance as object;
            if (!rowObject.Equals(data)) {
                _rowNumber += 1;
                rowObject = data;
            }

            var sheetColumns = ExcelManagementHelper.BookConfig.SheetColumns[_sheetName];
            column = sheetColumns.Values.FirstOrDefault(v => v.Name == objectPropertyName);
            propertyName = objectPropertyName;
            propertyValue =  ObjectPropertyHelper.GetPropertyValueAsObject(propertyName, data);
        }

        protected void BuildContextMessage(PropertyValidatorContext context, string columnName, string errorMessage)
        {
            context.MessageFormatter
                .AppendArgument("SheetName", _sheetName)
                .AppendArgument("RowNumber", _rowNumber)
                .AppendArgument("ColumnName", columnName)
                .AppendArgument("ValidationErrorMessage", errorMessage);
        }
    }
}