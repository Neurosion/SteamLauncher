using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.UI.Views
{
    public interface IErrorDialogView
    {
        string Title { get; }
        string ErrorMessage { get; }
        void Show(string errorMessage);
    }
}