using System;
using System.Collections.Generic;
using FluentValidation;
using OMP.BL.ExcelManagement.Entities;

namespace OMP.BL.ExcelManagement.Validation
{
    public class SheetValidator: AbstractValidator<Sheet>
    {
        public SheetValidator()
        {
            RuleFor(s => s.Name).NotEmpty();
            RuleFor(s => s.TotalColumns)
                .GreaterThan(2)
                .WithMessage("Sheet must have more than {ComparisonValue} columns");
            // RuleFor(s => s.Data).Must(HasNotDuplicated);
            RuleForEach(s => s.Data).SetValidator(s => new SheetRowValidator(s.Name)).When(s => s.Data != null && s.Data.Count > 0);
        }
    }
}
