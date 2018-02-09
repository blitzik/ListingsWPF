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
        private EmployerFacade _employerFacade;


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


        private List<Employer> _employers;
        public List<Employer> Employers
        {
            get
            {
                return _employers;
            }
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

        private Employer _promptEmployer = new Employer("Bez zaměstnavatele");
        private Employer _selectedEmployer;
        public Employer SelectedEmployer
        {
            get { return _selectedEmployer; }
            set
            {
                _selectedEmployer = value;
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


        private int? _hourlyWage;
        public string HourlyWage
        {
            get { return _hourlyWage.ToString(); }
            set
            {
                int result;
                if (!int.TryParse(value, out result)) {
                    _hourlyWage = null;
                    RaisePropertyChanged();
                    return;
                }

                if (result <= 0) {
                    _hourlyWage = null;

                } else {
                    _hourlyWage = result;
                }

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


        public ListingViewModel(string windowTitle, ListingFacade listingFacade, EmployerFacade employerFacade) : base(windowTitle)
        {
            _listingFacade = listingFacade;
            _employerFacade = employerFacade;

            SelectedYear = DateTime.Now.Year;
            SelectedMonth = DateTime.Now.Month;

            SelectedEmployer = _promptEmployer;
        }


        public override void Reset()
        {
            _employers = _employerFacade.FindAllEmployers();
            _employers.Insert(0, _promptEmployer);

            SelectedYear = DateTime.Now.Year;
            SelectedMonth = DateTime.Now.Month;
            SelectedEmployer = _promptEmployer;
            HourlyWage = null;
            Name = null;
        }


        public delegate void NewListingSaveHandler(object sender, ListingArgs args);
        public event NewListingSaveHandler OnListingCreation;
        private void SaveListing()
        {
            Name = string.IsNullOrEmpty(Name) ? null : Name.Trim();

            Listing newListing = new Listing(SelectedYear, SelectedMonth);
            newListing.Name = Name;

            if (_selectedEmployer != _promptEmployer) {
                newListing.Employer = _selectedEmployer;
            }

            if (_hourlyWage != null && _hourlyWage > 0) {
                newListing.HourlyWage = _hourlyWage;
            }

            _listingFacade.Save(newListing);

            SelectedEmployer = _promptEmployer;
            HourlyWage = null;
            Name = null;
            SelectedYear = DateTime.Now.Year;
            SelectedMonth = DateTime.Now.Month;

            NewListingSaveHandler handler = OnListingCreation;
            if (handler != null) {
                handler(this, new ListingArgs(newListing));
            }
        }

    }
}
