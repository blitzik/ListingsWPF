using Caliburn.Micro;
using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingEditingViewModelFactory
    {
        private readonly IEventAggregator _eventaggregator;
        private readonly ListingFacade _listingFacade;
        private readonly EmployerFacade _employerFacade;


        public ListingEditingViewModelFactory(IEventAggregator eventaggregator, ListingFacade listingFacade, EmployerFacade employerFacade)
        {
            _eventaggregator = eventaggregator;
            _listingFacade = listingFacade;
            _employerFacade = employerFacade;
        }


        public ListingEditingViewModel Create(string windowTitle)
        {
            return new ListingEditingViewModel(_eventaggregator, windowTitle, _listingFacade, _employerFacade);
        }
    }
}
