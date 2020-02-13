using System;
using System.Linq;
using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Enums;
using OMP.BL.ExcelManagement.Helpers;

namespace OMP.BL.ExcelManagement.Validation
{
    public class ObjectPropertyNotEmptyValidator: BasePropertyValidator
    {
        public ObjectPropertyNotEmptyValidator(string sheetName): base(sheetName, MessageProvider.GetPropertyValidationErrorMessage())
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            bool isValidValue = true;
            InitializeValidator(context, out string propertyName, out object propertyValue);

            bool hasMandatoryRule = column?.ValidationRules.Any(r => r.StartsWith("mandatory", StringComparison.InvariantCultureIgnoreCase)) ?? false;            
            if (hasMandatoryRule)
            {
                isValidValue = propertyValue != null ? !string.IsNullOrEmpty(propertyValue.ToString()) : false;
                if (!isValidValue)
                {
                    string errorMessage = MessageProvider.GetErrorMessage(ExcelValidationError.MandatoryFieldEmpty, _sheetName);
                    BuildContextMessage(context, column.ExcelColumnName, errorMessage);
                }
            }

            return isValidValue;
        }
    }
}