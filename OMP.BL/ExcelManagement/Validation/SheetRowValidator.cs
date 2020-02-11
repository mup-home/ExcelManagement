using FluentValidation;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Enums;
using OMP.BL.ExcelManagement.Helpers;

namespace OMP.BL.ExcelManagement.Validation
{
    public class SheetRowValidator: AbstractValidator<SheetRow>
    {
        public SheetRowValidator(string sheetName)
        {
            RuleFor(r => r.Data)
                .NotNull()
                .WithMessage(MessageProvider.GetErrorMessage(ExcelValidationError.SheetWithoutData, sheetName));
            RuleForEach(r => r.Data)
                .SetValidator(r => new ColumnValidator(sheetName, r.RowNumber))
                .When(r => r.Data.Count > 0);
        }
    }
}