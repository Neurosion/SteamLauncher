using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace SteamLauncher.Domain.Data
{
    public class RepositoryInstaller : IWindsorInstaller
    {
        private const string ApplicationsPath = "steamapps";
        private const string ApplicationConfigurationFileExtension = "acf";

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConfigurationRepository>()
                                        .ImplementedBy<ConfigurationRepository>()
                                        .DependsOn(Dependency.OnValue("configurationPath", ApplicationsPath), 
                                                   Dependency.OnValue("configurationFileExtension", ApplicationConfigurationFileExtension)));

            container.Register(Component.For<IApplicationRepository>()
                                        .ImplementedBy<InstalledApplicationRepository>()
                                        .DependsOn(Dependency.OnValue("applicationsPath", ApplicationsPath),
                                                   Dependency.OnValue("fileExtension", ApplicationConfigurationFileExtension)));

            container.Register(Component.For<IUserRepository>()
                                        .ImplementedBy<UserRepository>());
        }
    }
}