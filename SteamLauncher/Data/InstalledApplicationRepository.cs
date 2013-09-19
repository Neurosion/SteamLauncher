using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public class InstalledApplicationRepository : WatchedConfigurationBasedElementRepository<IApplication, int>, IApplicationRepository
    {
        /*
        // TODO: this needs to be put into a catagory importer or something simliar
        // Note: will need to get previously set application categories from "Steam\userdata\<online user id>\7\remote\sharedconfig.vdf"
        // tags located at: root.Software.Valve.Steam.apps.<appid>.tags
        // tag format: "index" "CategoryName"
        // Note: can save our own categories in the sharedconfig.vdf file as a new collection and Steam will persist it to the cloud
        
        // Note: application icons located at Steam\steam\games\
        // files are named with a special id
        // the id for an application's icon can be pulled from Steam\appcache\appinfo.vdf
        
        private const char NUL = (char)0x00;
        private const char SOH = (char)0x01;
        private readonly Regex ApplicationIconIdRegex = new Regex(string.Format("name{0}.*{1}clienticon{0}.*{1}", NUL, SOH));  
        */        
        
        public InstalledApplicationRepository(IWatchingConfigurationRepository configurationRepository) //, IconPathResolver, CategoryLoader)
            :base(configurationRepository)
        {
        }

        //public void Save(IApplication item)
        //{
        //    // Should only save categories
        //    throw new NotImplementedException();
        //}

        //public void Delete(IApplication item)
        //{
        //    // Should only delete all categories
        //    throw new NotImplementedException();
        //}
                
        protected override IApplication Load(IConfigurationElement configuration)
        {
            IApplication loadedApplication = null;

            if (configuration != null)
            {
                var userConfig = configuration.Children.Where(x => x.Name == "UserConfig").FirstOrDefault();
                
                if (userConfig != null && 
                    userConfig.Attributes.Keys.ContainsAll("Installed", "name", "GameID") &&
                    userConfig.Attributes["Installed"] == "1")
                {
                    loadedApplication = new Application()
                    {
                        Id = int.Parse(userConfig.Attributes["GameID"]),
                        Name = userConfig.Attributes["name"]
                    };
                }
            }

            return loadedApplication;
        }
    }
}