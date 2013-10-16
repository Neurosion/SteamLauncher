using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SteamLauncher.Domain;

namespace SteamLauncher.Domain.Input
{
    public class WindowsHookRegistrationController : IHookRegistrationController
    {
        private const uint WM_HOTKEY = 0x312; // Windows hot key pressed message id

        [DllImport("user32.dll", EntryPoint = "SetWindowsHookExA")]
        private static extern IntPtr SetWindowsHookEx(int idHook, Delegate lpfn, IntPtr hmod, IntPtr dwThreadId);

        [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx")]
        private static extern int UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32.dll", EntryPoint = "CallNextHookEx")]
        private static extern int CallNextHook(IntPtr hHook, int ncode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThreadId();

        private delegate int MessageHandler(int code, IntPtr wparam, IntPtr lparam);

        public void Register(IHookListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException("listener");

            var hookPointer = SetWindowsHookEx(listener.HookId, 
                                               new MessageHandler((c, w, l) => HandleMessage(c, w, l, listener)), 
                                               IntPtr.Zero, 
                                               GetCurrentThreadId());

            if (hookPointer == IntPtr.Zero)
                ThrowExceptionIfLastErrorExists();

            listener.HookPointer = hookPointer;
        }

        public void Unregister(IHookListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException("listener");

            UnhookWindowsHookEx(listener.HookPointer);
            ThrowExceptionIfLastErrorExists();
            listener.HookPointer = IntPtr.Zero;
        }

        private void ThrowExceptionIfLastErrorExists()
        {
            var errorCode = Marshal.GetLastWin32Error();

            if (errorCode != 0)
            {
                try
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to set hot key.", ex);
                }
            }
        }

        private int HandleMessage(int code, IntPtr wparam, IntPtr lparam, IHookListener listener)
        {
            if (code == WM_HOTKEY)
            {
                var key = (Keys)wparam.ToInt32();
                listener.HandleHookMessage(key);
                
                return 1;
            }

            return CallNextHook(listener.HookPointer, code, wparam, lparam);
        }
    }
}