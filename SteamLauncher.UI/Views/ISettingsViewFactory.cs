using System;
using SteamLauncher.UI.ViewModels;

namespace SteamLauncher.UI.Views
{
    public interface ISettingsViewFactory : IViewFactory<ISettingsView, ISettingsViewModel>
    {
    }
}