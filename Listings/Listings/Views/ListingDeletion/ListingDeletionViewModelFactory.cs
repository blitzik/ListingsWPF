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
        private readonly ListingFacade _listingFacade;


        public ListingDeletionViewModelFactory(ListingFacade listingFacade)
        {
            _listingFacade = listingFacade;
        }


        public ListingDeletionViewModel Create(string windowTitle)
        {
            return new ListingDeletionViewModel(windowTitle, _listingFacade);
        }
    }
}
