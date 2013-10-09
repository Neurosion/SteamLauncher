using System.Windows.Input;
using SteamLauncher.UI.ViewModels;

namespace SteamLauncher.UI.Views
{
    public interface IMainWindow
    {
        IMainWindowViewModel ViewModel { get; }
        void Show();
    }
}