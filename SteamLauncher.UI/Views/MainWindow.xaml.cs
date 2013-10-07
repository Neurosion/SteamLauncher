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
using SteamLauncher.UI.ViewModels;

namespace SteamLauncher.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        public MainWindow(IFilteredApplicationList applicationList)
        {
            InitializeComponent();
            DataContext = applicationList;
        }

        //<ItemsControl ItemsSource="{Binding ApplicationGroups}">
        //    <ItemsControl.ItemTemplate>
        //        <DataTemplate>
        //            <StackPanel Orientation="Vertical">
        //                <!-- Section header -->
        //                <StackPanel Orientation="Vertical">
        //                    <!-- Section applications list -->
        //                </StackPanel>
        //            </StackPanel>
        //        </DataTemplate>
        //    </ItemsControl.ItemTemplate>
        //    <ItemsControl.ItemsPanel>
        //        <ItemsPanelTemplate>
        //            <StackPanel Orientation="Vertical" />
        //        </ItemsPanelTemplate>
        //    </ItemsControl.ItemsPanel>
        //</ItemsControl>
    }
}
