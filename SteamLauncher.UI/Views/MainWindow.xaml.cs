using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using SteamLauncher.UI.ViewModels;
using SteamLauncher.Domain;

namespace SteamLauncher.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase<IMainViewModel>, IMainView
    {
        public MainWindow(IMainViewModel viewModel)
            :base(viewModel)
        {
            InitializeComponent();
        }

        private void ListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat)
                ViewModel.Launch(((ListBox)sender).SelectedItem as IApplication);
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Launch(((ListBox)sender).SelectedItem as IApplication);
        }
    }
}