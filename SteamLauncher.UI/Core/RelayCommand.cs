using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SteamLauncher.UI.Core
{
    public class RelayCommand : ICommand
    {
        private Action<object> _method;
        private Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged = delegate { };

        public RelayCommand(Action<object> method, Func<object, bool> canExecute)
        {
            _method = method;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                _method(parameter);
        }
    }
}