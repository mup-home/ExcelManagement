using FluentValidation;
using OMP.BL.ExcelManagement.Entities;

namespace OMP.BL.ExcelManagement.Validation
{
    class ColumnValidator: AbstractValidator<Column>
    {
        public ColumnValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Value).Custom((value, context) => {
                if (context.InstanceToValidate.GetType().GetProperty("ColumnType").GetValue(context.InstanceToValidate) != value.GetType())
                    context.AddFailure("Invalid property value type");
                });
        }
    }
}
