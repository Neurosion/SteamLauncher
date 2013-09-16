using System;
using System.Collections.Generic;
using System.Linq;

namespace SteamLauncher.Domain
{
    public class User : IUser
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsLoggedIn { get; private set; }

        public User(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
