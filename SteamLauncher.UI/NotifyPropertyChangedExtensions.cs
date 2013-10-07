using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using SteamLauncher.Domain;

namespace SteamLauncher.UI
{
    public static class NotifyPropertyChangedExtensions
    {
        public static void Notify(this PropertyChangedEventHandler source)
        {
            if (source != null)
            {
                var methodStack = new StackFrame(1); // Skipping one frame to skip this method and get the frame for the calling method
                var callingMethod = methodStack.GetMethod();
                var containingProperty = callingMethod.DeclaringType
                                                      .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                                      .Where(x => x.GetGetMethod(true) == callingMethod || x.GetSetMethod(true) == callingMethod)
                                                      .FirstOrDefault();

                if (containingProperty != null)
                {
                    var eventArgs = new PropertyChangedEventArgs(containingProperty.Name);
                    source.GetInvocationList().ForEach(x => x.DynamicInvoke(source, eventArgs));
                }
            }
        }
    }
}