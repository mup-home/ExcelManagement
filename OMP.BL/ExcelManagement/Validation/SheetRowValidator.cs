using FluentValidation;
using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Enums;
using OMP.BL.ExcelManagement.Helpers;

namespace OMP.BL.ExcelManagement.Validation
{
    public class SheetRowValidator: AbstractValidator<object>
    {
        public SheetRowValidator(string sheetName)
        {
            RuleForEach(r => r.GetType().GetProperties())
                .SetValidator(new ObjectPropertyValidator(sheetName))
                .WithSeverity(Severity.Warning);
                //.WithMessage(MessageProvider.GetErrorMessage(ExcelValidationError.SheetWithoutData, sheetName));
            /* RuleForEach(r => r.GetType().GetProperty("Data").GetValue(r))
                .SetValidator(r => new ColumnValidator(sheetName, int.Parse(typeof(object).GetProperty("RowNumber").GetValue(r).ToString())))
                .When(r => r.GetType().GetProperty("Data").Count > 0); */
        }
    }
}