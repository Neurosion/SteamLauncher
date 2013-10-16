using System;
using System.Windows.Forms;

namespace SteamLauncher.Domain.Input
{
    public interface IHookListener
    {
        int HookId { get; }
        IntPtr HookPointer { get; set; }
        void HandleHookMessage(Keys keys);
    }
}