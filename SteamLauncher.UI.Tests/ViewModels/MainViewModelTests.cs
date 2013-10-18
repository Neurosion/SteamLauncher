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
    public class MainViewModelTests
    {
        [Test]
        public void NotifiesPropertyChangedWhenFilterChanges()
        {
            var factoryMock = MockRepository.GenerateMock<IFilteredApplicationCategoryFactory>();
            factoryMock.Stub(x => x.Build()).Return(new IFilteredApplicationCategory[] { });
            var notifyIconMock = MockRepository.GenerateMock<INotifyIcon>();
            var viewModel = new MainWindowViewModel(null, factoryMock, null, notifyIconMock);

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
            var notifyIconMock = MockRepository.GenerateMock<INotifyIcon>();
            var viewModel = new MainWindowViewModel(null, factoryMock, null, notifyIconMock);

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
            var notifyIconMock = MockRepository.GenerateMock<INotifyIcon>();
            var viewModel = new MainWindowViewModel(steamProxyMock, factoryMock, null, notifyIconMock);

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
            var notifyIconMock = MockRepository.GenerateMock<INotifyIcon>();
            var viewModel = new MainWindowViewModel(steamProxyMock, factoryMock, null, notifyIconMock);

            viewModel.Launch(applicationMock);

            steamProxyMock.AssertWasCalled(x => x.LaunchApp(applicationMock.Id), c => c.Repeat.Once());
        }

        [TestCase(NotifyIconActions.ShowMainUI, true)]
        [TestCase(NotifyIconActions.ExitApplication, false)]
        [TestCase(NotifyIconActions.ShowSettingsUI, false)]
        public void IsVisibleIsTrueOnlyWhenNotifyIconNotifiesWithShowMainUI(NotifyIconActions action, bool expectedVisibility)
        {
            var factoryMock = MockRepository.GenerateMock<IFilteredApplicationCategoryFactory>();
            factoryMock.Stub(x => x.Build()).Return(new IFilteredApplicationCategory[] { });
            var notifyIconMock = MockRepository.GenerateMock<INotifyIcon>();
            var viewModel = new MainWindowViewModel(null, factoryMock, null, notifyIconMock);

            notifyIconMock.Raise(x => x.ItemSelected += delegate { }, action.GetDescription());

            Assert.AreEqual(expectedVisibility, viewModel.IsVisible);
        }
    }
}
