using Caliburn.Micro;
using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingsOverviewViewModelFactory
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ListingFacade _listingFacade;
        private readonly ListingDetailViewModelFactory _listingDetailViewModelFactory;

        public ListingsOverviewViewModelFactory(
            IEventAggregator eventAggregator,
            ListingFacade listingFacade,
            ListingDetailViewModelFactory listingDetailViewModelFactory
        ) {
            _eventAggregator = eventAggregator;
            _listingFacade = listingFacade;
            _listingDetailViewModelFactory = listingDetailViewModelFactory;
        }


        public ListingsOverviewViewModel Create(string windowTitle)
        {
            return new ListingsOverviewViewModel(_eventAggregator, windowTitle, _listingFacade, _listingDetailViewModelFactory);
        }
    }
}
