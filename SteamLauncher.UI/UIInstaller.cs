using System.Windows;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using SteamLauncher.UI.Views;
using SteamLauncher.UI.ViewModels;
using SteamLauncher.UI.Core;

namespace SteamLauncher.UI
{
    public class UIInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IFilteredApplicationCategoryFactory>()
                                        .ImplementedBy<FilteredApplicationCategoryFactory>());

            container.Register(Component.For<IMainWindowViewModel>()
                                        .ImplementedBy<MainWindowViewModel>());

            container.Register(Component.For<IMainWindow>()
                                        .ImplementedBy<MainWindow>());
        }
    }
}