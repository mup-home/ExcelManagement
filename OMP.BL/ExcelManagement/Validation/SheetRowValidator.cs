using FluentValidation;
using OMP.BL.ExcelManagement.Entities;

namespace OMP.BL.ExcelManagement.Validation
{
    public class SheetRowValidator: AbstractValidator<Sheet>
    {
        public SheetRowValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.TotalColumns).GreaterThan(2);
            RuleFor(r => r).Must(HasAnyData).When(r => r.ValidateFormat);
        }

        private bool HasAnyData(Sheet sheet)
        {
            return sheet.Data.Count > 0;
        }
    }
}
