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
        [Test]
        public void GetWithProvidedIdReturnsNullWhenNoMatchingConfigurationExists()
        {
            var locatorMock = MockRepository.GenerateMock<IConfigurationResourceLocator>();
            var repository = new ConfigurationRepository(locatorMock);
            var value = repository.Get("test");

            Assert.IsNull(value);
        }

        [Test]
        public void GetReturnsEmptyWhenNoMatchingConfigurationsExist()
        {
            var locatorMock = MockRepository.GenerateMock<IConfigurationResourceLocator>();
            var repository = new ConfigurationRepository(locatorMock);
            var value = repository.Get();

            Assert.IsEmpty(value);
        }

        [Test]
        public void GetsConfigurationWhenMatchingConfigurationExists()
        {
            var elementId = "test";
            var configElementMock = MockRepository.GenerateMock<IConfigurationElement>();
            configElementMock.Stub(x => x.Name).Return(elementId);
            var locatorMock = MockRepository.GenerateMock<IConfigurationResourceLocator>();
            locatorMock.Stub(x => x.Locate(elementId)).Return(new [] { configElementMock });
            var repository = new ConfigurationRepository(locatorMock);
            var value = repository.Get(elementId);
            Assert.IsNotNull(value);
        }
    }
}