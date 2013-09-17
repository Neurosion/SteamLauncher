using System;
using System.Collections.ObjectModel;

namespace SteamLauncher.Domain
{
    public interface IApplication
    {
        int Id { get; set; }
        string Name { get; set; }
        string ImagePath { get; set; }
        ObservableCollection<string> Categories { get; }
    }
}