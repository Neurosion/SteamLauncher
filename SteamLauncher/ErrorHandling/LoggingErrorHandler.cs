using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SteamLauncher.Domain.ErrorHandling
{
    public class LoggingErrorHandler : ErrorHandlerBase
    {
        private string _directoryPath;
        private string _filePath;

        public LoggingErrorHandler(string path, string fileName)
        {
            _directoryPath = path;
            _filePath = Path.Combine(path, fileName);
        }

        public override bool Handle(Exception ex)
        {
            bool wasHandled = TryHandle(() => WriteError(ex.ToString()));
            return wasHandled;
        }

        public override bool Handle(string message)
        {
            bool wasHandled = TryHandle(() => WriteError(message));
            return wasHandled;
        }

        private void WriteError(string errorText)
        {
            if (!Directory.Exists(_directoryPath))
                Directory.CreateDirectory(_directoryPath);

            File.AppendAllText(_filePath, Environment.NewLine + errorText);
        }
    }
}