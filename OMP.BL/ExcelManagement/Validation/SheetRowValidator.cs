using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Entities;

namespace OMP.BL.ExcelManagement.Validation
{
    public class SheetRowValidator: AbstractValidator<SheetRow>
    {
        public SheetRowValidator(string sheetName)
        {
            RuleFor(r => r.Data).NotNull();
            RuleForEach(r => r.Data).SetValidator(r => new ColumnValidator(sheetName, r.RowNumber)).When(r => r.Data.Count > 0);
        }
    }
}