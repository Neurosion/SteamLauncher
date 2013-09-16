using System;
using System.Collections.ObjectModel;

namespace SteamLauncher.Domain
{
    public interface IApplication
    {
        int Id { get; }
        string Name { get; }
        //string ImagePath { get; }
        ObservableCollection<string> Categories { get; }
    }
}