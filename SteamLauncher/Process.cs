namespace SteamLauncher.Domain
{
    public class Process : IProcessProxy
    {
        public void Start(string fileName)
        {
            System.Diagnostics.Process.Start(fileName);
        }

        public void Start(string fileName, string arguments)
        {
            System.Diagnostics.Process.Start(fileName, arguments);
        }
    }
}