using System;
using System.Collections.Generic;
using System.Linq;

namespace SteamLauncher.UI.Models
{
    public interface IApplicationModel
    {
        //event Action Starting;
        event Action Started;
        //event Action Exiting;
        event Action Exited;
        void Start();
        void Exit();
    }
}