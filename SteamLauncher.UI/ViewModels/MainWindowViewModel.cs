using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SteamLauncher.UI.Core;
using SteamLauncher.Domain;
using SteamLauncher.Domain.Data;
using SteamLauncher.Domain.Input;

namespace SteamLauncher.UI.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        private string _filter;
        private bool _isVisible;
        private ISteamProxy _steamProxy;
        private IHotKey _hotKey;

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

        public bool IsVisible
        {
            get { return _isVisible; }
            private set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    PropertyChanged.Notify();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(ISteamProxy steamProxy, IFilteredApplicationCategoryFactory applicationCategoryFactory, IHotKey hotKey)
        {
            _steamProxy = steamProxy;
            ApplicationCategories = applicationCategoryFactory.Build();
            _hotKey = hotKey;
            _filter = string.Empty;
        }

        public void Launch(IApplication application)
        {
            if (application != null)
                _steamProxy.LaunchApp(application.Id);
        }
    }
}