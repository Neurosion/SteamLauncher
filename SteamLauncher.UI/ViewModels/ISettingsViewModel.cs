using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;

namespace SteamLauncher.UI.ViewModels
{
    public interface ISettingsViewModel : IViewModel
    {
        string HotKeyString { get; set; }
        ICommand SaveCommand { get; }
        ICommand CancelCommand { get; }
    }
}