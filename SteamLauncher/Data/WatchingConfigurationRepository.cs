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
            var stringId = id.ToString();

            if (!CachedElements.ContainsKey(stringId))
            {
                var configuration = ConfigurationLocator.Locate(name).FirstOrDefault();
                CachedElements.Add(stringId, configuration);

                Added(configuration);
            }
        }

        private void RemoveConfiguration(int id, string name)
        {
            var stringId = id.ToString();

            if (CachedElements.ContainsKey(stringId))
            {
                var config = CachedElements[stringId];
                CachedElements.Remove(stringId);

                Removed(config);
            }
        }

        private void UpdateConfiguration(int id, string name)
        {
            var configuration = ConfigurationLocator.Locate(name).FirstOrDefault();
            var stringId = id.ToString();

            if (!CachedElements.ContainsKey(stringId))
                CachedElements.Add(stringId, null);

            var oldConfiguration = CachedElements[stringId];
            CachedElements[stringId] = configuration;

            Updated(oldConfiguration, configuration);
        }
    }
}