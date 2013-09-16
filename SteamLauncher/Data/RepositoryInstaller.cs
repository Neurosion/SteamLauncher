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
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConfigurationRepository>()
                                        .ImplementedBy<ApplicationConfigurationRepository>());

            container.Register(Component.For<IApplicationRepository>()
                                        .ImplementedBy<InstalledApplicationRepository>());

            container.Register(Component.For<IUserRepository>()
                                        .ImplementedBy<UserRepository>());
        }
    }
}