using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace SteamLauncher.Domain.Tests
{
    [TestFixture]
    public class EnumExtensionsTests
    {
        public enum TestEnum
        {
            [Description("First")]
            One,
            Two
        }

        [TestCase(TestEnum.One, "First")]
        [TestCase(TestEnum.Two, "Two")]
        public void GetDescriptionGetsValueOfDescriptionAttributeOrValueNameIfNoDescriptionAttributeExists(TestEnum enumValue, string expectedDescription)
        {
            var actualDescription = enumValue.GetDescription();
            Assert.AreEqual(expectedDescription, actualDescription);
        }
    }
}