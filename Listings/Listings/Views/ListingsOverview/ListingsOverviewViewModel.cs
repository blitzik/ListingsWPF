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
    public class ListingsOverviewViewModel : ViewModel
    {
        private ListingFacade _listingFacade;


        private List<Listing> _listings;
        public List<Listing> Listings
        {
            get { return _listings; }
            set { _listings = value; }
        }


        private List<int> _years = Date.GetLastYears(5);
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
            }
        }


        private int _selectedMonth;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                RaisePropertyChanged();
            }
        }


        public ListingsOverviewViewModel(ListingFacade listingFacade, string windowTitle)
        {
            _listingFacade = listingFacade;

            Listings = listingFacade.FindListings();

            _selectedYear = DateTime.Now.Year;
            _selectedMonth = DateTime.Now.Month;
            _windowTitle = windowTitle;
        }


        public ListingsOverviewViewModel(ListingFacade listingFacade) : this(listingFacade, null)
        {
        }


    }
}
