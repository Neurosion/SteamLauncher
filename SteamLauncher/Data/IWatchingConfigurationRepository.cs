using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public interface IWatchingConfigurationRepository : IConfigurationRepository
    {
        event Action<IConfigurationElement> Added;
        event Action<IConfigurationElement> Removed;
        event Action<IConfigurationElement> Updated;
    }
}