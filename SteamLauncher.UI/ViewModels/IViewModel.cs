﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;

namespace SteamLauncher.UI.ViewModels
{
    public interface IViewModel : INotifyPropertyChanged
    {
        string Title { get; }
        event Action<IViewModel> Closed;
    }
}