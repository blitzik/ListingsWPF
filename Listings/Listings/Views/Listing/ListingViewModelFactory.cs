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
        private readonly ListingDetailViewModelFactory _listingDetailViewModelFactory;
        private readonly ListingFacade _listingFacade;
        private readonly EmployerFacade _employerFacade;


        public ListingViewModelFactory(
            IEventAggregator eventAggregator,
            ListingDetailViewModelFactory listingDetailViewModelFactory,
            ListingFacade listingFacade,
            EmployerFacade employerFacade
        ) {
            _eventAggregator = eventAggregator;
            _listingDetailViewModelFactory = listingDetailViewModelFactory;
            _listingFacade = listingFacade;
            _employerFacade = employerFacade;
        }


        public ListingViewModel Create(string windowTitle)
        {
            ListingViewModel vm = new ListingViewModel(_eventAggregator, _listingFacade, _employerFacade);
            vm.BaseWindowTitle = windowTitle;

            return vm;
        }
    }
}
