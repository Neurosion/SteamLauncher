﻿using System;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Serialization;

namespace SteamLauncher.Domain.Input
{
    [Serializable]
    public class HotKey : IHotKey
    {
        [XmlIgnore]
        private IHotKeyRegistrationController _registrationController;
        [XmlIgnore]
        private IHookRegistrationController _hookListener;
        private ModifierKeys _modifiers;
        private Keys _key;

        [XmlIgnore]
        public int Id { get; private set; }

        [XmlIgnore]
        public ModifierKeys Modifiers
        {
            get { return _modifiers; }
            set
            {
                if (_modifiers != value)
                {
                    _modifiers = value;

                    if (IsEnabled)
                        Enable(true);
                }
            }
        }

        [XmlIgnore]
        public Keys Key
        {
            get { return _key; }
            set
            {
                if (_key != value)
                {
                    _key = value;

                    if (IsEnabled)
                        Enable(true);
                }
            }
        }

        [XmlIgnore]
        public bool IsEnabled { get; private set; }

        public HotKey(IHotKeyRegistrationController registrationController, IHookRegistrationController hookListener)
        {
            _registrationController = registrationController;
            IsEnabled = false;
        }

        ~HotKey()
        {
            Disable();
        }

        public void Enable()
        {
            Enable(false);
        }

        public void Disable()
        {
            if (IsEnabled)
            {
                IsEnabled = !_registrationController.Unregister(this);
                if (!IsEnabled)
                    Id = 0;
            }
        }
        
        private void Enable(bool force)
        {
            if (Key == Keys.None)
                throw new ArgumentException("A hot key must be set before it can be enabled.");

            if (IsEnabled)
            {
                if (force)
                    Disable();
                else
                    return;
            }

            Id = _registrationController.Register(this);
            IsEnabled = true;
        }

        public override string ToString()
        {
            var output = "";

            Enum.GetValues(typeof(ModifierKeys)).Cast<ModifierKeys>()
                                                .Where(x => x != ModifierKeys.None && ((Modifiers & x) == x))
                                                .ForEach(x => output += x.ToString() + " + ");

            output += Key.ToString();

            return output;
        }
    }
}