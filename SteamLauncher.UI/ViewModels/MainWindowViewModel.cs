using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using SteamLauncher.UI.Core;
using SteamLauncher.Domain;
using SteamLauncher.Domain.Data;
using SteamLauncher.Domain.Input;

namespace SteamLauncher.UI.ViewModels
{
    public class MainWindowViewModel : IMainViewModel
    {
        private string _filter;
        private ISteamProxy _steamProxy;
        private IHotKey _hotKey;

        public IEnumerable<IFilteredApplicationCategory> ApplicationCategories { get; private set; }

        public string Title
        {
            get { return "Application List"; }
        }

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

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action<IViewModel> Closed = delegate { };

        public MainWindowViewModel(ISteamProxy steamProxy, IFilteredApplicationCategoryFactory applicationCategoryFactory, IHotKey hotKey)
        {
            _steamProxy = steamProxy;
            ApplicationCategories = applicationCategoryFactory.Build();
            _hotKey = hotKey;
            //_hotKey.Enable();

            _filter = string.Empty;
        }

        public void Launch(IApplication application)
        {
            if (application != null)
            {
                _steamProxy.LaunchApp(application.Id);
                Closed(this);
            }
        }
    }
}