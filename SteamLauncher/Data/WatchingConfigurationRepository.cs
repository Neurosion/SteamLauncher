using System;
using System.Collections.Generic;
using System.Linq;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public class WatchingConfigurationRepository : ConfigurationRepository, IWatchingConfigurationRepository
    {
        protected IConfigurationResourceWatcher ConfigurationWatcher { get; private set; }

        public event Action<IConfigurationElement> Added = delegate { };
        public event Action<IConfigurationElement> Removed = delegate { };
        public event Action<IConfigurationElement, IConfigurationElement> Updated = delegate { };

        public WatchingConfigurationRepository(IConfigurationResourceLocator resourceLocator, IConfigurationResourceWatcher configurationWatcher)
            :base(resourceLocator)
        {
            this.ConfigurationWatcher = configurationWatcher;

            ConfigurationWatcher.ResourceAdded += AddConfiguration;
            ConfigurationWatcher.ResourceRemoved += RemoveConfiguration;
            ConfigurationWatcher.ResourceUpdated += UpdateConfiguration;
        }

        private void AddConfiguration(int id, string name)
        {
            if (!CachedElements.ContainsKey(id))
            {
                var configuration = ConfigurationLocator.Locate(name).FirstOrDefault();
                CachedElements.Add(id, configuration);

                Added(configuration);
            }
        }

        private void RemoveConfiguration(int id, string name)
        {
            if (CachedElements.ContainsKey(id))
            {
                var config = CachedElements[id];
                CachedElements.Remove(id);

                Removed(config);
            }
        }

        private void UpdateConfiguration(int id, string name)
        {
            var configuration = ConfigurationLocator.Locate(name).FirstOrDefault();

            if (configuration != null && CachedElements.ContainsKey(id))
            {
                var oldConfiguration = CachedElements[id];
                oldConfiguration.Copy(configuration);

                Updated(oldConfiguration, configuration);
            }
        }
    }
}