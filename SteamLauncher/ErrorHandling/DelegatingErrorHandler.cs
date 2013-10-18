using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain.ErrorHandling
{
    public class DelegatingErrorHandler : ErrorHandlerBase
    {
        private Action<string> _notificationHandler;

        public DelegatingErrorHandler(Action<string> notificationHandler)
        {
            _notificationHandler = notificationHandler;
        }

        public override bool Handle(Exception ex)
        {
            var wasHandled = TryHandle(() => _notificationHandler(ex.Message));
            return wasHandled;
        }

        public override bool Handle(string message)
        {
            var wasHandled = TryHandle(() => _notificationHandler(message));
            return wasHandled;
        }
    }
}