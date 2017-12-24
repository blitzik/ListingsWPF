using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Facades;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingsOverviewViewModel : ViewModel
    {
        private ListingFacade _listingFacade;


        private DelegateCommand<Listing> _openListingCommand;
        public DelegateCommand<Listing> OpenListingCommand
        {
            get
            {
                if (_openListingCommand == null) {
                    _openListingCommand = new DelegateCommand<Listing>(p => OpenListing(p));
                }

                return _openListingCommand;
            }
        }


        private ObservableCollection<Listing> _listings;
        public ObservableCollection<Listing> Listings
        {
            get { return _listings; }
            set {
                _listings = value;
                RaisePropertyChanged();
            }
        }


        private List<int> _years = Date.GetYears(2010, "DESC");
        public List<int> Years
        {
            get { return _years; }
        }


        private List<string> _months = new List<string>(Date.Months);
        public List<string> Months
        {
            get { return _months; }
        }


        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                RaisePropertyChanged();

                LoadListings(SelectedYear);
            }
        }


        public ListingsOverviewViewModel(ListingFacade listingFacade, string windowTitle)
        {
            _listingFacade = listingFacade;

            Listings = new ObservableCollection<Listing>(listingFacade.FindListings(DateTime.Now.Year));

            _selectedYear = DateTime.Now.Year;
            _windowTitle = windowTitle;
        }


        public ListingsOverviewViewModel(ListingFacade listingFacade) : this(listingFacade, null)
        {
        }


        private void LoadListings(int year)
        {
            Listings = new ObservableCollection<Listing>(_listingFacade.FindListings(year));
        }


        public void RefreshList()
        {
            LoadListings(SelectedYear);
        }


        public delegate void ListingSelectionHandler(object sender, ListingArgs args);
        public event ListingSelectionHandler OnListingSelected;
        private void OpenListing(Listing listing)
        {
            ListingSelectionHandler handler = OnListingSelected;
            if (handler != null) {
                handler(this, new ListingArgs(listing));
            }
        }


    }
}
