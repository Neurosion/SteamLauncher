using System;
using System.Linq;
using System.IO;
using Microsoft.Win32;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace SteamLauncher.Domain
{
    public abstract class DependencyInjectionInstallerBase : IWindsorInstaller
    {
        protected string SteamPath { get; private set; }
        protected string SteamDirectory { get; private set; }

        public DependencyInjectionInstallerBase()
        {
            SteamPath = ResolveSteamPath();
            SteamDirectory = Path.GetDirectoryName(SteamPath);
        }

        public abstract void Install(IWindsorContainer container, IConfigurationStore store);

        private string ResolveSteamPath()
        {
            var foundPath = GetRegistryValue(new[] { "Software", "Valve", "Steam", "SteamExe" });
            return foundPath;
        }

        protected string GetRegistryValue(string[] keyNames)
        {
            return GetRegistryValue(Registry.CurrentUser.OpenSubKey(keyNames[0]), keyNames);
        }

        protected string GetRegistryValue(RegistryKey currentKey, string[] keyNames)
        {
            if (currentKey != null)
                return keyNames.Length == 2
                            ? currentKey.GetValue(keyNames[1], "") as string
                            : GetRegistryValue(currentKey.OpenSubKey(keyNames[1]), keyNames.Skip(1).ToArray());

            return "";
        }
    }
}