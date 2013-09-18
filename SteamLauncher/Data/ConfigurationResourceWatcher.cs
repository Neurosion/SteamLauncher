using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SteamLauncher.Domain.Data
{
    public class ConfigurationResourceWatcher : ResourceWatcherBase<int>, IConfigurationResourceWatcher
    {
        public ConfigurationResourceWatcher(string path, string filter)
            : base(path, filter)
        {
        }

        protected override int GetIdFromPath(string filePath)
        {
            var bareName = Path.GetFileNameWithoutExtension(filePath);
            var splitName = bareName.Split(new[] { '_' });

            int id = 0;

            if (splitName.Length == 2)
                int.TryParse(splitName[1], out id);

            return id;
        }
    }
}