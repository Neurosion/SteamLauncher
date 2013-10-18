using System.ComponentModel;

namespace SteamLauncher.UI.Core
{
    public enum NotifyIconActions
    {
        [Description("Show")]
        ShowMainUI,
        [Description("Settings")]
        ShowSettingsUI,
        [Description("Exit")]
        ExitApplication
    }
}