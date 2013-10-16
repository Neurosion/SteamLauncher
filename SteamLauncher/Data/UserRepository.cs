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

                if (friends != null && 
                    friends.Attributes.Any(x => x.Key.Equals("PersonaName", StringComparison.OrdinalIgnoreCase)))
                {
                    var profileName = friends.Attributes
                                             .Where(x => x.Key.Equals("PersonaName", StringComparison.OrdinalIgnoreCase))
                                             .Select(x => x.Value)
                                             .FirstOrDefault();
                    var profileDetail = friends.Children
                                               .Where(c => c.Attributes
                                                            .Any(a => a.Key.Equals("name", StringComparison.OrdinalIgnoreCase) && 
                                                                      a.Value == profileName))
                                               .FirstOrDefault();

                    if (profileDetail != null)
                    {
                        bool isLoggedIn = false;

                        var isLoggedInValue = configuration.Attributes
                                                           .Where(x => x.Key.Equals("IsLoggedIn", StringComparison.OrdinalIgnoreCase))
                                                           .Select(x => x.Value)
                                                           .FirstOrDefault();
                        
                        bool.TryParse(isLoggedInValue, out isLoggedIn);

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