﻿using System;
using System.IO;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public class DataDependencyInjectionInstaller : DependencyInjectionInstallerBase
    {
        private readonly string ApplicationsPath;
        private const string ApplicationConfigurationFileExtension = "acf";
        private const string ApplicationConfigurationFileFilter = "*." + ApplicationConfigurationFileExtension;

        public DataDependencyInjectionInstaller()
        {
            ApplicationsPath = Path.Combine(SteamDirectory, "steamapps");
        }

        public override void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                    Component.For<IConfigurationResourceLocator>()
                             .ImplementedBy<ConfigurationResourceLocator>()
                             .DependsOn(Dependency.OnValue("directory", ApplicationsPath),
                                        Dependency.OnValue("fileExtension", ApplicationConfigurationFileExtension))
                             .Named("ApplicationConfigurationResourceLocator"),

                    Component.For<IConfigurationResourceWatcher>()
                             .ImplementedBy<ConfigurationResourceWatcher>()
                             .DependsOn(Dependency.OnValue("path", ApplicationsPath),
                                        Dependency.OnValue("filter", ApplicationConfigurationFileFilter))
                             .Named("ApplicationConfigurationResourceWatcher"),

                    Component.For<IWatchingConfigurationRepository>()
                             .ImplementedBy<WatchingConfigurationRepository>()
                             .DependsOn(Dependency.OnComponent(typeof(IConfigurationResourceLocator), "ApplicationConfigurationResourceLocator"),
                                        Dependency.OnComponent(typeof(IConfigurationResourceWatcher), "ApplicationConfigurationResourceWatcher"))
                             .Named("ApplicationConfigurationRepository"),

                    Component.For<IApplicationRepository>()
                             .ImplementedBy<InstalledApplicationRepository>()
                             .DependsOn(Dependency.OnComponent(typeof(IWatchingConfigurationRepository), "ApplicationConfigurationRepository")),

                    Component.For<IUserRepository>()
                             .ImplementedBy<UserRepository>()
                             );
        }
    }
}