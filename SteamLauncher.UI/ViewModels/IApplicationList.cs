using System;
using System.Collections.Generic;
using System.ComponentModel;
using SteamLauncher.Domain;

namespace SteamLauncher.UI.ViewModels
{
    public interface IApplicationList : INotifyPropertyChanged
    {
        IEnumerable<IApplication> Applications { get; }
    }
}