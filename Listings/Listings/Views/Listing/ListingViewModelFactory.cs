using Caliburn.Micro;
using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingViewModelFactory
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ListingFacade _listingFacade;
        private readonly EmployerFacade _employerFacade;


        public ListingViewModelFactory(IEventAggregator eventAggregator, ListingFacade listingFacade, EmployerFacade employerFacade)
        {
            _eventAggregator = eventAggregator;
            _listingFacade = listingFacade;
            _employerFacade = employerFacade;
        }


        public ListingViewModel Create(string windowTitle)
        {
            return new ListingViewModel(_eventAggregator, windowTitle, _listingFacade, _employerFacade);
        }
    }
}
