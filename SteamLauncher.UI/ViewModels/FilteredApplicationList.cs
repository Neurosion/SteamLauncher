using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using SteamLauncher.Domain;
using SteamLauncher.Domain.Data;

namespace SteamLauncher.UI.ViewModels
{
    public class FilteredApplicationList : IFilteredApplicationList
    {
        private IApplicationRepository _applicationRepository;
        private string _filter;
        
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value ?? string.Empty;
                    PropertyChanged.Notify();
                    FilterApplications();
                }
            }
        }
        
        public ICollection<IApplication> Applications { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public FilteredApplicationList(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
            _filter = string.Empty;
            Applications = new ObservableCollection<IApplication>();

            FilterApplications(); // Initial filtering with empty filter to populate applications
        }

        private void FilterApplications()
        {
            Applications.Clear();
            
            _applicationRepository.Get()
                                  .Where(x => string.IsNullOrEmpty(Filter) || x.Name.Contains(Filter))
                                  .ForEach(x => Applications.Add(x));
        }
    }
}