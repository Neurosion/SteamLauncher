using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SteamLauncher.Domain.Data
{
    public class ConfigurationResourceWatcher : ResourceWatcher, IConfigurationResourceWatcher
    {
        public ConfigurationResourceWatcher(string path, IIdConverter idConverter, string filter = null)
            : base(path, idConverter, filter)
        {
        }
    }
}