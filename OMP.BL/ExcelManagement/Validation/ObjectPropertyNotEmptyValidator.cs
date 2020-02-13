using System;
using System.Linq;
using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Enums;
using OMP.BL.ExcelManagement.Helpers;

namespace OMP.BL.ExcelManagement.Validation
{
    public class ObjectPropertyNotEmptyValidator: PropertyValidator
    {
        private static int _rowNumber;
        private static object rowObject = new object();
        private static string _sheetName = string.Empty;

        public ObjectPropertyNotEmptyValidator(string sheetName): base(MessageProvider.GetPropertyValidationErrorMessage())
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
            if (hasMandatoryRule)
            {
                isValidValue = propertyValue != null ? !string.IsNullOrEmpty(propertyValue.ToString()) : false;
                if (!isValidValue)
                {
                    string errorMessage = MessageProvider.GetErrorMessage(ExcelValidationError.MandatoryFieldEmpty, _sheetName);
                    ValidatorHelper.BuildContextMessage(context, _sheetName, _rowNumber, column.ExcelColumnName, errorMessage);
                }
            }

            return isValidValue;
        }
    }
}