using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace SteamLauncher.Domain
{
    public class DomainDependencyInjectionInstaller : DependencyInjectionInstallerBase
    {
        public override void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IProcessProxy>()
                                        .ImplementedBy<ProcessProxy>());

            container.Register(Component.For<ISteamProxy>()
                                        .ImplementedBy<CommandLineSteamProxy>()
                                        .DependsOn(Dependency.OnValue("steamPath", SteamPath)));

            container.Register(Component.For<IIdConverter>()
                                        .ImplementedBy<PathToIntIdConverter>());
        }
    }
}