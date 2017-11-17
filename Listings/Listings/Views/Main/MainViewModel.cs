using Db4objects.Db4o;
using Listings.Commands;
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


        private ListingsOverviewViewModel _listingsOverviewViewModel;
        private ListingsOverviewViewModel ListingsOverviewViewModel
        {
            get
            {
                if (_listingsOverviewViewModel == null) {
                    _listingsOverviewViewModel = new ListingsOverviewViewModel(_listingFacade, "Přehled výčetek");
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


        private ListingFacade _listingFacade;


        private List<NavigationItem> _navigation;
        public List<NavigationItem> Navigation
        {
            get { return _navigation; }
            set
            {
                _navigation = value;
                RaisePropertyChanged();
            }
        }


        private NavigationItem _selectedNavigationItem;
        public NavigationItem SelectedNavigationItem
        {
            get { return _selectedNavigationItem; }
            set
            {
                _selectedNavigationItem = value;
                RaisePropertyChanged();

                OnNav(_selectedNavigationItem);
            }
        }


        public MainViewModel(ListingFacade listingFacade)
        {
            _listingFacade = listingFacade;

            NavigationItem defaultItem = new NavigationItem("listingsOverview", "Přehled výčetek");

            _navigation = new List<NavigationItem>();
            _navigation.Add(defaultItem);
            _navigation.Add(new NavigationItem("newListing", "Nová výčetka"));
            _navigation.Add(new NavigationItem("employersManagement", "Správa zaměstnavatelů"));
            _navigation.Add(new NavigationItem("settings", "Nastavení"));

            SelectedNavigationItem = defaultItem;
        }


        private void OnNav(NavigationItem navItem)
        {
            switch (navItem.Index) {
                case "listingsOverview":
                    CurrentViewModel = ListingsOverviewViewModel;
                    break;

                case "newListing":
                    CurrentViewModel = ListingViewModel;
                    break;
            }

            WindowTitle = CurrentViewModel.WindowTitle;
        }
    }
}
