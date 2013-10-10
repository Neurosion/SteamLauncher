using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using SteamLauncher.UI.Core;
using SteamLauncher.Domain;

namespace SteamLauncher.UI.ViewModels
{
    public interface IMainWindowViewModel : INotifyPropertyChanged
    {
        IEnumerable<IFilteredApplicationCategory> ApplicationCategories { get; }
        string Filter { get; set; }
        bool IsVisible { get; }
        void Launch(IApplication application);
    }
}