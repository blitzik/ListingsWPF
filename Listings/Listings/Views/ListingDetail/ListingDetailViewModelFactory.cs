using Caliburn.Micro;
using Listings.Domain;
using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingDetailViewModelFactory
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ListingFacade _listingFacade;
        private readonly ListingItemViewModelFactory _listingItemViewModelFactory;


        public ListingDetailViewModelFactory(IEventAggregator eventAggregator, ListingFacade listingFacade, ListingItemViewModelFactory listingItemViewModelFactory)
        {
            _eventAggregator = eventAggregator;
            _listingFacade = listingFacade;
            _listingItemViewModelFactory = listingItemViewModelFactory;
        }


        public ListingDetailViewModel Create(string windowTitle, Listing listing)
        {
            ListingDetailViewModel vm = new ListingDetailViewModel(_eventAggregator, _listingFacade);
            vm.BaseWindowTitle = windowTitle;

            return vm;
        }
    }
}
