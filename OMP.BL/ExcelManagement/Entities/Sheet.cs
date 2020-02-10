﻿using System.Collections.Generic;

namespace OMP.BL.ExcelManagement.Entities
{
    public class Sheet
    {
        public string Name { get; set; }

        public int DataStartingRow { get; set; }

        public int TotalColumns { get; set; }

        public bool ValidateFormat { get; set; }

        public List<string> Columns { get; set; }

        public List<List<Column>> Data { get; set; }
    }
}
