using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain
{
    public interface IIdentifiable<T>
    {
        T Id { get; set; }
    }
}