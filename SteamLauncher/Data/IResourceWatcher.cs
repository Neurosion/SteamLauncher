using System;

namespace SteamLauncher.Domain.Data
{
    public interface IResourceWatcher<IdType>
    {
        event Action<IdType, string> ResourceAdded;
        event Action<IdType, string> ResourceRemoved;
        event Action<IdType, string> ResourceUpdated;
    }
}