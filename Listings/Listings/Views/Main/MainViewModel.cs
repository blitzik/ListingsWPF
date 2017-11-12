using Db4objects.Db4o;
using Listings.Commands;
using Listings.Facades;
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


        private ICommand _navigationCommand;
        public ICommand NavigationCommand
        {
            get
            {
                if (_navigationCommand == null) {
                    _navigationCommand = new DelegateCommand(p => OnNav((string)p));
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
                    _listingsOverviewViewModel = new ListingsOverviewViewModel(_listingFacade);
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
                    _listingViewModel = new ListingViewModel(_listingFacade);
                }

                return _listingViewModel;
            }
        }


        private ListingFacade _listingFacade;


        public MainViewModel(ListingFacade listingFacade)
        {
            _listingFacade = listingFacade;
        }


        private void OnNav(string destination)
        {
            switch (destination) {
                case "listingsOverview":
                    CurrentViewModel = ListingsOverviewViewModel;
                    break;

                case "newListing":
                    CurrentViewModel = ListingViewModel;
                    break;
            }
        }
    }
}
