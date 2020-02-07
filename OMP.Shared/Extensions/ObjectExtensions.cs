namespace OMP.Shared.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class ObjectExtensions
    {
        /// <summary>
        /// Return true is the object is not null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object value)
        {
            return value != null;
        }

        /// <summary>
        /// Return true is the object is null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNull(this object value)
        {
            return value == null;
        }

        public static DateTime ToDateTime(this object value)
        {
            return value.ToString().ToDateTime();
        }

        public static double ToDouble(this object value)
        {
            return value.ToString().ToDouble();
        }

        public static decimal ToDecimal(this object value)
        {
            return value.ToString().ToDecimal();
        }

        public static int ToInt(this object value)
        {
            return value.ToString().ToInt();
        }

        public static short ToShort(this object value)
        {
            return value.ToString().ToShort();
        }

        public static T DeepClone<T>(this T value)
        {
            try
            {
                if (value.IsNull())
                {
                    return default;
                }

                Type valueType = value.GetType();
                if (valueType.IsPrimitive || valueType == typeof(string) || valueType == typeof(DateTime))
                {
                    return value;
                }
                else if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var clonedValueItems = (IList)Activator.CreateInstance(valueType);
                    foreach (var item in (IList)value)
                    {
                        clonedValueItems.Add(item.DeepClone());
                    }
                    return (T)clonedValueItems;
                }
                else
                {
                    var clone = Activator.CreateInstance(valueType);
                    foreach (var property in clone.GetType().GetProperties())
                    {
                        property.SetValue(clone, property.GetValue(value).DeepClone());
                    }
                    return (T)clone;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Cloning object error", ex);
            }
        }
    }
}
