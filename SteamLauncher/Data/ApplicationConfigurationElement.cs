using System;
using System.Collections.Generic;
using System.Linq;

namespace SteamLauncher.Domain.Data
{
    public class ApplicationConfigurationElement : IConfigurationElement
    {
        public string Name { get; private set; }
        public IDictionary<string, string> Attributes { get; private set; }
        public IList<IConfigurationElement> Children { get; private set; }

        public ApplicationConfigurationElement(string name)
        {
            this.Name = name;
            Attributes = new Dictionary<string, string>();
            Children = new List<IConfigurationElement>();
        }
    }
}