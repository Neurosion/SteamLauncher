using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SteamLauncher.Domain;

namespace SteamLauncher.UI.Core
{
    public interface IFilteredApplicationCategory
    {
        string Name { get; }
        string Filter { get; set; }
        IEnumerable<IApplication> Applications { get; }
    }
}