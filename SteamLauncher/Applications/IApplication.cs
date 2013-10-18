using System;
using System.Collections.ObjectModel;

namespace SteamLauncher.Domain
{
    public interface IApplication : IIdentifiable<int>, ICopyable<IApplication>
    {
        string Name { get; set; }
    }
}