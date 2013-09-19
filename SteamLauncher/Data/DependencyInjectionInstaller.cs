using System;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public class DependencyInjectionInstaller : IWindsorInstaller
    {
        private const string ApplicationsPath = "steamapps"; // TODO: need to get the base steam path here
        private const string ApplicationConfigurationFileExtension = "acf";
        private const string ApplicationConfigurationFileFilter = "*." + ApplicationConfigurationFileExtension;

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConfigurationResourceLocator>()
                                        .ImplementedBy<ConfigurationResourceLocator>()
                                        .DependsOn(Dependency.OnValue("directory", ApplicationsPath),
                                                   Dependency.OnValue("fileExtension", ApplicationConfigurationFileExtension))
                                        .Named("ApplicationConfigurationResourceLocator"));

            container.Register(Component.For<IConfigurationResourceWatcher>()
                                        .ImplementedBy<ConfigurationResourceWatcher>()
                                        .DependsOn(Dependency.OnValue("path", ApplicationsPath),
                                                   Dependency.OnValue("filter", ApplicationConfigurationFileFilter))
                                        .Named("ApplicationConfigurationResourceWatcher"));

            container.Register(Component.For<IWatchingConfigurationRepository>()
                                        .ImplementedBy<WatchingConfigurationRepository>()
                                        .DependsOn(Dependency.OnComponent(typeof(IConfigurationResourceLocator), "ApplicationConfigurationResourceLocator"),
                                                   Dependency.OnComponent(typeof(IConfigurationResourceWatcher), "ApplicationConfigurationResourceWatcher"))
                                        .Named("ApplicationConfigurationRepository"));

            container.Register(Component.For<IApplicationRepository>()
                                        .ImplementedBy<InstalledApplicationRepository>()
                                        .DependsOn(Dependency.OnComponent(typeof(IWatchingConfigurationRepository), "ApplicationConfigurationRepository")));

            container.Register(Component.For<IUserRepository>()
                                        .ImplementedBy<UserRepository>());


            container.Register(Component.For<IConfigurationReader>()
                                        .ImplementedBy<ConfigurationReader>());
        }
    }
}