using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public abstract class ResourceWatcher : IResourceWatcher
    {
        private string _path;
        private string _filter;
        private FileSystemWatcher _watcher;
        private IIdConverter _idConverter;

        public event Action<int, string> ResourceAdded = delegate { };
        public event Action<int, string> ResourceRemoved = delegate { };
        public event Action<int, string> ResourceUpdated = delegate { };

        public ResourceWatcher(string path, IIdConverter idConverter, string filter = null)
        {
            if (!Directory.Exists(path))
                throw new ArgumentException(string.Format("The path {0} does not exist.", path ?? string.Empty));

            _path = path;
            _idConverter = idConverter;
            _filter = !string.IsNullOrEmpty(filter)
                        ? filter
                        : "*.*";

            _watcher = new FileSystemWatcher(_path, _filter)
                {
                    EnableRaisingEvents = true
                };
            _watcher.Created += (s, e) => ResourceAdded(_idConverter.Convert(e.FullPath), Path.GetFileName(e.FullPath));
            _watcher.Deleted += (s, e) => ResourceRemoved(_idConverter.Convert(e.FullPath), Path.GetFileName(e.FullPath));
            _watcher.Changed += (s, e) => ResourceUpdated(_idConverter.Convert(e.FullPath), Path.GetFileName(e.FullPath));
            _watcher.Renamed += (s, e) =>
                {
                    ResourceRemoved(_idConverter.Convert(e.OldFullPath), Path.GetFileName(e.OldFullPath));
                    ResourceAdded(_idConverter.Convert(e.FullPath), Path.GetFileName(e.FullPath));
                };
        }
    }
}