using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SteamLauncher.Domain
{
    public class PathToIntIdConverter : IIdConverter
    {
        private readonly string IdPattern = @"(?:.*?)(?<id>[0-9]+)(?:[^0-9]*)$";

        public int Convert(string source)
        {
            int id = 0;

            if (source != null)
            {
                var idMatch = Regex.Match(source, IdPattern);
                
                if (idMatch.Groups["id"].Success)
                    int.TryParse(idMatch.Groups["id"].Value, out id);
            }
            
            return id;
        }
    }
}
