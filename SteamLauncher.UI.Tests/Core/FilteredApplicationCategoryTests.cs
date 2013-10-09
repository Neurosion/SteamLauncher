using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.Domain.Data;
using SteamLauncher.Domain;
using SteamLauncher.UI.Core;

namespace SteamLauncher.UI.Tests.Core
{
    [TestFixture]
    public class FilteredApplicationCategoryTests
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
            
            var applicationList = new FilteredApplicationCategory(null, repositoryMock);

            Assert.IsEmpty(applicationList.Filter);

            var actualApplications = applicationList.Applications;

            Assert.AreEqual(mockedApplications.Count, actualApplications.Count());
            Assert.IsTrue(mockedApplications.Intersect(actualApplications).Count() == mockedApplications.Count);
        }

        [Test]
        public void ProvidesNoApplicationsWhenNoApplicationsMatchFilter()
        {
            var mockedApplications = GetMockApplicationList("One", "Two", "Three");

            var repositoryMock = MockRepository.GenerateMock<IApplicationRepository>();
            repositoryMock.Stub(x => x.Get()).Return(mockedApplications);

            var applicationList = new FilteredApplicationCategory(null, repositoryMock);

            applicationList.Filter = "Four";

            var actualApplications = applicationList.Applications;

            Assert.AreEqual(0, actualApplications.Count());
        }

        [Test]
        public void ProvidesApplicationsWhenFilterMatchesNameExactly()
        {
            var mockedApplications = GetMockApplicationList("One", "Two", "Three");

            var repositoryMock = MockRepository.GenerateMock<IApplicationRepository>();
            repositoryMock.Stub(x => x.Get()).Return(mockedApplications);

            var applicationList = new FilteredApplicationCategory(null, repositoryMock);

            applicationList.Filter = "One";

            var actualApplications = applicationList.Applications;

            Assert.AreEqual(1, actualApplications.Count());
            Assert.AreEqual(mockedApplications[0].Name, actualApplications.First().Name);
        }

        [TestCase(new[] { "One Two", "Two", "Three" }, new[] { "One Two", "Two" }, "Two")]
        [TestCase(new[] { "OneTwo", "Three", "tootwoto" }, new[] { "OneTwo", "tootwoto" }, "Two")]
        [TestCase(new[] { "twoo", "tw o", "invalid" }, new[] { "twoo" }, "Two")]
        public void ProvidesApplicationWhenFilterMatchesNamePartially(string[] applicationNames, string[] expectedNames, string filter)
        {
            var mockedApplications = GetMockApplicationList(applicationNames);

            var repositoryMock = MockRepository.GenerateMock<IApplicationRepository>();
            repositoryMock.Stub(x => x.Get()).Return(mockedApplications);

            var applicationList = new FilteredApplicationCategory(null, repositoryMock);

            applicationList.Filter = filter;

            var actualApplications = applicationList.Applications;

            Assert.AreEqual(expectedNames.Length, actualApplications.Count());

            foreach (var currentExpectedName in expectedNames)
                Assert.IsTrue(actualApplications.Any(x => x.Name == currentExpectedName));
        }
    }
}