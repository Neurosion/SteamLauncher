using System;

namespace SteamLauncher.Domain.Input
{
    [Flags]
    public enum ModifierKeys
    {
        None = 0x0,
        Alt = 0x1,
        Control = 0x2,
        Shift = 0x4,
        Windows = 0x8
    }
}