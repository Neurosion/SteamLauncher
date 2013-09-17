using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace SteamLauncher.Domain.Data
{
    public class InstalledApplicationRepository : IApplicationRepository
    {
        private IConfigurationRepository _configurationRepository;
        private List<IApplication> _applications;
        private FileSystemWatcher _watcher;

        // TODO: this needs to be put into a catagory importer or something simliar
        // Note: will need to get previously set application categories from "Steam\userdata\<online user id>\7\remote\sharedconfig.vdf"
        // tags located at: root.Software.Valve.Steam.apps.<appid>.tags
        // tag format: "index" "CategoryName"
        
        // Note: application icons located at Steam\steam\games\
        // files are named with a special id
        // the id for an application's icon can be pulled from Steam\appcache\appinfo.vdf
        
        private const char NUL = (char)0x00;
        private const char SOH = (char)0x01;
        private readonly Regex ApplicationIconIdRegex = new Regex(string.Format("name{0}*.{1}clienticon{0}*.{1}", NUL, SOH));

        // TODO: need to enforce the configuration repository to provide only application settings
        public InstalledApplicationRepository(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
            _applications = new List<IApplication>();
            _watcher = new FileSystemWatcher(_applicationsPath, string.Format("*.{0}", _fileExtension));

            _watcher.Created += (s, e) => AddApplication(e.FullPath);
            _watcher.Deleted += (s, e) => RemoveApplication(e.FullPath);
            _watcher.Renamed += (s, e) => RenameApplication(e.OldFullPath, e.FullPath);
            _watcher.Changed += (s, e) => UpdateApplication(e.FullPath);
        }

        public void Save(IApplication item)
        {
            // Should only save categories
            throw new NotImplementedException();
        }

        public void Delete(IApplication item)
        {
            // Should only delete all categories
            throw new NotImplementedException();
        }

        public IApplication Get(int id)
        {
            var foundItem = _applications.Where(x => x.Id == id)
                                         .FirstOrDefault();
            return foundItem;
        }

        public IEnumerable<IApplication> Get()
        {
            return _applications;
        }

        private void AddApplication(string fileName)
        {
            RemoveApplication(fileName);
            var loadedApplication = LoadApplication(fileName);
            if (loadedApplication != null)
                _applications.Add(loadedApplication);
        }

        private void RemoveApplication(string fileName)
        {
            var id = GetIdFromFileName(fileName);
            var foundApplication = _applications.Where(x => x.Id == id).FirstOrDefault();
            if (foundApplication != null)
                _applications.Remove(foundApplication);
        }

        private void RenameApplication(string oldFileName, string newFileName)
        {
            RemoveApplication(oldFileName);
            AddApplication(newFileName);
        }

        private void UpdateApplication(string fileName)
        {
            var id = GetIdFromFileName(fileName);
            var foundApplication = _applications.Where(x => x.Id == id).FirstOrDefault();
            if (foundApplication != null)
                foundApplication = LoadApplication(fileName);
        }

        private int GetIdFromFileName(string fileName)
        {
            var bareName = Path.GetFileNameWithoutExtension(fileName);
            var splitName = bareName.Split(new [] { '_' });

            int id = 0;

            if (splitName.Length == 2)
                int.TryParse(splitName[1], out id);

            return id;
        }

        private IApplication LoadApplication(string fileName)
        {
            IApplication loadedApplication = null;
            var loadedConfiguration = _configurationRepository.Get(fileName);
            
            if (loadedConfiguration != null)
            {
                var userConfig = loadedConfiguration.Children.Where(x => x.Name == "UserConfig").FirstOrDefault();

                if (userConfig != null && 
                    userConfig.Attributes.ContainsKey("Installed") && userConfig.Attributes["Installed"] == "1" &&
                    userConfig.Attributes.ContainsKey("name") &&
                    userConfig.Attributes.ContainsKey("GameID"))
                        loadedApplication = new Application()
                        {
                            Id = int.Parse(userConfig.Attributes["GameID"]),
                            Name = userConfig.Attributes["name"]
                        };
            }

            return loadedApplication;
        }
    }
}