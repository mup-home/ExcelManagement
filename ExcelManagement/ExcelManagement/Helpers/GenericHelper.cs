using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMP.BL.ExcelManagement.Helpers
{
    public static class GenericHelper
    {
        public static Type FindType(string fullName)
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name.Equals(fullName));
        }
    }
}
