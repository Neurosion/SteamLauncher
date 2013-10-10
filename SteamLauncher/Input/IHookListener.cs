﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain.Input
{
    public interface IHookListener
    {
        event Action HookTriggered;
        IntPtr HookId { get; set; }
    }
}