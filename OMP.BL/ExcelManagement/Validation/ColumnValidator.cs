using System;
using FluentValidation;
using OMP.BL.ExcelManagement.Entities;

namespace OMP.BL.ExcelManagement.Validation
{
    class ColumnValidator: AbstractValidator<Column>
    {
        public ColumnValidator(string sheetName, int rowNumber)
        {
            RuleFor(c => c.Name).NotEmpty();
            When(c => c.Mandatory, () => {
                RuleFor(c => c.Value)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .WithMessage(c => $"Sheet: {sheetName} [Row: {rowNumber}, Column: {c.ExcelColumnName}] is mandatory.");
            });
            When(c => !c.Mandatory, () => {
                RuleFor(c => c.Value)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .SetValidator(new ColumnValueValidator(sheetName, rowNumber))
                    .When(c => c.Value != null);
            });            
        }
    }
}
