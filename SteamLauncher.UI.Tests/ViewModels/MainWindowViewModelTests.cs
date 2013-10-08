using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.UI.ViewModels;
using SteamLauncher.UI.Core;

namespace SteamLauncher.UI.Tests.ViewModels
{
    public class MainWindowViewModelTests
    {
        [Test]
        public void NotifiesPropertyChangedWhenFilterChanges()
        {
            var factoryMock = MockRepository.GenerateMock<IFilteredApplicationCategoryFactory>();
            factoryMock.Stub(x => x.Build()).Return(new IFilteredApplicationCategory[] { });
            var viewModel = new MainWindowViewModel(factoryMock);

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
            var viewModel = new MainWindowViewModel(factoryMock);

            categoryMock.AssertWasNotCalled(x => x.Filter);

            var filterValue = "test";
            viewModel.Filter = filterValue;

            categoryMock.AssertWasCalled(x => x.Filter, c => c.Repeat.Once().SetPropertyWithArgument(filterValue));
        }
    }
}
