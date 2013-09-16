using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SteamLauncher.Domain
{
    [Serializable]
    public class Application : IApplication
    {
        public int Id { get; private set; }
        public string Name {get; private set; }
        public ObservableCollection<string> Categories { get; private set; }

        public Application(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}