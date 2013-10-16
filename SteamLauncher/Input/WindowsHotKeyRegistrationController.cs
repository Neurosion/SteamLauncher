using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SteamLauncher.Domain.Input
{
    public class WindowsHotKeyRegistrationController : IHotKeyRegistrationController
    {
        private const uint ERROR_HOTKEY_ALREADY_REGISTERED = 1409; // Windows hot key already registered message id
        private const int MAXIMUM_HOTKEY_ID = 0xBFFF; // The maximum allowed hot key id

        private int _currentHotKeyId = 1;
        private IHookRegistrationController _hookRegistrationController;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, uint modifiers, Keys virtualKeys);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);

        public WindowsHotKeyRegistrationController(IHookRegistrationController hookRegistrationController)
        {
            _hookRegistrationController = hookRegistrationController;
        }

        public int Register(IHotKey hotKey)
        {
            if (hotKey == null)
                throw new ArgumentNullException("hotKey");

            var assignedId = hotKey.Id;

            if (hotKey.Key != Keys.None)
            {
                if (assignedId == 0)
                    assignedId = IncrementHotKeyId();

                _hookRegistrationController.Register(hotKey);

                if (RegisterHotKey(hotKey.HookPointer, assignedId, (uint)hotKey.Modifiers, Keys.None) == 0 &&
                    Marshal.GetLastWin32Error() != ERROR_HOTKEY_ALREADY_REGISTERED)
                    GenerateException("register", hotKey);
            }

            return assignedId;
        }

        public bool Unregister(IHotKey hotKey)
        {
            if (hotKey == null)
                throw new ArgumentNullException("hotKey");

            _hookRegistrationController.Unregister(hotKey);

            if (UnregisterHotKey(hotKey.HookPointer, hotKey.Id) == 0)
                GenerateException("unregister", hotKey);

            return true;
        }

        private int IncrementHotKeyId()
        {
            _currentHotKeyId = _currentHotKeyId + 1 % MAXIMUM_HOTKEY_ID;
            return _currentHotKeyId;
        }

        private void GenerateException(string action, IHotKey hotKey)
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
                    throw new Exception(string.Format("An error occurred while attempting to {0} the hot key {1}.",
                                              action,
                                              hotKey.ToString()),
                                        ex);
                }
            }
        }
    }
}