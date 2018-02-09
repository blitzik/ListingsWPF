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
        private readonly ListingFacade _listingFacade;


        public ListingDetailViewModelFactory(ListingFacade listingFacade)
        {
            _listingFacade = listingFacade;
        }


        public ListingDetailViewModel Create(string windowTitle)
        {
            return new ListingDetailViewModel(windowTitle, _listingFacade);
        }
    }
}
