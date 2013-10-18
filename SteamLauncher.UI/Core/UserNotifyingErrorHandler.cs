using System;
using System.Collections.Generic;
using System.Linq;
using SteamLauncher.Domain.ErrorHandling;
using SteamLauncher.UI.Views;

namespace SteamLauncher.UI.Core
{
    public class UserNotifyingErrorHandler : DelegatingErrorHandler
    {
        public UserNotifyingErrorHandler(IErrorDialogView errorDialogView)
            : base(message => errorDialogView.Show(message))
        {
        }
    }
}