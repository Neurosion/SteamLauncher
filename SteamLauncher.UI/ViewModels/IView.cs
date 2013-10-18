using System;
using System.Linq;
using SteamLauncher.UI.ViewModels;

namespace SteamLauncher.UI.ViewModels
{
    public interface IView<T>
    {
        T ViewModel { get; }
        void Show();
        void Hide();
    }
}