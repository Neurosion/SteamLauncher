using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace SteamLauncher.Domain
{
    public class DomainInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ISteamProxy>()
                                        .ImplementedBy<CommandLineSteamProxy>()
                                        .DependsOn(Dependency.OnValue("steamPath", ResolveSteamPath())));
        }

        private string ResolveSteamPath()
        {
            var foundPath = GetRegistryValue(new[] { "Software", "Valve", "Steam", "SteamExe" });
            return foundPath;
        }

        private string GetRegistryValue(string[] keyNames)
        {
            return GetRegistryValue(Registry.CurrentUser.OpenSubKey(keyNames[0]), keyNames);
        }

        private string GetRegistryValue(RegistryKey currentKey, string[] keyNames)
        {
            if (currentKey != null)
                return keyNames.Length == 2
                            ? currentKey.GetValue(keyNames[1], "") as string
                            : GetRegistryValue(currentKey.OpenSubKey(keyNames[1]), keyNames.Skip(1).ToArray());

            return "";
        }
    }
}