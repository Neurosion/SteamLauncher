using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SteamLauncher.Domain
{
    public class StringToIntIdConverter : IIdConverter
    {
        private readonly string IdPattern = @"\d+";

        public int Convert(string source)
        {
            int id = 0;

            if (source != null)
            {
                var idMatch = Regex.Match(source, IdPattern);

                if (idMatch.Success)
                    int.TryParse(idMatch.Value, out id);
            }
            
            return id;
        }
    }
}
