using System;
using System.Collections.Generic;
using System.Linq;
using SteamLauncher.Domain.Data;

namespace SteamLauncher.UI.Core
{
    public class FilteredApplicationCategoryFactory : IFilteredApplicationCategoryFactory
    {
        private IApplicationRepository _applicationRepository;

        public FilteredApplicationCategoryFactory(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public IEnumerable<IFilteredApplicationCategory> Build()
        {
            return new[] { Build("All") };
        }

        public IFilteredApplicationCategory Build(string name)
        {
            var category = new FilteredApplicationCategory(name, _applicationRepository);
            return category;
        }
    }
}