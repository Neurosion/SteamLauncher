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
        private bool _isVisible;
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

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    PropertyChanged.Notify();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(ISteamProxy steamProxy, IFilteredApplicationCategoryFactory applicationCategoryFactory, IHotKey hotKey, INotifyIcon notifyIcon)
        {
            _steamProxy = steamProxy;
            ApplicationCategories = applicationCategoryFactory.Build();
            _hotKey = hotKey;
            //_hotKey.Enable();
            notifyIcon.ItemSelected += n => IsVisible = n.Equals(NotifyIconActions.ShowMainUI.GetDescription(), 
                                                                StringComparison.OrdinalIgnoreCase);
            Reset();
        }

        public void Launch(IApplication application)
        {
            if (application != null)
                _steamProxy.LaunchApp(application.Id);
        }

        public void Close()
        {
            IsVisible = false;
            Reset();
        }

        private void Reset()
        {
            Filter = string.Empty;
        }
    }
}