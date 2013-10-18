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
            try
            {
                _diConfiguration = new DependencyInjectionConfiguration();
                _model = _diConfiguration.Container.Resolve<IApplicationModel>();
                Startup += (s, e) => _model.Start();
                _model.Exited += () => Shutdown();

                //DispatcherUnhandledException += (s, e) => _errorHandler.Handle(e);
            }
            catch (Exception ex)
            {
                // handle here
                // _errorHandler.Handle(ex);
            }
        }
    }
}