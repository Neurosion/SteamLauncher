using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SteamLauncher.Domain.Input
{
    public class WindowsHotKeyRegistrationController : IHotKeyRegistrationController
    {
        private const uint WM_HOTKEY = 0x312; // Windows hot key pressed message id
        private const uint ERROR_HOTKEY_ALREADY_REGISTERED = 1409; // Windows hot key already registered message id
        private const int MAXIMUM_HOTKEY_ID = 0xBFFF; // The maximum allowed hot key id

        private int _currentHotKeyId = 1;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, uint modifiers, Keys virtualKeys);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);

        public int Register(IHotKey hotKey)
        {
            var assignedId = 0;

            if (hotKey != null)
            {
                assignedId = hotKey.Id;

                if (hotKey.Key != Keys.None)
                {
                    if (assignedId == 0)
                        assignedId = IncrementHotKeyId();

                    if (RegisterHotKey(hotKey.ParentWindowHandle, assignedId, (uint)hotKey.Modifiers, Keys.None) == 0 &&
                        Marshal.GetLastWin32Error() != ERROR_HOTKEY_ALREADY_REGISTERED)
                        GenerateException("register", hotKey);
                }
            }

            return assignedId;
        }

        private int IncrementHotKeyId()
        {
            _currentHotKeyId = _currentHotKeyId + 1 % MAXIMUM_HOTKEY_ID;
            return _currentHotKeyId;
        }

        public bool Unregister(IHotKey hotKey)
        {
            var didUnregister = false;

            if (hotKey != null)
            {
                if (UnregisterHotKey(hotKey.ParentWindowHandle, hotKey.Id) == 0)
                    GenerateException("unregister", hotKey);

                didUnregister = true;
            }

            return didUnregister;
        }

        private void GenerateException(string action, IHotKey hotKey)
        {
            throw new Exception(string.Format("An error occurred while attempting to {0} the hot key {1}.  Error code {2}.",
                                              action,                                        
                                              hotKey.ToString(),
                                              Marshal.GetLastWin32Error()));
        }
    }
}