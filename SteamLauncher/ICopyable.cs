using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamLauncher.Domain
{
    public interface ICopyable<T>
    {
        void Copy(T target);
    }
}