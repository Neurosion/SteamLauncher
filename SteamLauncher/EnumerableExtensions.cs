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

        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> requiredItems)
        {
            if (source != null && requiredItems != null)
            {
                var doesContainAll = true;
                requiredItems.ForEach(x => doesContainAll = doesContainAll && source.Contains(x));
                return doesContainAll;
            }

            return false;
        }

        public static bool ContainsAll<T>(this IEnumerable<T> source, params T[] requiredItems)
        {
            var doesContainAll = ContainsAll<T>(source, (IEnumerable<T>)requiredItems);
            return doesContainAll;
        }
    }
}
