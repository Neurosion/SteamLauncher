using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SteamLauncher.UI.ViewModels;

namespace SteamLauncher.UI.Views
{
    public abstract class WindowBase<T> : Window, IView<T>
        where T: IViewModel
    {
        public T ViewModel
        {
            get { return (T)DataContext; }
        }

        // NOTE: This parameterless constructor is required to make the XAML parser happy.
        public WindowBase()
        {
        }

        public WindowBase(T viewModel)
        {
            DataContext = viewModel;
        }

        // HACK: This is here because the OnPropertyChanged delegate is not 
        // handling the event notification when the window is in a closing state.
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}