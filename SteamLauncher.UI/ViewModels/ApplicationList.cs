using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using SteamLauncher.Domain;

namespace SteamLauncher.UI.ViewModels
{
    public class ApplicationList : IApplicationList
    {
        public IEnumerable<IApplication> Applications { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ApplicationList()
        {
            Applications = new ObservableCollection<IApplication>();
        }
    }
}