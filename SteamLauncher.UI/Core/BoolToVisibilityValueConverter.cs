using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Markup;

namespace SteamLauncher.UI.Core
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityValueConverter : MarkupExtension, IValueConverter
    {
        public BoolToVisibilityValueConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var convertedValue = value != null && value is bool && ((bool)value)
                                    ? Visibility.Visible
                                    : Visibility.Hidden;

            return convertedValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var convertedValue = value != null && 
                                 value is Visibility && 
                                 ((Visibility)value) == Visibility.Visible;
            
            return convertedValue;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}