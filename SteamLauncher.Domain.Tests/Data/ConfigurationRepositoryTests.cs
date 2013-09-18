using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.Domain.Data;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Tests.Data
{
    [TestFixture]
    public class ConfigurationRepositoryTests
    {
        private IConfigurationReader GetConfigReader(IConfigurationElement returnValue = null)
        {
            var configReaderMock = MockRepository.GenerateMock<IConfigurationReader>();
            configReaderMock.Stub(x => x.Read(Arg<string>.Is.Anything)).Return(returnValue);

            return configReaderMock;
        }

        [Test]
        public void GetWithProvidedIdReturnsNullWhenNoMatchingConfigurationExists()
        {
            var locatorMock = MockRepository.GenerateMock<IResourceLocator>();
            var repository = new ConfigurationRepository(locatorMock, GetConfigReader());
            var value = repository.Get("test");

            Assert.IsNull(value);
        }

        [Test]
        public void GetReturnsEmptyWhenNoMatchingConfigurationsExist()
        {
            var locatorMock = MockRepository.GenerateMock<IResourceLocator>();
            var repository = new ConfigurationRepository(locatorMock, GetConfigReader());
            var value = repository.Get();

            Assert.IsEmpty(value);
        }

        [Test]
        public void GetsConfigurationWhenMatchingConfigurationExists()
        {
            var elementId = "test";
            var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
            configElementMock.Stub(x => x.Name).Return(elementId);
            var locatorMock = MockRepository.GenerateMock<IResourceLocator>();
            locatorMock.Stub(x => x.Locate(elementId)).Return(new [] { "" });
            var repository = new ConfigurationRepository(locatorMock, GetConfigReader(configElementMock));
            var value = repository.Get(elementId);
            Assert.IsNotNull(value);
        }
    }
}