using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using SteamLauncher.UI.Views;
using SteamLauncher.UI.ViewModels;
using SteamLauncher.UI.Models;
using SteamLauncher.UI.Core;
using SteamLauncher.Domain;
using SteamLauncher.Domain.ErrorHandling;

namespace SteamLauncher.UI
{
    public class UIDependencyInjectionInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IFilteredApplicationCategoryFactory>()
                         .ImplementedBy<FilteredApplicationCategoryFactory>(),

                Component.For<IMainViewModel>()
                         .ImplementedBy<MainWindowViewModel>(),

                Component.For<IMainView>()
                         .ImplementedBy<MainWindow>(),

                Component.For<ISettingsViewModel>()
                         .ImplementedBy<SettingsViewModel>(),

                Component.For<ISettingsView>()
                         .ImplementedBy<SettingsWindow>(),

                Component.For<IErrorDialogView>()
                         .ImplementedBy<ErrorDialog>(),

                Component.For<INotifyIcon>()
                         .ImplementedBy<NotifyIconWrapper>()
                         .DependsOn(Dependency.OnResource<SteamLauncher.UI.Properties.Resources>("icon", "NotificationIcon"))
                         .OnCreate(new Action<INotifyIcon>(n => Enum.GetValues(typeof(NotifyIconActions))
                                                                    .Cast<NotifyIconActions>()
                                                                    .ForEach(a => n.Items.Add(a.GetDescription())))),
                Component.For<IApplicationModel>()
                         .ImplementedBy<ApplicationModel>(),

                Component.For<IErrorHandler>()
                         .UsingFactoryMethod((k, c) => new CompositeErrorHandler(new LoggingErrorHandler(Dependency.OnAppSettingsValue("Log.Path").Value, 
                                                                                                         Dependency.OnAppSettingsValue("Log.File").Value),
                                                                                 new UserNotifyingErrorHandler(k.Resolve<IErrorDialogView>())))
                              );
        }
    }
}