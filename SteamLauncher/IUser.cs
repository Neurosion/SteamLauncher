using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain
{
    public interface IUser
    {
        int Id { get; }
        string Name { get; }
        bool IsLoggedIn { get; }
    }
}
