using System.Diagnostics;

namespace SteamLauncher.Domain
{
    public class ProcessProxy : IProcessProxy
    {
        public void Start(string fileName)
        {
            Process.Start(fileName);
        }

        public void Start(string fileName, string arguments)
        {
            Process.Start(fileName, arguments);
        }
    }
}