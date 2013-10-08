using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.UI.Core
{
    public interface IFilteredApplicationCategoryFactory
    {
        IEnumerable<IFilteredApplicationCategory> Build();
        IFilteredApplicationCategory Build(string name);
    }
}