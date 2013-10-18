using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using SteamLauncher.UI.Core;
using SteamLauncher.Domain;

namespace SteamLauncher.UI.ViewModels
{
    public interface IMainViewModel : IViewModel
    {
        IEnumerable<IFilteredApplicationCategory> ApplicationCategories { get; }
        string Filter { get; set; }
        void Launch(IApplication application);
    }
}