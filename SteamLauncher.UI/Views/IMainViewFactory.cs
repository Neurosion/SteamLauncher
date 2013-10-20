using System;
using SteamLauncher.UI.ViewModels;

namespace SteamLauncher.UI.Views
{
    public interface IMainViewFactory : IViewFactory<IMainView, IMainViewModel>
    {
    }
}