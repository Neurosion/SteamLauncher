using System.Collections.Generic;

namespace SteamLauncher.Domain.Data
{
    public interface IConfigurationElement
    {
        string Name { get; }
        IList<IConfigurationElement> Children { get; }
        IDictionary<string, string> Attributes { get; }
    }
}
