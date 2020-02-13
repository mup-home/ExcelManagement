using System.Reflection;

namespace OMP.BL.ExcelManagement.Helpers
{
    public static class ObjectPropertyHelper
    {
        public static PropertyInfo GetProperty(string propertyName, object data)
        {
            return data.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }

        public static string GetPropertyValue(string propertyName, object data)
        {
            return data.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(data)?.ToString() ?? string.Empty;
        }

        public static object GetPropertyValueAsObject(string propertyName, object data)
        {
            return data.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(data);
        }

        public static void SetPropertyValue(string propertyName, object data, string newValue)
        {
            data.GetType().GetProperty(propertyName).SetValue(data, newValue);
        }
        public static void SetPropertyValueBool(string propertyName, object data, bool newValue)
        {
            data.GetType().GetProperty(propertyName).SetValue(data, newValue);
        }
    }
}
