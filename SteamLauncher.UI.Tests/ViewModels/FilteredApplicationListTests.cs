using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.Domain.Data;
using SteamLauncher.Domain;
using SteamLauncher.UI.ViewModels;

namespace SteamLauncher.UI.Tests.ViewModels
{
    [TestFixture]
    public class FilteredApplicationListTests
    {
        private IApplication GetMockApplication(string name)
        {
            var applicationMock = MockRepository.GenerateMock<IApplication>();
            applicationMock.Stub(x => x.Name).Return(name);

            return applicationMock;
        }

        private IList<IApplication> GetMockApplicationList(params string[] values)
        {
            var list = new List<IApplication>();
            values.ForEach(x => list.Add(GetMockApplication(x)));
            
            return list;
        }

        [Test]
        public void ProvidesAllApplicationsWhenFilterIsEmpty()
        {
            var mockedApplications = GetMockApplicationList("One", "Two", "Three");

            var repositoryMock = MockRepository.GenerateMock<IApplicationRepository>();
            repositoryMock.Stub(x => x.Get()).Return(mockedApplications);
            
            var applicationList = new FilteredApplicationList(repositoryMock);

            Assert.IsEmpty(applicationList.Filter);

            var actualApplications = applicationList.Applications;

            Assert.AreEqual(mockedApplications.Count, actualApplications.Count);
            Assert.IsTrue(mockedApplications.Intersect(actualApplications).Count() == mockedApplications.Count);
        }

        [Test]
        public void ProvidesNoApplicationsWhenNoApplicationsMatchFilter()
        {
            var mockedApplications = GetMockApplicationList("One", "Two", "Three");

            var repositoryMock = MockRepository.GenerateMock<IApplicationRepository>();
            repositoryMock.Stub(x => x.Get()).Return(mockedApplications);

            var applicationList = new FilteredApplicationList(repositoryMock);

            applicationList.Filter = "Four";

            var actualApplications = applicationList.Applications;

            Assert.AreEqual(0, actualApplications.Count);
        }

        [Test]
        public void ProvidesApplicationsWhenFilterMatchesNameExactly()
        {
            var mockedApplications = GetMockApplicationList("One", "Two", "Three");

            var repositoryMock = MockRepository.GenerateMock<IApplicationRepository>();
            repositoryMock.Stub(x => x.Get()).Return(mockedApplications);

            var applicationList = new FilteredApplicationList(repositoryMock);

            applicationList.Filter = "One";

            var actualApplications = applicationList.Applications;

            Assert.AreEqual(1, actualApplications.Count);
            Assert.AreEqual(mockedApplications[0].Name, actualApplications.First().Name);
        }

        [Test]
        public void ProvidesApplicationWhenFilterMatchesNamePartially()
        {
            var mockedApplications = GetMockApplicationList("Booth", "Tooth", "South");

            var repositoryMock = MockRepository.GenerateMock<IApplicationRepository>();
            repositoryMock.Stub(x => x.Get()).Return(mockedApplications);

            var applicationList = new FilteredApplicationList(repositoryMock);

            applicationList.Filter = "oo";

            var actualApplications = applicationList.Applications;

            Assert.AreEqual(2, actualApplications.Count);
            Assert.AreEqual(1, actualApplications.Count(x => x.Name == mockedApplications[0].Name));
            Assert.AreEqual(1, actualApplications.Count(x => x.Name == mockedApplications[1].Name));
            Assert.AreEqual(0, actualApplications.Count(x => x.Name == mockedApplications[2].Name));
        }

        [Test]
        public void NotifiesWhenFilterChanges()
        {
            var mockedApplications = GetMockApplicationList("One");

            var repositoryMock = MockRepository.GenerateMock<IApplicationRepository>();
            repositoryMock.Stub(x => x.Get()).Return(mockedApplications);

            var applicationList = new FilteredApplicationList(repositoryMock);

            var wasPropertyChangedCalled = false;

            applicationList.PropertyChanged += (s, e) =>
                {
                    wasPropertyChangedCalled = true;
                    Assert.AreEqual("Filter", e.PropertyName);
                };

            Assert.IsFalse(wasPropertyChangedCalled);

            applicationList.Filter = "test";

            Assert.IsTrue(wasPropertyChangedCalled);
        }
    }
}