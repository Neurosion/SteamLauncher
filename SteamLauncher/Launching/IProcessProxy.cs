using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain
{
    public interface IProcessProxy
    {
        void Start(string fileName);
        void Start(string fileName, string arguments);
    }
}
