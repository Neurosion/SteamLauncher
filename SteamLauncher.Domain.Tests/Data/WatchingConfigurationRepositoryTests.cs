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
    public class WatchingConfigurationRepositoryTests
    {
        private IConfigurationResourceLocator _locatorMock;
        private IConfigurationResourceWatcher _watcherMock;
        private IRootConfigurationElement _configurationMock;
        private IConfigurationElement _configurationChildMock;
        private WatchingConfigurationRepository _repository;
        private int _id;
        private string _name;
        
        [SetUp]
        public void TestSetup()
        {
            _id = 8;
            _name = "Test Config";

            _configurationMock = MockRepository.GenerateMock<IRootConfigurationElement>();
            _configurationMock.Stub(x => x.Name).Return(_name);
            _configurationMock.Id = _id;

            _configurationChildMock = MockRepository.GenerateMock<IConfigurationElement>();
            _configurationChildMock.Stub(x => x.Name).Return("Child");

            _configurationMock.Stub(x => x.Children).Return(new List<IConfigurationElement>(new [] { _configurationChildMock }));
            _configurationMock.Stub(x => x.Attributes).Return(new Dictionary<string, string>() { { "Attribute One", "Value One" }, { "Attribute Two", "Value Two" } });
            
            _locatorMock = MockRepository.GenerateMock<IConfigurationResourceLocator>();
            _locatorMock.Stub(x => x.Locate(Arg<string>.Is.NotEqual(_name))).Return(new IConfigurationElement[] { });
            _locatorMock.Stub(x => x.Locate(Arg<string>.Is.Equal(_name))).Return(new IConfigurationElement[] { _configurationMock });

            _watcherMock = MockRepository.GenerateMock<IConfigurationResourceWatcher>();
            _repository = new WatchingConfigurationRepository(_locatorMock, _watcherMock);
            _locatorMock.Stub(x => x.Locate(Arg<string>.Is.Equal(_name))).Return(new[] { _configurationMock });
        }

        [TearDown]
        public void TestTeardown()
        {
            _locatorMock = null;
            _watcherMock = null;
            _configurationMock = null;
        }

        [Test]
        public void AddedEventIsFiredWhenConfigurationWatcherNotifiesOfAddedItem()
        {
            var wasAddedCalled = false;
            
            _repository.Added += element => wasAddedCalled = true;
            _watcherMock.Raise(x => x.ResourceAdded += delegate { }, _id, _name);

            System.Threading.Thread.Sleep(50);

            Assert.IsTrue(wasAddedCalled);
        }

        [Test]
        public void RemovedEventIsFiredWhenConfigurationWatcherNotifiesOfRemovedItemThatExistsInRepository()
        {
            var wasRemovedCalled = false;

            _repository.Removed += element => wasRemovedCalled = true;
            _watcherMock.Raise(x => x.ResourceAdded += delegate { }, _id, _name);
            _watcherMock.Raise(x => x.ResourceRemoved += delegate { }, _id, _name);

            System.Threading.Thread.Sleep(50);

            Assert.IsTrue(wasRemovedCalled);
        }

        [Test]
        public void RemovedEventIsNotFiredWhenConfigurationWatcherNotifiesOfRemovedItemThatDoesNotExistInRepository()
        {
            var wasRemovedCalled = false;

            _repository.Removed += element => wasRemovedCalled = true;
            _watcherMock.Raise(x => x.ResourceRemoved += delegate { }, _id, _name);

            System.Threading.Thread.Sleep(50);

            Assert.IsFalse(wasRemovedCalled);
        }

        [Test]
        public void UpdatedEventIsFiredWhenConfigurationWatcherNotifiesOfUpdatedItemThatExistsRepository()
        {
            var wasUpdatedCalled = false;

            _repository.Updated += (oldElement, newElement) => wasUpdatedCalled = true;
            _watcherMock.Raise(x => x.ResourceAdded += delegate { }, _id, _name);
            _watcherMock.Raise(x => x.ResourceUpdated += delegate { }, _id, _name);
            
            System.Threading.Thread.Sleep(50);

            Assert.IsTrue(wasUpdatedCalled);
        }

        [Test]
        public void UpdatedEventIsNotFiredWhenConfigurationWatcherNotifiesOfUpdatedItemThatDoesNotExistsRepository()
        {
            var wasUpdatedCalled = false;

            _repository.Updated += (oldElement, newElement) => wasUpdatedCalled = true;
            _watcherMock.Raise(x => x.ResourceUpdated += delegate { }, _id, "invalid name");

            System.Threading.Thread.Sleep(50);

            Assert.IsFalse(wasUpdatedCalled);
        }

        [Test]
        public void GetReturnsAddedConfigurationItemAfterWatcherNotification()
        {
            IEnumerable<IConfigurationElement> elements = _repository.Get();
            Assert.IsEmpty(elements);

            IConfigurationElement addedElement = null;

            _repository.Added += element => addedElement = element;
            _watcherMock.Raise(x => x.ResourceAdded += delegate { }, _id, _name);
            
            System.Threading.Thread.Sleep(150);

            elements = _repository.Get();

            Assert.IsNotEmpty(elements);
            Assert.IsNotNull(addedElement);

            var foundElement = elements.Where(x => x.Name == addedElement.Name).FirstOrDefault();
            Assert.IsNotNull(foundElement);

            Assert.AreEqual(addedElement.Name, foundElement.Name);
            Assert.AreEqual(addedElement.Attributes.Count, foundElement.Attributes.Count);
            Assert.AreEqual(addedElement.Attributes.Keys.Count, _configurationMock.Attributes.Keys.Intersect(foundElement.Attributes.Keys).Count());

            addedElement.Attributes.Keys.ForEach(x => Assert.AreEqual(_configurationMock.Attributes[x], foundElement.Attributes[x]));
        }

        [Test]
        public void GetWithIdParameterReturnsAddedConfigurationItemAfterWatcherNotification()
        {
            var element = _repository.Get(_id);
            
            Assert.IsNull(element);

            IConfigurationElement addedElement = null;

            _repository.Added += e => addedElement = e;
            _watcherMock.Raise(x => x.ResourceAdded += delegate { }, _id, _name);

            System.Threading.Thread.Sleep(150);

            element = _repository.Get(_id);

            Assert.IsNotNull(element);

            AssertAreEqual(addedElement, element);
        }

        private void AssertAreEqual(IConfigurationElement expected, IConfigurationElement actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Attributes.Count, actual.Attributes.Count);
            Assert.AreEqual(expected.Attributes.Keys.Count, actual.Attributes.Keys.Intersect(expected.Attributes.Keys).Count());

            expected.Attributes.Keys.ForEach(x => Assert.AreEqual(expected.Attributes[x], actual.Attributes[x]));
        }

        [Test]
        public void GetDoesNotReturnRemovedConfigurationItemAfterWatcherNotification()
        {
            var elements = _repository.Get();

            Assert.IsEmpty(elements);

            _watcherMock.Raise(x => x.ResourceAdded += delegate { }, _id, _name);

            System.Threading.Thread.Sleep(150);

            elements = _repository.Get();

            Assert.IsNotEmpty(elements);

            _watcherMock.Raise(x => x.ResourceRemoved += delegate { }, _id, _name);

            elements = _repository.Get();
            Assert.IsEmpty(elements);
        }

        [Test]
        public void GetWithIdParameterDoesNotReturnRemovedConfigurationItemAfterWatcherNotification()
        {
            var element = _repository.Get(_id);

            Assert.IsNull(element);

            _watcherMock.Raise(x => x.ResourceAdded += delegate { }, _id, _name);

            System.Threading.Thread.Sleep(150);

            element = _repository.Get(_id);

            Assert.IsNotNull(element);

            _watcherMock.Raise(x => x.ResourceRemoved += delegate { }, _id, _name);

            element = _repository.Get(_id);
            Assert.IsNull(element);
        }

        [Test]
        public void GetReturnsUpdatedConfigurationItemAfterWatcherNotification()
        {
            var elements = _repository.Get();

            Assert.IsEmpty(elements);

            _watcherMock.Raise(x => x.ResourceAdded += delegate { }, _id, _name);

            System.Threading.Thread.Sleep(150);

            elements = _repository.Get();

            Assert.IsNotEmpty(elements);

            _watcherMock.Raise(x => x.ResourceUpdated += delegate { }, _id, _name);

            var updatedElements = _repository.Get();
            var firstElement = elements.First();
            var firstUpdatedElement = updatedElements.First();

            AssertAreEqual(firstElement, firstUpdatedElement);
        }

        [Test]
        public void GetWithIdParameterReturnsUpdatedConfigurationItemAfterWatcherNotification()
        {
            var element = _repository.Get(_id);

            Assert.IsNull(element);

            _watcherMock.Raise(x => x.ResourceAdded += delegate { }, _id, _name);

            System.Threading.Thread.Sleep(150);

            element = _repository.Get(_id);

            Assert.IsNotNull(element);

            _watcherMock.Raise(x => x.ResourceUpdated += delegate { }, _id, _name);

            var updatedElement = _repository.Get(_id);

            AssertAreEqual(element, updatedElement);
        }
    }
}