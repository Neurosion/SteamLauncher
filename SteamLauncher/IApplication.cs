using System;
using System.Collections.ObjectModel;

namespace SteamLauncher.Domain
{
    public interface IApplication : IIdentifiable<int>, ICopyable<IApplication>
    {
        string Name { get; set; }
        //string ImagePath { get; set; }
        //ObservableCollection<string> Categories { get; }
    }
}