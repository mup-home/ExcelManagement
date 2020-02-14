using System;
using System.Collections.Generic;
using System.Linq;

namespace OMP.BL.ExcelManagement.Helpers
{
    public static class GenericHelper
    {
        public static Type FindType(string entityName)
        {
            var entityType = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name.Equals(entityName, StringComparison.InvariantCulture));
            return entityType ?? null;
        }

        public static Type FindTypeByAssembly(string assemblyStartWith, string entityName)
        {
            if (string.IsNullOrEmpty(assemblyStartWith))
            {
                return FindType(entityName);
            }
            var entityType = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith(assemblyStartWith, StringComparison.InvariantCultureIgnoreCase) && !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name.Equals(entityName, StringComparison.InvariantCulture));
            return entityType ?? null;
        }
    }
}
