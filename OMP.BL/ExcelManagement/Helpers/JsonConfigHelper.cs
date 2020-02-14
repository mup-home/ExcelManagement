using OMP.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using Utf8Json.Resolvers;

namespace OMP.BL.ExcelManagement.Helpers
{
    public static class JsonConfigHelper
    {
        public static T ReadJsonConfig<T>(string name, List<string> errors)
        {
            string jsonConfigPath = ConfigurationHelper.GetKeyValue("SheetConfigPath");
            string configType = typeof(T).Name.ToLower();
            try
            {
                string fileFullPath = $"{jsonConfigPath}/{GetConfiTypeJsonPrefix(configType)}_{name}.json";
                if (File.Exists(fileFullPath))
                {
                    var jsonData = File.ReadAllText(fileFullPath);
                    return Utf8Json.JsonSerializer.Deserialize<T>(jsonData, StandardResolver.CamelCase);
                }
                errors?.Add(MessageProvider.GetJsonConfigFileNotFound(configType, name));
                return (T)Activator.CreateInstance(typeof(T));                
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    errors?.Add(MessageProvider.GetInvalidJsonConfigFormat(configType, name, ex.Message));
                    return (T)Activator.CreateInstance(typeof(T));
                } 
                else
                {
                    throw new FileNotFoundException(MessageProvider.GetJsonConfigFileNotFound(configType, name), ex);
                }                
            }
            
        }

        private static object GetConfiTypeJsonPrefix(string configType)
        {
            return configType.Equals("sheetcolumn") ? "column" : configType;
        }
    }
}