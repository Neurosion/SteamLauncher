using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace SteamLauncher.UI.Core
{
    public interface INotifyIcon
    {
        bool IsVisible { get; set; }
        IList<string> Items { get; }
        event Action<string> ItemSelected;
    }
}