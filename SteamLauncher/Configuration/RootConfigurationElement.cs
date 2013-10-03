using System;

namespace SteamLauncher.Domain.Configuration
{
    public class RootConfigurationElement : ConfigurationElement, IRootConfigurationElement
    {
        public int Id { get; set; }

        public RootConfigurationElement(string name)
            : base(name)
        {
        }

        public override void Copy(IConfigurationElement element)
        {
            base.Copy(element);

            var rootElement = element as IRootConfigurationElement;

            if (rootElement != null)
                this.Id = rootElement.Id;
        }
    }
}