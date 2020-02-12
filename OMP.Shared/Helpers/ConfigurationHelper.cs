using OMP.Dtos;
using System.Diagnostics;
using System.IO;
using Utf8Json;

namespace OMP.Shared
{
    public static class ConfigurationHelper
    {
        private static CustomAppSettings _config;

        public static void Init(string configFile)
        {
            try
            {
                var configAsJson = JsonSerializer.Deserialize<dynamic>(File.ReadAllText(configFile));
                var customConfigPath = configAsJson["CustomAppSettings"];
                _config = JsonSerializer.Deserialize<CustomAppSettings>(File.ReadAllText(customConfigPath));  
            }
            catch (JsonParsingException ex)
            {
                Debug.WriteLine(ex);
                throw;
            }                                  
        }

        public static string GetKeyValue(string key) => _config.GetType().GetProperty(key)?.GetValue(_config).ToString() ?? string.Empty;
    }
}
