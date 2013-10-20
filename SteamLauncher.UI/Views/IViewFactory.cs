using System;
using SteamLauncher.UI.Views;

namespace SteamLauncher.UI.Views
{
    public interface IViewFactory<ViewType, ViewModelType>
        where ViewType: IView<ViewModelType>
    {
        ViewType Build();
    }
}