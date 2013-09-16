using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Registration;

namespace SteamLauncher.Domain.Configuration
{
    public class DependencyInjectionConfiguration
    {
        private IWindsorContainer _container;
        
        public DependencyInjectionConfiguration()
        {
            var assemblyPath = Path.GetDirectoryName(this.GetType().Assembly.CodeBase).Replace(@"file:\", "");

            _container = new WindsorContainer();
            _container.Install(FromAssembly.InDirectory(new AssemblyFilter(assemblyPath, "SteamLauncher*")));
        }
    }
}