using Caliburn.Micro;
using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingDeletionViewModelFactory
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ListingFacade _listingFacade;


        public ListingDeletionViewModelFactory(IEventAggregator eventAggregator, ListingFacade listingFacade)
        {
            _eventAggregator = eventAggregator;
            _listingFacade = listingFacade;
        }


        public ListingDeletionViewModel Create(string windowTitle)
        {
            ListingDeletionViewModel vm = new ListingDeletionViewModel(_eventAggregator, _listingFacade);
            vm.BaseWindowTitle = windowTitle;

            return vm;
        }
    }
}
