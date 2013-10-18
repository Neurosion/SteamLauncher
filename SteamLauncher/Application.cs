using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SteamLauncher.Domain
{
    public class Application : IApplication
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public void Copy(IApplication target)
        {
            if (target == null)
                throw new ArgumentNullException();

            Id = target.Id;
            Name = target.Name;
        }
    }
}