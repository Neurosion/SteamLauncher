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
using SteamLauncher.Domain.ErrorHandling;

namespace SteamLauncher.UI
{
    public partial class App : Application
    {
        private DependencyInjectionConfiguration _diConfiguration;
        private IApplicationModel _model;
        private IErrorHandler _errorHandler;

        public App()
        {
            _diConfiguration = new DependencyInjectionConfiguration();
            _errorHandler = _diConfiguration.Container.Resolve<IErrorHandler>();

            try
            {
                _model = _diConfiguration.Container.Resolve<IApplicationModel>();
                Startup += (s, e) => _model.Start();
                _model.Exited += () => Shutdown();

                DispatcherUnhandledException += (s, e) => e.Handled = _errorHandler.Handle(e.Exception);
            }
            catch (Exception ex)
            {
                if (_errorHandler == null)
                    throw;
                
                _errorHandler.Handle(ex);
            }
        }
    }
}