using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.Domain;

namespace SteamLauncher.Domain.Tests
{
    [TestFixture]
    public class StringtoIntIdConverterTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("test")]
        [TestCase("   test")]
        [TestCase(" ")]
        public void ConvertsStringsWithoutNumbersToZeroId(string valueToConvert)
        {
            var converter = new StringToIntIdConverter();
            var convertedValue = converter.Convert(valueToConvert);
            
            Assert.AreEqual(0, convertedValue);
        }

        [TestCase("12", 12)]
        [TestCase(" 12", 12)]
        [TestCase("1", 1)]
        [TestCase(" 1", 1)]
        [TestCase("1 2", 1)]
        [TestCase(" 1 2", 1)]
        [TestCase("  4,3,2,1", 4)]
        [TestCase("word800_900", 800)]
        public void ConvertsStringsWithNumbersToTheFirstSetOfNumbersInTheString(string valueToConvert, int expectedValue)
        {
            var converter = new StringToIntIdConverter();
            var convertedValue = converter.Convert(valueToConvert);

            Assert.AreEqual(expectedValue, convertedValue);
        }
    }
}