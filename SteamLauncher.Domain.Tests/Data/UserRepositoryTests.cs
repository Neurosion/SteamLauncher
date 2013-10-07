using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.Domain.Configuration;
using SteamLauncher.Domain.Data;
using SteamLauncher.Domain;

namespace SteamLauncher.Domain.Tests.Data
{
    [TestFixture]
    public class UserRepositoryTests
    {
        [Test]
        public void DoesRepositoryGenerateUserWithCorrectValuesFromConfiguration()
        {
            var watchingConfigRepoMock = MockRepository.GenerateMock<IWatchingConfigurationRepository>();
            var repository = new UserRepository(watchingConfigRepoMock);
            var mockConfig = MockRepository.GenerateMock<IConfigurationElement>();
            mockConfig.Stub(x => x.Attributes).Return(new Dictionary<string, string>()
            {
                { "IsLoggedIn", "true" }
            });
            var mockFriendsConfig = MockRepository.GenerateMock<IConfigurationElement>();
            mockFriendsConfig.Stub(x => x.Name).Return("friends");
            mockFriendsConfig.Stub(x => x.Attributes).Return(new Dictionary<string, string>()
            {
                { "PersonaName", "User Name" },
            });
            var mockProfileDetailConfig = MockRepository.GenerateMock<IConfigurationElement>();
            mockProfileDetailConfig.Stub(x => x.Name).Return("123");
            mockProfileDetailConfig.Stub(x => x.Attributes).Return(new Dictionary<string, string>()
            {
                { "name", mockFriendsConfig.Attributes["PersonaName"] }
            });
            mockFriendsConfig.Stub(x => x.Children).Return(new[] { mockProfileDetailConfig });
            mockConfig.Stub(x => x.Children).Return(new[] { mockFriendsConfig });

            watchingConfigRepoMock.Raise(x => x.Added += delegate { }, mockConfig);

            var user = repository.Get().FirstOrDefault();
            Assert.IsNotNull(user);

            Assert.AreEqual(int.Parse(mockProfileDetailConfig.Name), user.Id);
            Assert.AreEqual(mockProfileDetailConfig.Attributes["name"], user.Name);
            Assert.AreEqual(bool.Parse(mockConfig.Attributes["IsLoggedIn"]), user.IsLoggedIn);
        }

        [Test]
        public void UserIsNotAddedWhenConfigurationIsNull()
        {
            var watchingConfigRepoMock = MockRepository.GenerateMock<IWatchingConfigurationRepository>();
            var repository = new UserRepository(watchingConfigRepoMock);

            watchingConfigRepoMock.Raise(x => x.Added += delegate { }, (IConfigurationElement)null);

            var users = repository.Get();
            Assert.IsEmpty(users);
        }

        [Test]
        public void IsLoggedInIsFalseWhenValueIsMissingFromConfiguration()
        {
            var watchingConfigRepoMock = MockRepository.GenerateMock<IWatchingConfigurationRepository>();
            var repository = new UserRepository(watchingConfigRepoMock);
            var mockConfig = MockRepository.GenerateMock<IConfigurationElement>();
            mockConfig.Stub(x => x.Attributes).Return(new Dictionary<string, string>());
            var mockFriendsConfig = MockRepository.GenerateMock<IConfigurationElement>();
            mockFriendsConfig.Stub(x => x.Name).Return("friends");
            mockFriendsConfig.Stub(x => x.Attributes).Return(new Dictionary<string, string>()
            {
                { "PersonaName", "User Name" },
            });
            var mockProfileDetailConfig = MockRepository.GenerateMock<IConfigurationElement>();
            mockProfileDetailConfig.Stub(x => x.Name).Return("123");
            mockProfileDetailConfig.Stub(x => x.Attributes).Return(new Dictionary<string, string>()
            {
                { "name", mockFriendsConfig.Attributes["PersonaName"] }
            });
            mockFriendsConfig.Stub(x => x.Children).Return(new[] { mockProfileDetailConfig });
            mockConfig.Stub(x => x.Children).Return(new[] { mockFriendsConfig });

            watchingConfigRepoMock.Raise(x => x.Added += delegate { }, mockConfig);

            var user = repository.Get().FirstOrDefault();
            Assert.IsNotNull(user);

            Assert.AreEqual(int.Parse(mockProfileDetailConfig.Name), user.Id);
            Assert.AreEqual(mockProfileDetailConfig.Attributes["name"], user.Name);
            Assert.AreEqual(false, user.IsLoggedIn);
        }
    }
}