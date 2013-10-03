using System;
using System.Collections.Generic;

namespace SteamLauncher.Domain.Configuration
{
    public interface IConfigurationElement
    {
        string Name { get; }
        IList<IConfigurationElement> Children { get; }
        IDictionary<string, string> Attributes { get; }
        void Copy(IConfigurationElement element);
    }
}