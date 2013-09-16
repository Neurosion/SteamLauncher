using System;
using System.Windows.Forms;

namespace SteamLauncher.Domain.Input
{
    public interface IHotKeyRegistrationController
    {
        int Register(IHotKey hotKey);
        bool Unregister(IHotKey hotKey);
    }
}