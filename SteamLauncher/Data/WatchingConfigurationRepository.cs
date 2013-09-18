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
        public event Action<IConfigurationElement> Updated = delegate { };

        public WatchingConfigurationRepository(IConfigurationResourceLocator resourceLocator, IConfigurationResourceWatcher configurationWatcher)
            :base(resourceLocator)
        {
            this.ConfigurationWatcher = configurationWatcher;

            ConfigurationWatcher.ResourceAdded += AddConfiguration;
            ConfigurationWatcher.ResourceRemoved += RemoveConfiguration;
            ConfigurationWatcher.ResourceUpdated += UpdateConfiguration;
        }

        private void AddConfiguration(int id, string location)
        {
            UpdateConfiguration(id, location);
        }

        private void RemoveConfiguration(int id, string location)
        {
            var stringId = id.ToString();

            if (CachedElements.ContainsKey(stringId))
                CachedElements.Remove(stringId);
        }

        private void UpdateConfiguration(int id, string location)
        {
            var configuration = ConfigurationReader.Read(location);
            var stringId = id.ToString();

            if (!CachedElements.ContainsKey(stringId))
                CachedElements.Add(stringId, null);

            CachedElements[stringId] = configuration;
        }
    }
}