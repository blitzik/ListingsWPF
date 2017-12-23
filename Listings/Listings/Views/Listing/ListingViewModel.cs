using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Facades;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingViewModel : ViewModel
    {
        private ListingFacade _listingFacade;


        private List<int> _years = Date.GetLastYears(3);
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


        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }


        private DelegateCommand<object> _saveCommand;
        public DelegateCommand<object> SaveCommand
        {
            get
            {
                if (_saveCommand == null) {
                    _saveCommand = new DelegateCommand<object>(p => SaveListing());
                }

                return _saveCommand;
            }
        }


        public ListingViewModel(ListingFacade listingFacade, string windowTitle)
        {
            _listingFacade = listingFacade;

            _selectedYear = DateTime.Now.Year;
            _selectedMonth = DateTime.Now.Month;
            _windowTitle = windowTitle;
        }


        public delegate void NewListingSaveHandler(object sender, ListingArgs args);
        public event NewListingSaveHandler OnListingCreation;
        private void SaveListing()
        {
            Listing newListing = new Listing(SelectedYear, SelectedMonth);
            if (!string.IsNullOrEmpty(Name)) {
                newListing.Name = Name;
            }

            _listingFacade.Save(newListing);

            NewListingSaveHandler handler = OnListingCreation;
            if (handler != null) {
                handler(this, new ListingArgs(newListing));
            }
        }

    }
}
