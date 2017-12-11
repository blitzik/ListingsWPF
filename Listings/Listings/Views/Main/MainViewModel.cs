using Db4objects.Db4o;
using Listings.Commands;
using Listings.EventArguments;
using Listings.Domain;
using Listings.Facades;
using Listings.Services;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Listings.Views
{
    public class MainViewModel : ViewModel
    {
        private ViewModel _currentViewModel;
        public ViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (_currentViewModel == value) return;

                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }


        private DelegateCommand _navigationCommand;
        public DelegateCommand NavigationCommand
        {
            get
            {
                if (_navigationCommand == null) {
                    _navigationCommand = new DelegateCommand(p => ChangeView((string)p));
                }

                return _navigationCommand;
            }
        }


        private ListingsOverviewViewModel _listingsOverviewViewModel;
        private ListingsOverviewViewModel ListingsOverviewViewModel
        {
            get
            {
                if (_listingsOverviewViewModel == null) {
                    _listingsOverviewViewModel = new ListingsOverviewViewModel(_listingFacade, "Přehled výčetek");
                    _listingsOverviewViewModel.OnListingSelected += (object sender, ListingArgs args) => {
                        ListingDetailViewModel.Listing = args.Listing;
                        ChangeView(nameof(ListingDetailViewModel));

                        /*IObjectContainer db = _listingFacade._db;
                        args.SelectedListing.AddItem(3, "Lorem ipsum dolor sit amet consecteteur Lorem ipsum dolor sit amet consecteteur Lorem ipsum dolor sit amet consecteteur", new Time("6:00"), new Time("16:00"), new Time("11:00"), new Time("12:00"));
                        db.Store(args.SelectedListing);
                        db.Commit();*/
                    };
                }

                return _listingsOverviewViewModel;
            }
        }


        private ListingViewModel _listingViewModel;
        private ListingViewModel ListingViewModel
        {
            get
            {
                if (_listingViewModel == null) {
                    _listingViewModel = new ListingViewModel(_listingFacade, "Nová výčetka");
                    _listingViewModel.OnListingCreation += (object sender, ListingArgs args) => {
                        ListingDetailViewModel.Listing = args.Listing;
                        ChangeView(nameof(ListingDetailViewModel));
                    };
                }

                return _listingViewModel;
            }
        }


        private ListingDetailViewModel _listingDetailViewModel;
        private ListingDetailViewModel ListingDetailViewModel
        {
            get
            {
                if (_listingDetailViewModel == null) {
                    _listingDetailViewModel = new ListingDetailViewModel(_listingFacade, "Detail výčetky");
                    _listingDetailViewModel.OnListingItemClicked += (object sender, SelectedDayItemArgs args) => {
                        _listingItemViewModel = new ListingItemViewModel(_listingFacade, args.DayItem, "Detail položky");
                        _listingItemViewModel.OnListingItemSaving += (object s, ListingItemArgs a) => {
                            _listingDetailViewModel.ReplaceDayInListBy(a.ListingItem);
                            ChangeView(nameof(ListingDetailViewModel));
                        };
                        ChangeView(nameof(ListingItemViewModel));
                    };

                    _listingDetailViewModel.OnListingDeletionClicked += (object sender, ListingArgs args) => {
                        ListingDeletionViewModel.Listing = args.Listing;
                        ChangeView(nameof(ListingDeletionViewModel));
                    };
                }

                return _listingDetailViewModel;
            }
        }


        private ListingDeletionViewModel _listingDeletionViewModel;
        public ListingDeletionViewModel ListingDeletionViewModel
        {
            get
            {
                if (_listingDeletionViewModel == null) {
                    _listingDeletionViewModel = new ListingDeletionViewModel(_listingFacade, "Odstranění výčetky");
                    _listingDeletionViewModel.OnDeletedListing += (object sender, EventArgs args) => {
                        ChangeView(nameof(ListingsOverviewViewModel));
                    };
                    _listingDeletionViewModel.OnReturnBackClicked += (object sender, EventArgs args) => {
                        ChangeView(nameof(ListingDetailViewModel));
                    };
                }

                return _listingDeletionViewModel;
            }
        }


        private ListingItemViewModel _listingItemViewModel;
        private ListingItemViewModel ListingItemViewModel
        {
            get
            {
                return _listingItemViewModel;
            }
        }


        private ListingFacade _listingFacade;


        public MainViewModel(ListingFacade listingFacade)
        {
            _listingFacade = listingFacade;

            ChangeView(nameof(ListingsOverviewViewModel));
        }


        private void ChangeView(string viewCode)
        {
            // we dont want the same menu item to be clicked more than once
            if (CurrentViewModel != null && CurrentViewModel.GetType().Name == viewCode) {
                return;
            }

            switch (viewCode) {
                case nameof(ListingsOverviewViewModel):
                    ListingsOverviewViewModel.RefreshList();
                    CurrentViewModel = ListingsOverviewViewModel;
                    break;

                case nameof(ListingViewModel):
                    CurrentViewModel = ListingViewModel;
                    break;

                case nameof(ListingDetailViewModel):
                    CurrentViewModel = ListingDetailViewModel;
                    break;

                case nameof(ListingItemViewModel):
                    CurrentViewModel = ListingItemViewModel;
                    break;

                case nameof(ListingDeletionViewModel):
                    CurrentViewModel = ListingDeletionViewModel;
                    break;
            }

            WindowTitle = CurrentViewModel.WindowTitle;
        }
    }
}
