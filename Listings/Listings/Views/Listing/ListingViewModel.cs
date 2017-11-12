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
    public class ListingViewModel : ViewModel
    {
        private ListingFacade _listingFacade;


        private List<int> _years = new List<int>();
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


        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null) {
                    _saveCommand = new DelegateCommand(p => SaveListing());
                }

                return _saveCommand;
            }
        }


        public ListingViewModel(ListingFacade listingFacade)
        {
            _listingFacade = listingFacade;

            GenerateYears(_years);

            _selectedYear = DateTime.Now.Year;
            _selectedMonth = DateTime.Now.Month;
        }


        private void SaveListing()
        {
            Listing newListing = new Listing(SelectedYear, SelectedMonth);
            if (!string.IsNullOrEmpty(Name)) {
                newListing.Name = Name;
            }

            _listingFacade.Save(newListing);
        }


        private void GenerateYears(List<int> list)
        {
            int stopYear = DateTime.Now.Year - 5;
            for (int year = DateTime.Now.Year; year > stopYear; year--) {
                list.Add(year);
            }
        }
    }
}
