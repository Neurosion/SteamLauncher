using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain.ErrorHandling
{
    public class CompositeErrorHandler : ErrorHandlerBase
    {
        private List<IErrorHandler> _errorHandlers;

        public CompositeErrorHandler(params IErrorHandler[] errorHandlers)
        {
            _errorHandlers = new List<IErrorHandler>(errorHandlers);
        }

        public override bool Handle(Exception ex)
        {
            var wasSuccess = TryHandle(() => _errorHandlers.ForEach(x => x.Handle(ex)));
            return wasSuccess;
        }

        public override bool Handle(string message)
        {
            var wasSuccess = TryHandle(() => _errorHandlers.ForEach(x => x.Handle(message)));
            return wasSuccess;
        }
    }
}