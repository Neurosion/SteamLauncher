using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SteamLauncher.Domain.Data
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private string _configurationPath;
        private string _configurationFileExtension;

        public ConfigurationRepository(string configurationPath, string configurationFileExtension)
        {
            if (!Directory.Exists(configurationPath))
                throw new ArgumentException(string.Format("The path {0} does not exist.", configurationPath));

            _configurationPath = configurationPath;
            _configurationFileExtension = configurationFileExtension;
        }

        public IConfigurationElement Get(string id)
        {
            var foundConfig = GetFilteredConfigurations(string.Format("*{0}.{1}", id, _configurationFileExtension)).FirstOrDefault();
            return foundConfig;
        }

        public IEnumerable<IConfigurationElement> Get()
        {
            var foundConfigurations = GetFilteredConfigurations(string.Format("*.{0}", _configurationFileExtension));
            return foundConfigurations;
        }

        private IEnumerable<IConfigurationElement> GetFilteredConfigurations(string filter)
        {
            foreach (var currentFile in Directory.GetFiles(_configurationPath, filter))
                yield return LoadConfig(currentFile);
        }

        private IConfigurationElement LoadConfig(string filePath)
        {
            var fileContents = File.ReadAllLines(filePath);
            IConfigurationElement loadedElement = null;

            if (fileContents.Any())
            {
                var configStack = new Stack<IConfigurationElement>();

                for (int i = 0; i < fileContents.Length; i++)
                {
                    var splitLine = fileContents[i].Split(new [] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (splitLine.Length == 1)
                    {
                        // This is the beginning of a config element, build new element and push onto the stack
                        if (i < (fileContents.Length - 1) && Clean(fileContents[i + 1]) == "{")
                        {
                            loadedElement = new ApplicationConfigurationElement(Clean(splitLine[0]));

                            // If there is an element currently on the stack, add the loaded element to its children
                            if (configStack.Any())
                                configStack.Peek().Children.Add(loadedElement);

                            configStack.Push(loadedElement);
                            i++; // skip the next line, we have already determined it's the body start of this element
                        }
                        // This is the end of a config element, pop off the stack
                        else if (Clean(splitLine[0]) == "}" && configStack.Any())
                            loadedElement = configStack.Pop();
                    }
                    // This is an attribute line, if an element exists on the stack add to its attributes
                    else if (splitLine.Length == 2 && configStack.Any())
                        configStack.Peek().Attributes.Add(Clean(splitLine[0]), Clean(splitLine[1]));
                }
            }

            return loadedElement;
        }

        private string Clean(string value)
        {
            var cleanedValue = value.Replace("\t", string.Empty).Replace("\"", string.Empty);
            return cleanedValue;
        }
    }
}