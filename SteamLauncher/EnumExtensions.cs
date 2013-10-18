using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;

namespace SteamLauncher.Domain
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum source)
        {
            var sourceType = source.GetType();
            var enumValueName = Enum.GetName(sourceType, source);

            var description = sourceType.GetField(enumValueName)
                                        .GetCustomAttributes(true)
                                        .OfType<DescriptionAttribute>()
                                        .Select(x => x.Description)
                                        .FirstOrDefault()
                                ?? enumValueName;

            return description;
        }
    }
}