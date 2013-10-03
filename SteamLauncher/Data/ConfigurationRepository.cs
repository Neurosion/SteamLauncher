using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        protected IConfigurationResourceLocator ConfigurationLocator { get; private set; }
        protected Dictionary<int, IConfigurationElement> CachedElements { get; private set; }

        public ConfigurationRepository(IConfigurationResourceLocator configurationLocator)
        {
            CachedElements = new Dictionary<int, IConfigurationElement>();
            this.ConfigurationLocator = configurationLocator;
        }

        public IConfigurationElement Get(int id)
        {
            IConfigurationElement foundConfig = null;

            if (CachedElements.ContainsKey(id))
                foundConfig = CachedElements[id];
            else
            {
                foundConfig = ConfigurationLocator.Locate(id.ToString()).FirstOrDefault();
                AddElementToCache(foundConfig);
            }

            return foundConfig;
        }

        public IEnumerable<IConfigurationElement> Get()
        {
            ConfigurationLocator.Locate("*")
                                .Where(x => x != null)
                                .ForEach(x => AddElementToCache(x));

            var elements = CachedElements.Values;
            
            return elements;
        }

        private void AddElementToCache(IConfigurationElement element)
        {
            var rootElement = element as IRootConfigurationElement;

            if (rootElement != null && !CachedElements.ContainsKey(rootElement.Id))
                CachedElements.Add(rootElement.Id, rootElement);
        }
    }
}