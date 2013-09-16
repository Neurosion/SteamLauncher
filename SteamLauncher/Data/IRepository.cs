using System.Collections.Generic;

namespace SteamLauncher.Domain.Data
{
    public interface IRepository<ItemType, IdType>
    {
        ItemType Get(IdType id);
        IEnumerable<ItemType> Get();
    }
}