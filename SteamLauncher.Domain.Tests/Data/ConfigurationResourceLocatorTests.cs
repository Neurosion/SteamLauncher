using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.Domain.Data;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Tests.Data
{
    [TestFixture]
    public class ConfigurationResourceLocatorTests : FileBasedTestFixture
    {
        [Test]
        public void ThrowsExceptionWhenNullPathIsProvided()
        {
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceLocator(null, null, null));
        }

        [Test]
        public void ThrowsExceptionWhenEmptyPathIsProvided()
        {
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceLocator(string.Empty, null, null));
        }

        [Test]
        public void ThrowsExceptionWhenInvalidPathIsProvided()
        {
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceLocator(Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString()), null, null));
        }

        [Test]
        public void ReturnsEmptyWhenNoItemCanBeLocated()
        {
            var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, string.Empty, null);
            var result = locator.Locate("test");

            Assert.IsEmpty(result);
        }

        [Test]
        public void EmptyExtensionReturnsResultsWithAnyExtension()
        {
            AssertFileBasedTest(new[] { "test", "test.a", "test.b", "test.c" },
                fileNames =>
                {
                    var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
                    var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
                    configReaderMock.Stub(x => x.Read(Arg<string>.Is.Anything)).Return(configElementMock);
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, string.Empty, configReaderMock);
                    var result = locator.Locate("test");

                    Assert.AreEqual(fileNames.Length, result.Count());

                    foreach (var currentFileName in fileNames)
                        configReaderMock.AssertWasCalled(x => x.Read(Arg<string>.Is.Equal(currentFileName)), c => c.Repeat.Once());
                });
        }

        [Test]
        public void EmptyLocatorValueWithEmptyExtensionReturnsEmptyResults()
        {
            AssertFileBasedTest(new[] { "test", "test.a", "test.b", "test.c" },
                fileNames =>
                {
                    var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
                    var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
                    configReaderMock.Stub(x => x.Read(Arg<string>.Is.Anything)).Return(configElementMock);
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, string.Empty, configReaderMock);
                    var result = locator.Locate(string.Empty);

                    Assert.AreEqual(0, result.Count());
                });
        }

        [Test]
        public void NullLocatorValueWithEmptyExtensionReturnsEmptyResults()
        {
            AssertFileBasedTest(new[] { "test", "test.a", "test.b", "test.c" },
                fileNames =>
                {
                    var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
                    var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
                    configReaderMock.Stub(x => x.Read(Arg<string>.Is.Anything)).Return(configElementMock);
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, string.Empty, configReaderMock);
                    var result = locator.Locate(null);

                    Assert.AreEqual(0, result.Count());
                });
        }

        [Test]
        public void NonEmptyExtensionReturnsResultsWithOnlyThatExtension()
        {
            AssertFileBasedTest(new[] { "test", "test.a", "test.b", "test.c", "test_1.x", "test_2.x" },
                fileNames =>
                {
                    var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
                    var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
                    configReaderMock.Stub(x => x.Read(Arg<string>.Is.Anything)).Return(configElementMock);
                    var extension = "x";
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, extension, configReaderMock);
                    var result = locator.Locate("test");

                    var expectedFileNames = fileNames.Where(x => x.EndsWith(extension));

                    Assert.AreEqual(expectedFileNames.Count(), result.Count());

                    foreach (var currentFileName in expectedFileNames)
                        configReaderMock.AssertWasCalled(x => x.Read(Arg<string>.Is.Equal(currentFileName)), c => c.Repeat.Once());
                });
        }

        [Test]
        public void NullLocatorValueWithNonEmptyExtensionReturnsEmptyResults()
        {
            AssertFileBasedTest(new[] { "test", "test.a", "test.b", "test.c", "test_1.x", "test_2.x" },
                fileNames =>
                {
                    var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
                    var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
                    configReaderMock.Stub(x => x.Read(Arg<string>.Is.Anything)).Return(configElementMock);
                    var extension = "x";
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, extension, configReaderMock);
                    var result = locator.Locate(null);

                    Assert.AreEqual(0, result.Count());
                });
        }

        [Test]
        public void EmptyLocatorValueWithNonEmptyExtensionReturnsEmptyResults()
        {
            AssertFileBasedTest(new[] { "test", "test.a", "test.b", "test.c", "test_1.x", "test_2.x" },
                fileNames =>
                {
                    var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
                    var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
                    configReaderMock.Stub(x => x.Read(Arg<string>.Is.Anything)).Return(configElementMock);
                    var extension = "x";
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, extension, configReaderMock);
                    var result = locator.Locate(string.Empty);

                    Assert.AreEqual(0, result.Count());
                });
        }
    }
}