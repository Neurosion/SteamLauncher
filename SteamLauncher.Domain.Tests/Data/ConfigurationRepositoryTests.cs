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
            locatorMock.Stub(x => x.Locate(Arg<string>.Is.Anything)).Return(new IConfigurationElement[] { });
            var repository = new ConfigurationRepository(locatorMock);
            var value = repository.Get(0);

            Assert.IsNull(value);
        }

        [Test]
        public void GetReturnsEmptyWhenNoMatchingConfigurationsExist()
        {
            var locatorMock = MockRepository.GenerateMock<IConfigurationResourceLocator>();
            locatorMock.Stub(x => x.Locate(Arg<string>.Is.Anything)).Return(new IConfigurationElement[] { });
            var repository = new ConfigurationRepository(locatorMock);
            var value = repository.Get();

            Assert.IsEmpty(value);
        }

        [Test]
        public void GetsConfigurationWhenMatchingConfigurationExists()
        {
            int elementId = 0;
            var configElementMock = MockRepository.GenerateMock<IRootConfigurationElement>();
            configElementMock.Id = elementId;
            var locatorMock = MockRepository.GenerateMock<IConfigurationResourceLocator>();
            locatorMock.Stub(x => x.Locate(elementId.ToString())).Return(new [] { configElementMock });
            var repository = new ConfigurationRepository(locatorMock);
            var value = repository.Get(elementId);
            Assert.IsNotNull(value);
        }
    }
}