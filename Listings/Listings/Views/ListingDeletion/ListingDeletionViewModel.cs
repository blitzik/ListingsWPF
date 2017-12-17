using Listings.Commands;
using Listings.Domain;
using Listings.Facades;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingDeletionViewModel : ViewModel
    {
        private ListingFacade _listingFacade;

        private string _basicWindowTitle;


        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                _listing = value;
                if (value == null) {
                    WindowTitle = _basicWindowTitle;

                } else {
                    WindowTitle = string.Format("{0} [{1} {2} {3}]", _basicWindowTitle, Date.Months[12 - value.Month], value.Year, string.Format("- {0}", value.Name));
                }
            }
        }


        private string _confirmationText;
        public string ConfirmationText
        {
            get { return _confirmationText; }
            set {
                _confirmationText = value != null ? value.ToLower() : null;
                RaisePropertyChanged();
                DeleteListingCommand.RaiseCanExecuteChanged();
            }
        }


        private DelegateCommand<object> _deleteListingCommand;
        public DelegateCommand<object> DeleteListingCommand
        {
            get
            {
                if (_deleteListingCommand == null) {
                    _deleteListingCommand = new DelegateCommand<object>(
                        p => DeleteListing(),
                        p => _confirmationText == "odstranit"
                    );
                }

                return _deleteListingCommand;
            }
        }


        private DelegateCommand<object> _returnBackCommand;
        public DelegateCommand<object> ReturnBackCommand
        {
            get
            {
                if (_returnBackCommand == null) {
                    _returnBackCommand = new DelegateCommand<object>(p => ReturnBack());
                }

                return _returnBackCommand;
            }
        }


        public ListingDeletionViewModel(ListingFacade listingFacade, string windowTitle)
        {
            _listingFacade = listingFacade;

            _basicWindowTitle = windowTitle;
        }


        public delegate void DeletetionListingHandler(object sender, EventArgs args);
        public event DeletetionListingHandler OnDeletedListing;
        private void DeleteListing()
        {
            _listingFacade.DeleteListing(Listing);
            Listing = null;
            ConfirmationText = null;

            DeletetionListingHandler handler = OnDeletedListing;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }


        public delegate void ReturnBackHandler(object sender, EventArgs args);
        public event ReturnBackHandler OnReturnBackClicked;
        private void ReturnBack()
        {
            ReturnBackHandler handler = OnReturnBackClicked;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
