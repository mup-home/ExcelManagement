using OMP.Dtos;
using System.IO;
using Utf8Json;

namespace OMP.Shared
{
    public static class ConfigurationHelper
    {
        private static CustomAppSettings _config;

        public static void Init(string configFile)
        {
            var configAsJson = JsonSerializer.Deserialize<dynamic>(File.ReadAllText(configFile));
            var customConfigPath = configAsJson["CustomAppSettings"];
            _config = JsonSerializer.Deserialize<CustomAppSettings>(File.ReadAllText(customConfigPath));                        
        }

        public static string GetKeyValue(string key) => _config.GetType().GetProperty(key)?.GetValue(_config).ToString() ?? string.Empty;
    }
}
