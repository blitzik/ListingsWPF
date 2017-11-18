using Db4objects.Db4o;
using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Facades;
using Listings.Services;
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
                    _listingsOverviewViewModel.OnListingSelected += (object sender, SelectedListingArgs args) => {
                        ListingDetailViewModel.Listing = args.SelectedListing;
                        ChangeView(nameof(ListingDetailViewModel));
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
                }

                return _listingDetailViewModel;
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
            switch (viewCode) {
                case nameof(ListingsOverviewViewModel):
                    CurrentViewModel = ListingsOverviewViewModel;
                    break;

                case nameof(ListingViewModel):
                    CurrentViewModel = ListingViewModel;
                    break;

                case nameof(ListingDetailViewModel):
                    CurrentViewModel = ListingDetailViewModel;
                    break;
            }

            WindowTitle = CurrentViewModel.WindowTitle;
        }
    }
}
