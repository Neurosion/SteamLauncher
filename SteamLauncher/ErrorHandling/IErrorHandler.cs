using System;
using System.Collections.Generic;
using System.Linq;

namespace SteamLauncher.Domain.ErrorHandling
{
    public interface IErrorHandler
    {
        bool Handle(Exception ex);
        bool Handle(string message);
    }
}