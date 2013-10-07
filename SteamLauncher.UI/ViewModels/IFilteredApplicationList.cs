using System;
using System.Collections.Generic;
using System.ComponentModel;
using SteamLauncher.Domain;

namespace SteamLauncher.UI.ViewModels
{
    public interface IFilteredApplicationList : INotifyPropertyChanged
    {
        string Filter { get; set; }
        ICollection<IApplication> Applications { get; }
    }
}