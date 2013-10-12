using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain.Input
{
    public interface IHookRegistrationController
    {
        event Action HookTriggered;
        int HookId { get; set; }
    }
}