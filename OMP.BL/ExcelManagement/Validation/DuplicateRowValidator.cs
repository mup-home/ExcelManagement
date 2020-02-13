using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using OMP.BL.ExcelManagement.Entities;
using OMP.BL.ExcelManagement.Enums;
using OMP.BL.ExcelManagement.Helpers;

namespace OMP.BL.ExcelManagement.Validation
{
    public class DuplicateRowValidator: AbstractValidator<List<object>>
    {
        public DuplicateRowValidator(string sheetName)
        {
            RuleFor(rows => rows)
                .Must(HasNotDuplicateRows);
        }

        private bool HasNotDuplicateRows(List<object> rows)
        {
            var myhash = new HashSet<int>();
            var primariesKey = new List<string>();
            var duplcatedRows = new List<List<KeyValuePair<string, object>>>();
                        
            
            return true;
        }
    }
}