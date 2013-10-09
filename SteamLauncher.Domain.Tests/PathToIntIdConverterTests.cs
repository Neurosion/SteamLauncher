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
    public class PathToIntIdConverterTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("test")]
        [TestCase("   test")]
        [TestCase(" ")]
        public void ConvertsStringsWithoutNumbersToZeroId(string valueToConvert)
        {
            var converter = new PathToIntIdConverter();
            var convertedValue = converter.Convert(valueToConvert);
            
            Assert.AreEqual(0, convertedValue);
        }

        [TestCase("12", 12)]
        [TestCase(" 12", 12)]
        [TestCase("1", 1)]
        [TestCase(" 1", 1)]
        [TestCase("1 2", 2)]
        [TestCase(" 1 2", 2)]
        [TestCase("  1,2,3,4", 4)]
        [TestCase("word800_900", 900)]
        [TestCase("one1two2three", 2)]
        public void ConvertsStringsWithNumbersToTheLastSetOfNumbersInTheString(string valueToConvert, int expectedValue)
        {
            var converter = new PathToIntIdConverter();
            var convertedValue = converter.Convert(valueToConvert);

            Assert.AreEqual(expectedValue, convertedValue);
        }
    }
}