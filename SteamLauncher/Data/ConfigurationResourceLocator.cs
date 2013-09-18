using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public class ConfigurationResourceLocator : IConfigurationResourceLocator
    {
        private string _directory;
        private string _fileExtension;
        private IConfigurationReader _configurationReader;

        public ConfigurationResourceLocator(string directory, string fileExtension, IConfigurationReader configurationReader)
        {
            if (!Directory.Exists(directory))
                throw new ArgumentException(string.Format("The path {0} does not exist.", directory));

            _directory = directory;
            _fileExtension = !string.IsNullOrEmpty(fileExtension)
                                ? "." + fileExtension
                                : "";

            _configurationReader = configurationReader;
        }

        public IEnumerable<IConfigurationElement> Locate(string name)
        {
            foreach (var currentFilePath in Directory.GetFiles(_directory, string.Format("{0}{1}", string.IsNullOrEmpty(name) ? "*" : name, _fileExtension)))
            {
                var fileText = File.ReadAllText(currentFilePath);
                yield return _configurationReader.Read(fileText);
            }
        }
    }
}