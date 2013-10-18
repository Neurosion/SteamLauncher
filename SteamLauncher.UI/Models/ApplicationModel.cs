using System;
using System.Collections.Generic;
using System.Linq;
using SteamLauncher.Domain;
using SteamLauncher.UI.Core;
using SteamLauncher.UI.Views;

namespace SteamLauncher.UI.Models
{
    public class ApplicationModel : IApplicationModel
    {
        private INotifyIcon _notifyIcon;
        private Dictionary<string, Action> _notifyItemSelectedActions;

        public event Action Started = delegate { };
        public event Action Exited = delegate { };

        public ApplicationModel(INotifyIcon notifyIcon, IMainView mainView, ISettingsView settingsView)
        {
            _notifyItemSelectedActions = new Dictionary<string, Action>()
            {
                { NotifyIconActions.ShowMainUI.GetDescription(), () => mainView.Show() },
                { NotifyIconActions.ShowSettingsUI.GetDescription(), () => settingsView.Show() },
                { NotifyIconActions.ExitApplication.GetDescription(), () => Exit() }
            };

            _notifyIcon = notifyIcon;
            _notifyIcon.ItemSelected += HandleNotifyIconItemSelected;
        }

        private void HandleNotifyIconItemSelected(string itemName)
        {
            if (_notifyItemSelectedActions.ContainsKey(itemName))
                _notifyItemSelectedActions[itemName]();
        }

        public void Start()
        {
            _notifyIcon.IsVisible = true;
            
            Started();
        }

        public void Exit()
        {
            _notifyIcon.IsVisible = false;
            
            Exited();
        }
    }
}