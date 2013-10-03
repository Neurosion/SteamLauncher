using System;
using System.Collections.Generic;
using System.Linq;

namespace SteamLauncher.Domain.Configuration
{
    public class RootConfigurationReader : IConfigurationReader
    {
        public IConfigurationElement Read(int id, string configurationData)
        {
            var splitData = configurationData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var readConfiguration = Read(id, splitData);
            return readConfiguration;
        }

        protected IConfigurationElement Read(int id, string[] configurationLines)
        {
            IConfigurationElement loadedElement = null;

            if (configurationLines.Any())
            {
                var configStack = new Stack<IConfigurationElement>();

                for (int i = 0; i < configurationLines.Length; i++)
                {
                    var splitLine = configurationLines[i].Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (splitLine.Length == 1)
                    {
                        // This is the beginning of a config element, build new element and push onto the stack
                        if (i < (configurationLines.Length - 1) && Clean(configurationLines[i + 1]) == "{")
                        {
                            loadedElement = CreateElement(id, Clean(splitLine[0]));

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

        protected virtual IConfigurationElement CreateElement(int id, string name)
        {
            var rootElement = new RootConfigurationElement(name)
            {
                Id = id
            };

            return rootElement;
        }

        protected string Clean(string value)
        {
            var cleanedValue = value.Replace("\t", string.Empty).Replace("\"", string.Empty);
            return cleanedValue;
        }
    }
}