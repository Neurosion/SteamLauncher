﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        protected IConfigurationResourceLocator ConfigurationLocator { get; private set; }
        protected Dictionary<string, IConfigurationElement> CachedElements { get; private set; }
        protected IConfigurationReader ConfigurationReader { get; private set; }

        public ConfigurationRepository(IConfigurationResourceLocator configurationLocator)
        {
            CachedElements = new Dictionary<string, IConfigurationElement>();
            this.ConfigurationLocator = configurationLocator;
        }

        public IConfigurationElement Get(string id)
        {
            var foundConfig = CachedElements.ContainsKey(id)
                                ? CachedElements[id]
                                : GetFilteredConfigurations(id).FirstOrDefault();
            return foundConfig;
        }

        public IEnumerable<IConfigurationElement> Get()
        {
            var foundConfigurations = GetFilteredConfigurations();
            return foundConfigurations;
        }

        private IEnumerable<IConfigurationElement> GetFilteredConfigurations(string id = null)
        {
            var locatedConfigurations = ConfigurationLocator.Locate(id ?? "*") ?? new IConfigurationElement[] { };
            return locatedConfigurations;
        }
    }
}