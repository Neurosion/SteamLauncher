using System;
using System.Windows.Forms;

namespace SteamLauncher.Domain.Input
{
    public interface IHotKey : IHookListener
    {
        int Id { get; }
        ModifierKeys Modifiers { get; set; }
        Keys Key { get; set; }
        bool IsEnabled { get; }
        void Enable();
        void Disable();
    }
}