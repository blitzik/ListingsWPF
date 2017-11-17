using Listings.Commands;
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


        private DelegateCommand _backToOverviewCommand;
        public DelegateCommand BackToOverviewCommand
        {
            get
            {
                if (_backToOverviewCommand == null) {
                    _backToOverviewCommand = new DelegateCommand(p => DisplayListingsOverview());
                }

                return _backToOverviewCommand;
            }
        }


        public ListingDetailViewModel(ListingFacade listingFacade, string windowTitle)
        {
            _listingFacade = listingFacade;
            WindowTitle = windowTitle;
        }


        public delegate void SwitchViewHandler(object sender, EventArgs args);
        public event SwitchViewHandler OnDisplayListingsOverviewClicked;
        private void DisplayListingsOverview()
        {
            SwitchViewHandler handler = OnDisplayListingsOverviewClicked;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

    }
}
