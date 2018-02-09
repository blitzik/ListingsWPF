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
        private readonly ListingFacade _listingFacade;


        public ListingsOverviewViewModelFactory(ListingFacade listingFacade)
        {
            _listingFacade = listingFacade;
        }


        public ListingsOverviewViewModel Create(string windowTitle)
        {
            return new ListingsOverviewViewModel(windowTitle, _listingFacade);
        }
    }
}
