﻿using System;
using System.Linq;
using Microsoft.Win32;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace SteamLauncher.Domain
{
    public abstract class InstallerBase : IWindsorInstaller
    {
        protected string SteamPath { get; private set; }

        public InstallerBase()
        {
            SteamPath = ResolveSteamPath();
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