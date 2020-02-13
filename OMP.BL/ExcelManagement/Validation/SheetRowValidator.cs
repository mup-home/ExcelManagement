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
                .SetValidator(new ObjectPropertyNotEmptyValidator(sheetName))
                .WithSeverity(Severity.Error)
                .SetValidator(new ObjectPropertyLengthValidator(sheetName))
                .WithSeverity(Severity.Warning);
        }
    }
}