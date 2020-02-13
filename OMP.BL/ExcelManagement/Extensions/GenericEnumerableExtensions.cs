using System;
using System.Collections.Generic;
using System.Linq;
using OMP.BL.ExcelManagement.Helpers;

namespace OMP.BL.ExcelManagement.Extensions
{
    public static class GenericEnumerableExtensions
    {
        public static Dictionary<string, T> GetDuplicates<T>(this IEnumerable<T> list, List<string> uniqueKeys) where T : class
        {
            var tempDict = new Dictionary<string,T>();
            var duplicates = new Dictionary<string, T>();
            int rowIndex = 1;
            foreach (var item in list)
            {
                string itemUniqueKey = GetItemUniqueKey(item, uniqueKeys);
                bool itemAdded = tempDict.TryAdd(itemUniqueKey, item);
                if(!itemAdded)
                {
                    duplicates.Add($"{rowIndex},{itemUniqueKey}", item);
                }
                rowIndex++;
            }
            return duplicates;
        }

        public static string GetDuplicateUniqueKeyRows<T>(this IEnumerable<T> list, List<string> uniqueKeys, List<string> duplicatedUniqueKeys)
        {
            string duplicateRowNumbers = string.Empty;
            var listAsKeyValues = new List<KeyValuePair<string, T>>();
            int rowIndex = 1;
            list.ToList().ForEach(item => 
            {
                var itemUniqueKey = GetItemUniqueKey(item, uniqueKeys);
                listAsKeyValues.Add(new KeyValuePair<string, T>($"{rowIndex++},{itemUniqueKey}", item));
            });
            duplicatedUniqueKeys.ForEach(k => 
            {
                var keys = k.Split(',');
                var duplicatesWithKey = listAsKeyValues.Where(i => i.Key.Substring(i.Key.IndexOf(',')) == k.Substring(k.IndexOf(',')));
                duplicatesWithKey.ToList().ForEach(d => 
                {
                    var row = k.Substring(k.IndexOf(','));
                    duplicateRowNumbers += string.IsNullOrEmpty(duplicateRowNumbers) ? row : $",{row}";
                });
            });

            return duplicateRowNumbers;
        }

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

        public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> extList, Func<T, object> groupProps) where T : class
        { 
            return extList
                .GroupBy(groupProps)
                .SelectMany(z => z);
        }

        public static IEnumerable<T> GetMoreThanOnceRepeated<T>(this IEnumerable<T> extList, Func<T, object> groupProps) where T : class
        { 
            //Return only the second and next reptition
            return extList
                .GroupBy(groupProps)
                .SelectMany(z => z.Skip(1)); //Skip the first occur and return all the others that repeats
        }
        
        public static IEnumerable<T> getAllRepeated<T>(this IEnumerable<T> extList, Func<T, object> groupProps) where T : class
        {
            //Get All the lines that has repeating
            return extList
                .GroupBy(groupProps)
                .Where(z => z.Count() > 1) //Filter only the distinct one
                .SelectMany(z => z);//All in where has to be retuned
        }
    }
}