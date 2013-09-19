using System;
using System.Collections.Generic;
using System.Linq;

namespace SteamLauncher.Domain
{
    public class User : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsLoggedIn { get; set; }

        public void Copy(IUser target)
        {
            if (target == null)
                throw new ArgumentNullException();

            Id = target.Id;
            Name = target.Name;
            IsLoggedIn = target.IsLoggedIn;
        }
    }
}
