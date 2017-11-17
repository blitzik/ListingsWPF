using Listings.Domain;
using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingDetailViewModel : ViewModel
    {
        private readonly ListingFacade _listingFacade;


        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                _listing = value;
                RaisePropertyChanged();
            }
        }


        public ListingDetailViewModel(ListingFacade listingFacade, string windowTitle)
        {
            _listingFacade = listingFacade;
            WindowTitle = windowTitle;
        }



    }
}
