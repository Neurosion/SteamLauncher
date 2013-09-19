using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain
{
    public interface IUser : IIdentifiable<int>, ICopyable<IUser>
    {
        string Name { get; set; }
        bool IsLoggedIn { get; set; }
    }
}
