using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> itemHandler)
        {
            if (source != null)
                foreach (var currentItem in source)
                    itemHandler(currentItem);
        }
    }
}
