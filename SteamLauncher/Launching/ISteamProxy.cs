using System;

namespace SteamLauncher.Domain
{
    public interface ISteamProxy
    {
        bool IsSilent { get; set; }
        string BasePath { get; }
        string UsersPath { get; }
        string ApplicationsPath { get; }
        void LaunchApp(int id);
        void LaunchApp(int id, params string[] applicationParameters);
    }
}