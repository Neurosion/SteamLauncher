using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SteamLauncher.Domain.Data
{
    public class UserRepository : IUserRepository
    {
        private string _usersPath;

        // note: data located at "Steam\userdata\<user id>\config\localconfig.vdf"
        // localconfig.vdf user name location: root.friends.PersonaName (attribute)

        public UserRepository(string usersPath)
        {
            if (!Directory.Exists(usersPath))
                throw new ArgumentException(string.Format("The path {0} does not exist.", usersPath));

            _usersPath = usersPath;
        }

        public IUser Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUser> Get()
        {
            throw new NotImplementedException();
        }
    }
}
