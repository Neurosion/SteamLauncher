using System;
using System.Collections.Generic;

namespace SteamLauncher.Domain.Data
{
    public interface IResourceLocator<T>
    {
        IEnumerable<T> Locate(string name);
    }
}