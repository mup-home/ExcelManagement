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
    public class DuplicateRowValidator: PropertyValidator
    {
        private string _sheetName;

        public DuplicateRowValidator(string sheetName): base(MessageProvider.GetDuplicateRowMessage())
        { 
            _sheetName = sheetName;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var data = context.Instance as List<SheetRow>;
            return HasDuplicateRow(context, data);
        }

        protected private bool HasDuplicateRow(PropertyValidatorContext context, List<SheetRow> data)
        {
            var primariesKey = new List<string>();
            var rows = new List<List<KeyValuePair<string, object>>>();
            data.ForEach(r => {
                var row = new List<KeyValuePair<string, object>>();
                row.Add(new KeyValuePair<string, object>("rowNumber", r.RowNumber));
                /* r.Data.ForEach(c => {
                    if (c.IsPrimaryKey)
                    {
                        primariesKey.Add(c.Name);
                    }
                    row.Add(new KeyValuePair<string, object>(c.Name, c.Value));
                }); */
                rows.Add(row);
            });
            
            string rowKey = "";
            rows.ForEach(r => {
                
                foreach (var primaryKey in primariesKey)
                {
                    rowKey += r.Find(k => k.Key == primaryKey).Value.ToString();
                }
                r.Add(new KeyValuePair<string, object>("rowKey", rowKey)); 
            });


            if (rows.Count > 1000) 
            {
                context.MessageFormatter
                .AppendArgument("SheetName", _sheetName)
                .AppendArgument("RowKey", rowKey);

                return false;
            }
            return true;
        }
    }
}