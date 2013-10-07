using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain
{
    public class StringToIntIdConverter : IIdConverter
    {
        public int Convert(string source)
        {
            int id = 0;
            int.TryParse(source, out id);
            
            return id;
        }
    }
}
