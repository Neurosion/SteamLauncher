using System;
using System.Collections.Generic;
using System.Linq;

namespace SteamLauncher.Domain.Configuration
{
    public class ConfigurationElement : IConfigurationElement
    {
        public string Name { get; private set; }
        public IDictionary<string, string> Attributes { get; private set; }
        public IList<IConfigurationElement> Children { get; private set; }

        public ConfigurationElement(string name)
        {
            this.Name = name;
            Attributes = new Dictionary<string, string>();
            Children = new List<IConfigurationElement>();
        }
        
        public virtual void Copy(IConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            this.Name = element.Name;
            this.Attributes.Clear();
            element.Attributes.ForEach(x => this.Attributes.Add(x));
            this.Children.Clear();
            element.Children.ForEach(x => this.Children.Add(x));
        }
    }
}