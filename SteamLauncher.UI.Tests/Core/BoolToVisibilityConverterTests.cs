using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using NUnit.Framework;
using SteamLauncher.UI.Core;

namespace SteamLauncher.UI.Tests.Core
{
    [TestFixture]
    public class BoolToVisibilityConverterTests
    {
        [TestCase(true, Visibility.Visible)]
        [TestCase(false, Visibility.Hidden)]
        public void BooleanValuesConvertToCorrectVisibilityValues(bool value, Visibility expectedValue)
        {
            var converter = new BoolToVisibilityValueConverter();
            var actualValue = (Visibility)converter.Convert(value, null, null, null);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCase(Visibility.Collapsed, false)]
        [TestCase(Visibility.Hidden, false)]
        [TestCase(Visibility.Visible, true)]
        public void VisibilityValuesConvertToCorrectBooleanValues(Visibility value, bool expectedValue)
        {
            var converter = new BoolToVisibilityValueConverter();
            var actualValue = (bool)converter.ConvertBack(value, null, null, null);

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}