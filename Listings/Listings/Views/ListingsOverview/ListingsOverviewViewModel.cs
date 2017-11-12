using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingsOverviewViewModel : ViewModel
    {
        private ListingFacade _listingFacade;


        public ListingsOverviewViewModel(ListingFacade listingFacade)
        {
            _listingFacade = listingFacade;
        }
    }
}
