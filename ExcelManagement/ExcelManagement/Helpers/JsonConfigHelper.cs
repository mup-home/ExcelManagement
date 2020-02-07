using OMP.BL.ExcelManagement.Enums;
using OMP.Shared;
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
                string fileFullPath = $"{jsonConfigPath}/{configType}_{name}.json";
                if (File.Exists(fileFullPath))
                {
                    var jsonData = File.ReadAllText(fileFullPath);
                    return Utf8Json.JsonSerializer.Deserialize<T>(jsonData, StandardResolver.Default);
                }
                errors?.Add($"Configuration file: {configType}_{name}.json, not found");
                return (T)Activator.CreateInstance(typeof(T));                
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    errors?.Add($"Invalid JSON format on file: {configType}_{name}.json: message: {ex.Message}");
                    return (T)Activator.CreateInstance(typeof(T));
                } 
                else
                {
                    throw new FileNotFoundException($"Configuration file: {configType}_{name}.json, not found", ex);
                }                
            }
            
        }
    }
}
