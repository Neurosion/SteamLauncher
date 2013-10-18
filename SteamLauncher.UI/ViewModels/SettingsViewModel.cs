using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Forms;
using SteamLauncher.Domain.Input;
using SteamLauncher.UI.Core;

namespace SteamLauncher.UI.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        private bool _isVisible;
        private bool _isValid;
        private IHotKey _hotKey;
        private IHotKey _editingHotKey;
        private RelayCommand _saveCommand;
        private RelayCommand _cancelCommand;

        public string Title
        {
            get { return "Settings"; }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    PropertyChanged.Notify();
                }
            }
        }

        public string HotKeyString
        {
            get { return _editingHotKey.ToString(); }
            set { /*_editingHotKey.Parse(value);*/ }
        }

        public ICommand SaveCommand
        {
            get 
            { 
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(x => Save(), x => IsValid);

                return _saveCommand;
            }
        }

        public ICommand CancelCommand
        {
            get 
            { 
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(x => Cancel(), x => true);

                return _cancelCommand;
            }
        }

        public bool IsValid 
        {
            get { return _isValid; }
            private set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    PropertyChanged.Notify();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel(IHotKey hotKey)
        {
            _hotKey = hotKey;
            Reset();
        }

        public void Close()
        {
            IsVisible = false;
            Reset();
        }

        private void Reset()
        {
            _editingHotKey = _hotKey;
            IsValid = true;
        }

        private void Validate()
        {
            IsValid = _hotKey.Key != Keys.None;
        }

        private void Save()
        {
            if (IsValid)
                IsVisible = false;
        }

        private void Cancel()
        {
            IsVisible = false;
        }
    }
}