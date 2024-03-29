﻿using System;
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
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : WindowBase<ISettingsViewModel>, ISettingsView
    {
        public SettingsWindow(ISettingsViewModel viewModel)
            :base(viewModel)
        {
            InitializeComponent();
        }
    }
}