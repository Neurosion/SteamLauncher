using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using SteamLauncher.Domain.Configuration;
using SteamLauncher.UI.Views;

namespace SteamLauncher.UI
{
    public partial class App : Application
    {
        private DependencyInjectionConfiguration _diConfiguration;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _diConfiguration = new DependencyInjectionConfiguration();
            _diConfiguration.Container
                            .Resolve<IMainWindow>()
                            .Show();
        }
    }
}