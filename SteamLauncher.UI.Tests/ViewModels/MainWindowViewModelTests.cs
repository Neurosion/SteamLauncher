using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.UI.ViewModels;
using SteamLauncher.UI.Core;
using SteamLauncher.Domain;

namespace SteamLauncher.UI.Tests.ViewModels
{
    public class MainWindowViewModelTests
    {
        [Test]
        public void NotifiesPropertyChangedWhenFilterChanges()
        {
            var factoryMock = MockRepository.GenerateMock<IFilteredApplicationCategoryFactory>();
            factoryMock.Stub(x => x.Build()).Return(new IFilteredApplicationCategory[] { });
            var viewModel = new MainWindowViewModel(null, factoryMock);

            var wasPropertyChangedCalled = false;

            viewModel.PropertyChanged += (s, e) =>
                {
                    wasPropertyChangedCalled = true;
                    Assert.AreEqual("Filter", e.PropertyName);
                };

            Assert.IsFalse(wasPropertyChangedCalled);

            viewModel.Filter = "test";

            Assert.IsTrue(wasPropertyChangedCalled);
        }

        [Test]
        public void UpdatesFilteredApplicationCategoriesWhenFilterChanges()
        {
            var categoryMock = MockRepository.GenerateMock<IFilteredApplicationCategory>();
            var factoryMock = MockRepository.GenerateMock<IFilteredApplicationCategoryFactory>();
            factoryMock.Stub(x => x.Build()).Return(new [] { categoryMock });
            var viewModel = new MainWindowViewModel(null, factoryMock);

            categoryMock.AssertWasNotCalled(x => x.Filter);

            var filterValue = "test";
            viewModel.Filter = filterValue;

            categoryMock.AssertWasCalled(x => x.Filter, c => c.Repeat.Once().SetPropertyWithArgument(filterValue));
        }

        [Test]
        public void DoesNotCallSteamProxyWhenApplicationIsNull()
        {
            var steamProxyMock = MockRepository.GenerateMock<ISteamProxy>();
            var factoryMock = MockRepository.GenerateMock<IFilteredApplicationCategoryFactory>();
            var viewModel = new MainWindowViewModel(steamProxyMock, factoryMock);

            viewModel.Launch(null);

            steamProxyMock.AssertWasNotCalled(x => x.LaunchApp(Arg<int>.Is.Anything));
        }

        [Test]
        public void CallsSteamProxyWithIdOfProvidedApplication()
        {
            var steamProxyMock = MockRepository.GenerateMock<ISteamProxy>();
            var applicationMock = MockRepository.GenerateMock<IApplication>();
            applicationMock.Stub(x => x.Id).Return(8);
            var factoryMock = MockRepository.GenerateMock<IFilteredApplicationCategoryFactory>();
            var viewModel = new MainWindowViewModel(steamProxyMock, factoryMock);

            viewModel.Launch(applicationMock);

            steamProxyMock.AssertWasCalled(x => x.LaunchApp(applicationMock.Id), c => c.Repeat.Once());
        }
    }
}
