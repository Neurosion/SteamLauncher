using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SteamLauncher.Domain.Input
{
    public delegate void HookMessageHandler(Keys keys);

    public interface IHookRegistrationController
    {
        void Register(IHookListener listener);
        void Unregister(IHookListener listener);
    }
}