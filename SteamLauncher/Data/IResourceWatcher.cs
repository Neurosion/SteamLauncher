using System;

namespace SteamLauncher.Domain.Data
{
    public interface IResourceWatcher
    {
        event Action<int, string> ResourceAdded;
        event Action<int, string> ResourceRemoved;
        event Action<int, string> ResourceUpdated;
    }
}