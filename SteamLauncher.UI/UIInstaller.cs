using System.Windows;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using SteamLauncher.UI.Views;
using SteamLauncher.UI.ViewModels;

namespace SteamLauncher.UI
{
    public class UIInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IFilteredApplicationList>()
                                        .ImplementedBy<FilteredApplicationList>());

            container.Register(Component.For<IMainWindow>()
                                        .ImplementedBy<MainWindow>());
        }
    }
}