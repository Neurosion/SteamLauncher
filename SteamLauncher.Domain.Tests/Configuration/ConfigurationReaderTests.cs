using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationReaderTests
    {
        private string TestElement = "\"RootElement\"" + Environment.NewLine +
                                     "{" + Environment.NewLine +
                                     "\t\"AttributeOne\"\t\t\"1\"" + Environment.NewLine +
                                     "\t\"AttributeTwo\"\t\t\"ValueTwo\"" + Environment.NewLine +
                                     "\t\"SubElementOne\"" + Environment.NewLine +
                                     "\t{" + Environment.NewLine +
                                     "\t\"One\"\t\t\"1.00\"" + Environment.NewLine +
                                     "\t\t\"2\"\t\t\"dos\"" + Environment.NewLine +
                                     "\t}" + Environment.NewLine +
                                     "\t\"3\"\t\t\"3.33\"" + Environment.NewLine +
                                     "\t\"SubElement Two\"" + Environment.NewLine +
                                     "\t{" + Environment.NewLine +
                                     "\t\t\"3\"\t\t\"4\"" + Environment.NewLine +
                                     "\t}" + Environment.NewLine +
                                     "}";

        [Test]
        public void RetrievedConfigurationHasExpectedRootElementName()
        {
            var reader = new RootConfigurationReader();
            var value = reader.Read(0, TestElement);

            Assert.AreEqual("RootElement", value.Name);
        }

        [Test]
        public void RetrivedConfigurationHasExpectedAttributes()
        {
            var reader = new RootConfigurationReader();
            var value = reader.Read(0, TestElement);

            Assert.AreEqual(3, value.Attributes.Count);
            Assert.IsTrue(value.Attributes.ContainsKey("AttributeOne"));
            Assert.AreEqual("1", value.Attributes["AttributeOne"]);
            Assert.IsTrue(value.Attributes.ContainsKey("AttributeTwo"));
            Assert.AreEqual("ValueTwo", value.Attributes["AttributeTwo"]);
            Assert.IsTrue(value.Attributes.ContainsKey("3"));
            Assert.AreEqual("3.33", value.Attributes["3"]);
        }

        [Test]
        public void RetievedConfigurationHasExpectedChildren()
        {
            var reader = new RootConfigurationReader();
            var value = reader.Read(0, TestElement);

            Assert.AreEqual(2, value.Children.Count);

            Assert.AreEqual("SubElementOne", value.Children[0].Name);
            Assert.IsTrue(value.Children[0].Attributes.ContainsKey("One"));
            Assert.AreEqual("1.00", value.Children[0].Attributes["One"]);
            Assert.IsTrue(value.Children[0].Attributes.ContainsKey("2"));
            Assert.AreEqual("dos", value.Children[0].Attributes["2"]);

            Assert.AreEqual("SubElement Two", value.Children[1].Name);
            Assert.IsTrue(value.Children[1].Attributes.ContainsKey("3"));
            Assert.AreEqual("4", value.Children[1].Attributes["3"]);
        }
    }
}