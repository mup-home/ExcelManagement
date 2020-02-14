using System.Collections.Generic;
using System.Linq;
using OMP.BL.ExcelManagement.Helpers;

namespace OMP.BL.ExcelManagement.Extensions
{
    public static class GenericEnumerableExtensions
    {
        public static string ToDelimitedString(this IEnumerable<string> source, string separator) => string.Join(separator, source.ToArray());

        public static Dictionary<string, T> GetDuplicates<T>(this IEnumerable<T> list, int firstDataRow, List<string> uniqueKeys, out List<KeyValuePair<string, T>> listAsKeyValues) where T : class
        {
            var listAsKeyValuesWithRowNumber = new List<KeyValuePair<string, T>>();
            var tempDict = new Dictionary<string,T>();
            var duplicates = new Dictionary<string, T>();
            int rowIndex = firstDataRow;
            foreach (var item in list)
            {
                string itemUniqueKey = GetItemUniqueKey(item, uniqueKeys);
                string keyWithoutRowNumber = $"{rowIndex},{itemUniqueKey}";
                listAsKeyValuesWithRowNumber.Add(new KeyValuePair<string, T>(keyWithoutRowNumber, item));
                bool itemAdded = tempDict.TryAdd(itemUniqueKey, item);
                if(!itemAdded)
                {
                    duplicates.Add($"{rowIndex},{itemUniqueKey}", item);
                }
                rowIndex++;
            }
            listAsKeyValues = listAsKeyValuesWithRowNumber;
            return duplicates;
        }

        public static string GetDuplicateUniqueKeyRows<T>(this List<KeyValuePair<string, T>> source, List<string> uniqueKeys, List<string> duplicatedUniqueKeys)
        {
            string duplicateRowNumbers = string.Empty;
            duplicatedUniqueKeys.ForEach(k => 
            {
                var keys = k.Split(',');
                var duplicatesWithKey = source.Where(i => GetKeyWithoutRowNumber(i.Key) == GetKeyWithoutRowNumber(k)).Select(i => i);
                duplicatesWithKey.ToList().ForEach(d => 
                {
                    var rowNumber = GetRowNumberFromKey(d.Key);
                    duplicateRowNumbers += string.IsNullOrEmpty(duplicateRowNumbers) ? rowNumber : $",{rowNumber}";
                });
            });

            return duplicateRowNumbers;
        }

        private static string GetKeyWithoutRowNumber(string key)
        {
            int firstCommaIndex = key.IndexOf(',');
            string keyWithoutRowNumber = key.Substring(firstCommaIndex + 1);
            return keyWithoutRowNumber;
        }
        private static string GetRowNumberFromKey(string key) => key.Substring(0, key.IndexOf(','));

        private static string GetItemUniqueKey<T>(T item, List<string> uniqueKeys)
        {
            string itemUniqueKey = string.Empty;
            uniqueKeys.ForEach(key => 
            {
                string value = ObjectPropertyHelper.GetPropertyValue(key, item);
                itemUniqueKey += string.IsNullOrEmpty(itemUniqueKey) ? value : $",{value}";
            });
            return itemUniqueKey;
        }
    }
}