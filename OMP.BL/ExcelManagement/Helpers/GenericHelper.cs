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

        public static object ToAnonymousObject<TValue>(this IDictionary<string, TValue> value)
        {
            var types = new Type[value.Count];

            for (int i = 0; i < types.Length; i++)
            {
                types[i] = typeof(TValue);
            }

            // dictionaries don't have an order, so we force an order based
            // on the Key
            var ordered = value.OrderBy(x => x.Key).ToArray();

            string[] names = Array.ConvertAll(ordered, x => x.Key);

            Type type = AnonymousTypeHelper.CreateType(types, names);

            object[] values = Array.ConvertAll(ordered, x => (object)x.Value);

            object anonymousObject = type.GetConstructor(types).Invoke(values);

            return anonymousObject;
        }
    }
}
