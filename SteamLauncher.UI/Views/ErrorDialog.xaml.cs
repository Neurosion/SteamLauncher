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
using System.Windows.Shapes;

namespace SteamLauncher.UI.Views
{
    /// <summary>
    /// Interaction logic for ErrorDialog.xaml
    /// </summary>
    public partial class ErrorDialog : Window, IErrorDialogView
    {
        public string Title
        {
            get { return "Application Error"; }
        }

        public string ErrorMessage { get; private set; }

        public ErrorDialog()
        {
            InitializeComponent();
        }

        public void Show(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
            Show();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage = string.Empty;
            Hide();
        }
    }
}
