﻿using System;
using System.Collections.Generic;
using System.Linq;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public abstract class WatchedConfigurationBasedElementRepository<ItemType, IdType> : IRepository<ItemType, IdType>
        where ItemType: IIdentifiable<IdType>, ICopyable<ItemType>
    {
        protected IWatchingConfigurationRepository ConfigurationRepository { get; private set; }
        protected List<ItemType> Items { get; private set; }

        public WatchedConfigurationBasedElementRepository(IWatchingConfigurationRepository configurationRepository)
        {
            Items = new List<ItemType>();

            this.ConfigurationRepository = configurationRepository;

            LoadInitialItems();

            this.ConfigurationRepository.Added += AddItem;
            this.ConfigurationRepository.Removed += RemoveItem;
            this.ConfigurationRepository.Updated += UpdateItem;
        }

        private void LoadInitialItems()
        {
            var allConfigurations = ConfigurationRepository.Get();

            if (allConfigurations != null)
            {
                var loadedItems = allConfigurations.Where(config => config != null)
                                                   .Select(config => Load(config))
                                                   .Where(app => app != null);
                Items.AddRange(loadedItems);
            }
        }

        public ItemType Get(IdType id)
        {
            var foundItem = Items.Where(x => AreIdsEqual(x, id))
                                 .FirstOrDefault();
            return foundItem;
        }

        private bool AreIdsEqual(ItemType item, IdType id)
        {
            var doesMatch = item != null && item.Id.Equals(id);
            return doesMatch;
        }

        private bool AreIdsEqual(ItemType left, ItemType right)
        {
            var doesMatch = left != null && right != null && left.Id.Equals(right.Id);
            return doesMatch;
        }

        public IEnumerable<ItemType> Get()
        {
            return Items;
        }

        protected void AddItem(IConfigurationElement configuration)
        {
            var item = Load(configuration);

            if (item != null)
                Items.Add(item);
        }

        protected void RemoveItem(IConfigurationElement configuration)
        {
            var item = Load(configuration);
            Items.RemoveAll(x => AreIdsEqual(x, item));
        }

        protected void UpdateItem(IConfigurationElement oldConfiguration, IConfigurationElement newConfiguration)
        {
            var oldItem = Load(oldConfiguration);
            var newItem = Load(newConfiguration);

            if (oldItem != null)
                foreach (var currentItem in Items.Where(x => AreIdsEqual(x, oldItem)))
                    currentItem.Copy(newItem);
        }

        protected abstract ItemType Load(IConfigurationElement configuration);
    }
}