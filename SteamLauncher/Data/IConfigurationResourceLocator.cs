using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public interface IConfigurationResourceLocator : IResourceLocator<IConfigurationElement>
    {
    }
}