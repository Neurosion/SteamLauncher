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
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceLocator(null, null, null, null));
        }

        [Test]
        public void ThrowsExceptionWhenEmptyPathIsProvided()
        {
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceLocator(string.Empty, null, null, null));
        }

        [Test]
        public void ThrowsExceptionWhenInvalidPathIsProvided()
        {
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceLocator(Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString()), null, null, null));
        }

        [Test]
        public void ReturnsEmptyWhenNoItemCanBeLocated()
        {
            var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, string.Empty, null, null);
            var result = locator.Locate("test");

            Assert.IsEmpty(result);
        }

        [Test]
        public void ProvidesConfigReaderWithContentsOfMatchingFiles()
        {
            AssertFileBasedTest(new[] { "test", "test.a", "test.b", "test.c", "test_2.b" },
                fileNames =>
                {
                    var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
                    var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
                    configReaderMock.Stub(x => x.Read(Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(configElementMock);
                    var extension = "b";
                    var converterMock = MockRepository.GenerateMock<IIdConverter>();
                    converterMock.Stub(x => x.Convert(Arg<string>.Is.Anything)).Return(0);
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, extension, configReaderMock, converterMock);
                    var result = locator.Locate(string.Empty);
                    var matchedFileNames = fileNames.Where(x => x.EndsWith(extension));

                    Assert.AreEqual(2, result.Count());

                    foreach (var currentItem in matchedFileNames)
                        configReaderMock.AssertWasCalled(x => x.Read(Arg<int>.Is.Anything, Arg<string>.Is.Equal("Test File: " + currentItem)), c => c.Repeat.Once());
                });
        }

        [Test]
        public void EmptyExtensionReturnsResultsWithNoExtension()
        {
            AssertFileBasedTest(new[] { "test", "test.a", "test.b", "test.c" },
                fileNames =>
                {
                    var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
                    var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
                    configReaderMock.Stub(x => x.Read(Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(configElementMock);
                    var converterMock = MockRepository.GenerateMock<IIdConverter>();
                    converterMock.Stub(x => x.Convert(Arg<string>.Is.Anything)).Return(0);
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, string.Empty, configReaderMock, converterMock);
                    var result = locator.Locate("test");

                    Assert.AreEqual(1, result.Count());
                });
        }

        [Test]
        public void EmptyLocatorValueWithEmptyExtensionReturnsAllResults()
        {
            AssertFileBasedTest(new[] { "test", "test.a", "test.b", "test.c" },
                fileNames =>
                {
                    var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
                    var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
                    configReaderMock.Stub(x => x.Read(Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(configElementMock);
                    var converterMock = MockRepository.GenerateMock<IIdConverter>();
                    converterMock.Stub(x => x.Convert(Arg<string>.Is.Anything)).Return(0);
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, string.Empty, configReaderMock, converterMock);
                    var result = locator.Locate(string.Empty);

                    Assert.AreEqual(Directory.GetFiles(Environment.CurrentDirectory, "*").Count(), result.Count());
                });
        }

        [Test]
        public void NullLocatorValueWithEmptyExtensionReturnsAllResults()
        {
            AssertFileBasedTest(new[] { "test", "test.a", "test.b", "test.c" },
                fileNames =>
                {
                    var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
                    var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
                    configReaderMock.Stub(x => x.Read(Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(configElementMock);
                    var converterMock = MockRepository.GenerateMock<IIdConverter>();
                    converterMock.Stub(x => x.Convert(Arg<string>.Is.Anything)).Return(0);
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, string.Empty, configReaderMock, converterMock);
                    var result = locator.Locate(null);

                    Assert.AreEqual(Directory.GetFiles(Environment.CurrentDirectory, "*").Count(), result.Count());
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
                    configReaderMock.Stub(x => x.Read(Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(configElementMock);
                    var extension = "x";
                    var converterMock = MockRepository.GenerateMock<IIdConverter>();
                    converterMock.Stub(x => x.Convert(Arg<string>.Is.Anything)).Return(0);
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, extension, configReaderMock, converterMock);
                    var result = locator.Locate(string.Empty);

                    var expectedFileNames = fileNames.Where(x => x.EndsWith(extension));

                    Assert.AreEqual(expectedFileNames.Count(), result.Count());
                });
        }

        [Test]
        public void NullLocatorValueWithNonEmptyExtensionReturnsAllResultsWithThatExtension()
        {
            AssertFileBasedTest(new[] { "test", "test.a", "test.b", "test.c", "test_1.x", "test_2.x" },
                fileNames =>
                {
                    var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
                    var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
                    configReaderMock.Stub(x => x.Read(Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(configElementMock);
                    var extension = "x";
                    var converterMock = MockRepository.GenerateMock<IIdConverter>();
                    converterMock.Stub(x => x.Convert(Arg<string>.Is.Anything)).Return(0);
                    var locator = new ConfigurationResourceLocator(Environment.CurrentDirectory, extension, configReaderMock, converterMock);
                    var result = locator.Locate(null);

                    Assert.AreEqual(Directory.GetFiles(Environment.CurrentDirectory, "*." + extension).Count(), result.Count());
                });
        }
    }
}