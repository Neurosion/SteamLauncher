using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using SteamLauncher.Domain.Configuration;
using SteamLauncher.UI.Views;
using SteamLauncher.UI.Core;
using SteamLauncher.UI.Models;

namespace SteamLauncher.UI
{
    public partial class App : Application
    {
        private DependencyInjectionConfiguration _diConfiguration;
        private IApplicationModel _model;

        public App()
        {
            _diConfiguration = new DependencyInjectionConfiguration();
            _model = _diConfiguration.Container.Resolve<IApplicationModel>();
            _model.Exited += () => this.Shutdown();
            Startup += (s, e) => _model.Start();
        }
    }
}