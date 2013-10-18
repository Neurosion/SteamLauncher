using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain.ErrorHandling
{
    public abstract class ErrorHandlerBase : IErrorHandler
    {
        public abstract bool Handle(Exception ex);
        public abstract bool Handle(string message);

        protected bool TryHandle(Action handler)
        {
            bool wasHandled = false;

            try
            {
                handler();
                wasHandled = true;
            }
            catch { }

            return wasHandled;
        }
    }
}
