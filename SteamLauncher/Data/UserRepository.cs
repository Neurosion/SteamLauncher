using System;
using System.Collections.Generic;
using System.Linq;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public class UserRepository : WatchedConfigurationBasedElementRepository<IUser, int>, IUserRepository
    {
        // note: data located at "Steam\userdata\<user id>\config\localconfig.vdf"
        // localconfig.vdf user name location: root.friends.PersonaName (attribute)

        // TODO: for reader that reads user profile info, need to inject configuration line that tells if the user is logged in

        public UserRepository(IWatchingConfigurationRepository configurationRepository)
            : base(configurationRepository)
        {
        }

        protected override IUser Load(IConfigurationElement configuration)
        {
            IUser loadedUser = null;

            if (configuration != null)
            {
                var friends = configuration.Children.Where(x => x.Name == "friends").FirstOrDefault();

                if (friends != null && friends.Attributes.ContainsKey("PersonaName"))
                {
                    var profileName = friends.Attributes["PersonaName"];
                    var profileDetail = friends.Children.Where(x => x.Attributes.ContainsKey("name") && x.Attributes["name"] == profileName)
                                                        .FirstOrDefault();

                    if (profileDetail != null)
                    {
                        bool isLoggedIn = false;

                        if (configuration.Attributes.ContainsKey("IsLoggedIn"))
                            bool.TryParse(configuration.Attributes["IsLoggedIn"], out isLoggedIn);

                        loadedUser = new User()
                        {
                            Id = int.Parse(profileDetail.Name),
                            Name = profileName,
                            IsLoggedIn = isLoggedIn
                        };
                    }
                }
            }

            return loadedUser;
        }
    }
}