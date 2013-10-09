using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Text.RegularExpressions;
using SteamLauncher.Domain;
using SteamLauncher.Domain.Data;

namespace SteamLauncher.UI.Core
{
    public class FilteredApplicationCategory : IFilteredApplicationCategory
    {
        private IList<IApplication> _applications;
        private IApplicationRepository _applicationRepository;
        private string _filter;

        public string Name { get; private set; }
        
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value ?? string.Empty;
                    FilterApplications();
                }
            }
        }

        public IEnumerable<IApplication> Applications
        {
            get { return _applications; }
        }

        public FilteredApplicationCategory(string name, IApplicationRepository applicationRepository)
        {
            this.Name = name;
            _applicationRepository = applicationRepository;
            _filter = string.Empty;

            _applications = new ObservableCollection<IApplication>();

            FilterApplications(); // Initial filtering to populate the applications
        }

        private void FilterApplications()
        {
            _applications.Clear();

            _applicationRepository.Get()
                                  .Where(x => string.IsNullOrEmpty(Filter) || DoesNameMatchFilter(x.Name))
                                  .OrderBy(x => x.Name)
                                  .ForEach(x => _applications.Add(x));
        }

        private bool DoesNameMatchFilter(string name)
        {
            var filterParts = Filter.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var pattern = string.Format("({0})+", string.Join("|", filterParts));

            var doesMatch = Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            return doesMatch;
        }
    }
}