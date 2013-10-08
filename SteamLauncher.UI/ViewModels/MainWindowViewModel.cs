using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SteamLauncher.UI.Core;
using SteamLauncher.Domain;
using SteamLauncher.Domain.Data;

namespace SteamLauncher.UI.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        private string _filter;

        public IEnumerable<IFilteredApplicationCategory> ApplicationCategories { get; private set; }

        public string Filter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value ?? string.Empty;
                    PropertyChanged.Notify();
                    ApplicationCategories.ForEach(x => x.Filter = Filter);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(IFilteredApplicationCategoryFactory applicationCategoryFactory)
        {
            ApplicationCategories = applicationCategoryFactory.Build();
            _filter = string.Empty;
        }
    }
}