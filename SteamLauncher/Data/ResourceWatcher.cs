using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public abstract class ResourceWatcher<IdType> : IResourceWatcher<IdType>
    {
        private string _path;
        private string _filter;
        private FileSystemWatcher _watcher;

        public event Action<IdType, string> ResourceAdded = delegate { };
        public event Action<IdType, string> ResourceRemoved = delegate { };
        public event Action<IdType, string> ResourceUpdated = delegate { };

        public ResourceWatcher(string path, string filter = null)
        {
            if (!Directory.Exists(path))
                throw new ArgumentException(string.Format("The path {0} does not exist.", path ?? string.Empty));

            _path = path;
            _filter = !string.IsNullOrEmpty(filter)
                        ? filter
                        : "*.*";

            _watcher = new FileSystemWatcher(_path, _filter)
                {
                    EnableRaisingEvents = true
                };
            _watcher.Created += (s, e) => ResourceAdded(GetIdFromPath(e.FullPath), e.FullPath);
            _watcher.Deleted += (s, e) => ResourceRemoved(GetIdFromPath(e.FullPath), e.FullPath);
            _watcher.Changed += (s, e) => ResourceUpdated(GetIdFromPath(e.FullPath), e.FullPath);
            _watcher.Renamed += (s, e) =>
                {
                    ResourceRemoved(GetIdFromPath(e.OldFullPath), e.OldFullPath);
                    ResourceAdded(GetIdFromPath(e.FullPath), e.FullPath);
                };
        }

        protected abstract IdType GetIdFromPath(string filePath);
    }
}