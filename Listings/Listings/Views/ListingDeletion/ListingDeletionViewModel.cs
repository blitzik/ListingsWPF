using Caliburn.Micro;
using Listings.Commands;
using Listings.Domain;
using Listings.Facades;
using Listings.Messages;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingDeletionViewModel : BaseScreen, IHandle<ListingMessage>
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                _listing = value;
                if (value == null) {
                    WindowTitle.Text = BaseWindowTitle;

                } else {
                    WindowTitle.Text = string.Format("{0} [{1} {2} {3}]", BaseWindowTitle, Date.Months[12 - value.Month], value.Year, string.Format("- {0}", value.Name));
                }
            }
        }


        private string _confirmationText;
        public string ConfirmationText
        {
            get { return _confirmationText; }
            set {
                _confirmationText = value != null ? value.ToLower() : null;
                NotifyOfPropertyChange(() => ConfirmationText);
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


        private ListingFacade _listingFacade;

        public ListingDeletionViewModel(ListingFacade listingFacade)
        {
            BaseWindowTitle = "Odstranění výčetky";

            _listingFacade = listingFacade;            
        }


        private void DeleteListing()
        {
            _listingFacade.DeleteListing(Listing);
            Listing = null;
            ConfirmationText = null;

            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(ListingsOverviewViewModel)));
        }


        private void ReturnBack()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(ListingDetailViewModel)));
        }


        // -----


        public void Handle(ListingMessage message)
        {
            Listing = message.Listing;
        }
    }
}
