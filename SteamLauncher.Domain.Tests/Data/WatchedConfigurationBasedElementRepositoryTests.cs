using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.Domain.Data;
using SteamLauncher.Domain.Configuration;

namespace SteamLauncher.Domain.Tests.Data
{
    [TestFixture]
    public class WatchedConfigurationBasedElementRepositoryTests
    {
        internal interface ITestInterface : IIdentifiable<int>, ICopyable<ITestInterface>
        {
            string Name { get; set; }
        }

        internal class TestClass : ITestInterface
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public void Copy(ITestInterface source)
            {
                this.Id = source.Id;
                this.Name = source.Name;
            }
        }

        internal class TestRepository : WatchedConfigurationBasedElementRepository<ITestInterface, int>
        {
            public TestRepository(IWatchingConfigurationRepository watchingRepo)
                : base(watchingRepo)
            {
            }

            protected override ITestInterface Load(IConfigurationElement configuration)
            {
                var rootConfigElement = configuration as IRootConfigurationElement;
                
                var item = new TestClass()
                {
                    Id = rootConfigElement != null
                            ? rootConfigElement.Id
                            : -1,
                    Name = configuration.Name
                };

                return item;
            }
        }

        [Test]
        public void ReturnsEmptyCollectionWhenNoItemsExist()
        {
            var watchingConfigRepoMock = MockRepository.GenerateMock<IWatchingConfigurationRepository>();
            var repository = new TestRepository(watchingConfigRepoMock);

            var items = repository.Get();
            Assert.IsEmpty(items);
        }

        [Test]
        public void ReturnsNullWhenItemWithIdDoesNotExist()
        {
            var watchingConfigRepoMock = MockRepository.GenerateMock<IWatchingConfigurationRepository>();
            var repository = new TestRepository(watchingConfigRepoMock);

            var item = repository.Get(0);
            Assert.IsNull(item);
        }

        private void AssertRepositoryGetsItemWhenWatcherAddsConfiguration(Func<TestRepository, ITestInterface> retrieveMethod)
        {
            var watchingConfigRepoMock = MockRepository.GenerateMock<IWatchingConfigurationRepository>();
            var repository = new TestRepository(watchingConfigRepoMock);
            var itemId = 0;
            var configMock = MockRepository.GenerateMock<IRootConfigurationElement>();
            configMock.Id = itemId;

            var item = retrieveMethod(repository);
            Assert.IsNull(item);

            watchingConfigRepoMock.Raise(x => x.Added += delegate { }, configMock);

            System.Threading.Thread.Sleep(100);

            item = retrieveMethod(repository);
            Assert.IsNotNull(item);
            Assert.AreEqual(itemId, item.Id);
        }

        [Test]
        public void RepositoryGetsItemByIdWhenWatcherAddsConfiguration()
        {
            AssertRepositoryGetsItemWhenWatcherAddsConfiguration(x => x.Get(0));
        }

        [Test]
        public void RepositoryGetsItemInCollectionWhenWatcherAddsConfiguration()
        {
            AssertRepositoryGetsItemWhenWatcherAddsConfiguration(x => x.Get().FirstOrDefault());
        }

        private void AssertRepositoryDoesNotGetItemWhenWatcherRemovesConfiguration(Func<TestRepository, ITestInterface> retrieveMethod)
        {
            var watchingConfigRepoMock = MockRepository.GenerateMock<IWatchingConfigurationRepository>();
            var repository = new TestRepository(watchingConfigRepoMock);
            var itemId = 0;
            var configMock = MockRepository.GenerateMock<IRootConfigurationElement>();
            configMock.Id = itemId;

            var item = retrieveMethod(repository);
            Assert.IsNull(item);

            watchingConfigRepoMock.Raise(x => x.Added += delegate { }, configMock);

            item = retrieveMethod(repository);
            Assert.IsNotNull(item);

            watchingConfigRepoMock.Raise(x => x.Removed += delegate { }, configMock);

            item = retrieveMethod(repository);
            Assert.IsNull(item);
        }

        [Test]
        public void RepositoryDoesNotGetItemWithIdWhenWatcherRemovesConfiguration()
        {
            AssertRepositoryDoesNotGetItemWhenWatcherRemovesConfiguration(x => x.Get(0));
        }

        [Test]
        public void RepositoryDoesNotGetItemInCollectionWhenWatcherRemovesConfiguration()
        {
            AssertRepositoryDoesNotGetItemWhenWatcherRemovesConfiguration(x => x.Get().FirstOrDefault());
        }

        private void AssertRepositoryGetsUpdatedItemWhenWatcherUpdatesConfiguration(Func<TestRepository, ITestInterface> retrieveMethod)
        {
            var watchingConfigRepoMock = MockRepository.GenerateMock<IWatchingConfigurationRepository>();
            var repository = new TestRepository(watchingConfigRepoMock);
            var testId = 0;
            var configMock = MockRepository.GenerateMock<IRootConfigurationElement>();
            configMock.Id = testId;
            configMock.Stub(x => x.Name).Return("Not Updated");
            var updatedConfigMock = MockRepository.GenerateMock<IRootConfigurationElement>();
            updatedConfigMock.Id = testId;
            updatedConfigMock.Stub(x => x.Name).Return("Updated");

            watchingConfigRepoMock.Raise(x => x.Added += delegate { }, configMock);

            var item = retrieveMethod(repository);
            Assert.IsNotNull(item);

            watchingConfigRepoMock.Raise(x => x.Updated += delegate { }, configMock, updatedConfigMock);

            var updatedItem = retrieveMethod(repository);
            Assert.IsNotNull(updatedItem);

            Assert.AreEqual(item.Id, updatedItem.Id);
            Assert.AreEqual(updatedItem.Name, updatedConfigMock.Name);
        }

        [Test]
        public void RepositoryGetsUpdatedItemByIdWhenWatcherUpdatesConfiguration()
        {
            AssertRepositoryGetsUpdatedItemWhenWatcherUpdatesConfiguration(x => x.Get(0));
        }

        [Test]
        public void RepositoryGetsUpdatedItemInCollectionWhenWatcherUpdatesConfiguration()
        {
            AssertRepositoryGetsUpdatedItemWhenWatcherUpdatesConfiguration(x => x.Get().FirstOrDefault());
        }
    }
}
