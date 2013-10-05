using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.Domain.Configuration;
using SteamLauncher.Domain.Data;

namespace SteamLauncher.Domain.Tests.Data
{
    [TestFixture]
    public class InstalledApplicationRepositoryTests
    {
        [Test]
        public void DoesApplicationRepositoryGenerateApplicationWithCorrectValuesFromConfiguration()
        {
            var watchingConfigRepoMock = MockRepository.GenerateMock<IWatchingConfigurationRepository>();
            var repository = new InstalledApplicationRepository(watchingConfigRepoMock);
            var mockConfig = MockRepository.GenerateMock<IConfigurationElement>();
            var mockUserConfig = MockRepository.GenerateMock<IConfigurationElement>();
            mockUserConfig.Stub(x => x.Name).Return("UserConfig");
            var attributes = new Dictionary<string, string>()
            {
                { "Installed", "1" },
                { "GameID", "100" },
                { "name", "test" }
            };
            mockUserConfig.Stub(x => x.Attributes).Return(attributes);
            mockConfig.Stub(x => x.Children).Return(new[] { mockUserConfig });

            watchingConfigRepoMock.Raise(x => x.Added += delegate { }, mockConfig);

            var application = repository.Get().FirstOrDefault();
            Assert.IsNotNull(application);

            Assert.AreEqual(int.Parse(attributes["GameID"]), application.Id);
            Assert.AreEqual(attributes["name"], application.Name);
        }

        [Test]
        public void ApplicationIsNotAddedWhenConfigurationIsNull()
        {
            var watchingConfigRepoMock = MockRepository.GenerateMock<IWatchingConfigurationRepository>();
            var repository = new InstalledApplicationRepository(watchingConfigRepoMock);

            watchingConfigRepoMock.Raise(x => x.Added += delegate { }, (IConfigurationElement)null);

            var applications = repository.Get();
            Assert.IsEmpty(applications);
        }

        [Test]
        public void ApplicationIsNotAddedWhenInstalledValueIsNotOne()
        {
            var watchingConfigRepoMock = MockRepository.GenerateMock<IWatchingConfigurationRepository>();
            var repository = new InstalledApplicationRepository(watchingConfigRepoMock);
            var mockConfig = MockRepository.GenerateMock<IConfigurationElement>();
            var mockUserConfig = MockRepository.GenerateMock<IConfigurationElement>();
            mockUserConfig.Stub(x => x.Name).Return("UserConfig");
            var attributes = new Dictionary<string, string>()
            {
                { "Installed", "0" },
            };
            mockUserConfig.Stub(x => x.Attributes).Return(attributes);
            mockConfig.Stub(x => x.Children).Return(new[] { mockUserConfig });

            watchingConfigRepoMock.Raise(x => x.Added += delegate { }, mockConfig);

            var application = repository.Get().FirstOrDefault();
            Assert.IsNull(application);
        }
    }
}