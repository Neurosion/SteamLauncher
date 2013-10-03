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
        private IIdConverter _idConverter;

        public ConfigurationResourceLocator(string directory, string fileExtension, IConfigurationReader configurationReader, IIdConverter idConverter)
        {
            if (!Directory.Exists(directory))
                throw new ArgumentException(string.Format("The path {0} does not exist.", directory ?? string.Empty));

            _directory = directory;
            _fileExtension = !string.IsNullOrEmpty(fileExtension)
                                ? "." + fileExtension
                                : "";

            _configurationReader = configurationReader;
            _idConverter = idConverter;
        }

        public IEnumerable<IConfigurationElement> Locate(string name)
        {
            var filter = string.Format("{0}{1}", string.IsNullOrEmpty(name) ? "*" : name, _fileExtension);

            foreach (var currentFilePath in Directory.GetFiles(_directory, filter))
            {
                var fileText = File.ReadAllText(currentFilePath);
                var id = _idConverter.Convert(currentFilePath);
                yield return _configurationReader.Read(id, fileText);
            }
        }
    }
}