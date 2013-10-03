using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Data
{
    public interface IConfigurationRepository : IRepository<IConfigurationElement, int>
    {
    }
}