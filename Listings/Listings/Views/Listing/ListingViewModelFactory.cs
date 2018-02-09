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
        private readonly ListingFacade _listingFacade;
        private readonly EmployerFacade _employerFacade;


        public ListingViewModelFactory(ListingFacade listingFacade, EmployerFacade employerFacade)
        {
            _listingFacade = listingFacade;
            _employerFacade = employerFacade;
        }


        public ListingViewModel Create(string windowTitle)
        {
            return new ListingViewModel(windowTitle, _listingFacade, _employerFacade);
        }
    }
}
