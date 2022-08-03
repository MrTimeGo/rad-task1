using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace EtlService.Configuration
{
    public class Configuration : IConfiguration
    {
        private readonly string configurationFilePath;

        private ConfigurationModel? configurationModel;

        public Configuration(string configurationFilePath)
        {
            this.configurationFilePath = configurationFilePath;
        }

        public string GetOutputFolderPath()
        {
            if (configurationModel == null)
            {
                throw new NullReferenceException("Configuration is null");
            }
            return configurationModel.OutputFolderPath;
        }

        public string GetSourceFolderPath()
        {
            if (configurationModel == null)
            {
                throw new NullReferenceException("Configuration is null");
            }
            return configurationModel.SourceFolderPath;
        }

        public void ValidateConfiguration()
        {
            ValidateIfConfigFileIsPresent();
            ValidateIfConfigFileCorrect();
        }

        private void ValidateIfConfigFileCorrect()
        {
            LoadConfiguration();

            if (configurationModel == null)
            {
                throw new ArgumentException("JSON can't be deserialized");
            }
            if (string.IsNullOrEmpty(configurationModel.SourceFolderPath))
            {
                throw new ArgumentException("Source folder path is not provided");
            }
            if (string.IsNullOrEmpty(configurationModel.OutputFolderPath))
            {
                throw new ArgumentException("Output folder path is not provided");
            }
        }

        private void ValidateIfConfigFileIsPresent()
        {
            if (string.IsNullOrEmpty(configurationFilePath))
            {
                throw new ArgumentException("Path to configuration file is not provided");
            }
            if (!File.Exists(configurationFilePath))
            {
                throw new FileNotFoundException("Configuration file was not founded", configurationFilePath);
            }
        }

        private void LoadConfiguration()
        {
            string jsonString = File.ReadAllText(configurationFilePath);
            configurationModel = JsonSerializer.Deserialize<ConfigurationModel>(jsonString);
        }
    }
}
