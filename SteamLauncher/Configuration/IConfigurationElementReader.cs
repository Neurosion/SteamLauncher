using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain.Configuration
{
    public interface IConfigurationReader
    {
        IConfigurationElement Read(int id, string configurationData);
    }
}