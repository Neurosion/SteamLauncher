using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using SteamLauncher.Domain;

namespace SteamLauncher.UI
{
    public static class PropertyChangedEventHandlerExtensions
    {
        public static void Notify(this PropertyChangedEventHandler source)
        {
            var stackFrame = new StackFrame(1); // Skipping one frame to skip this method and get the frame for the calling method
            var callingMethod = stackFrame.GetMethod();
            var containingProperty = callingMethod.DeclaringType
                                                  .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                                  .Where(x => x.GetGetMethod(true) == callingMethod || x.GetSetMethod(true) == callingMethod)
                                                  .FirstOrDefault();
            
            if (containingProperty != null)
                source.GetInvocationList().ForEach(x => x.DynamicInvoke(containingProperty.Name));
        }
    }
}