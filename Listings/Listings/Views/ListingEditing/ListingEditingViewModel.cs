using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Facades;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingEditingViewModel : ViewModel//, IDataErrorInfo
    {
        private string _basicWindowTitle;

        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                _listing = value;
                RaisePropertyChanged();
                RefreshEmployers();

                WindowTitle = string.Format("{0} [{1} {2} {3}]", _basicWindowTitle, Date.Months[12 - value.Month], value.Year, string.Format("- {0}", value.Name));

                _years.Clear();
                _years.Add(value.Year);
                _months.Clear();
                _months.Add(Date.Months[12 - value.Month]);
                SelectedYear = 0;
                SelectedMonth = 0;


                if (value.Employer == null || !_employers.Exists(e => e == value.Employer)) {
                    SelectedEmployer = _promptEmployer;
                } else {
                    SelectedEmployer = value.Employer;
                }

                Name = value.Name;
                HourlyWage = value.HourlyWage == null ? null : value.HourlyWage.ToString();

                Vacation = Listing.Vacation;
                Holiday = Listing.Holiday;
                SicknessHours = Listing.SicknessHours;
                VacationDays = Listing.VacationDays;
                Diets = Listing.Diets;
                PaidHolidays = Listing.PaidHolidays;
                Bonuses = Listing.Bonuses;
                Dollars = Listing.Dollars;
                Prepayment = Listing.Prepayment;
                Sickness = Listing.Sickness;
            }
        }


        private List<int> _years = new List<int>();
        public List<int> Years
        {
            get { return _years; }
        }


        private List<string> _months = new List<string>();
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


        private string _vacation;
        public string Vacation
        {
            get { return _vacation; }
            set { _vacation = value; }
        }


        private string _holiday;
        public string Holiday
        {
            get { return _holiday; }
            set { _holiday = value; }
        }


        private string _sicknessHours;
        public string SicknessHours
        {
            get { return _sicknessHours; }
            set { _sicknessHours = value; }
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


        private string _vacationDays;
        public string VacationDays
        {
            get { return _vacationDays; }
            set { _vacationDays = value; }
        }


        private string _diets;
        public string Diets
        {
            get { return _diets; }
            set { _diets = value; }
        }


        private string _paidHolidays;
        public string PaidHolidays
        {
            get { return _paidHolidays; }
            set { _paidHolidays = value; }
        }


        private string _bonuses;
        public string Bonuses
        {
            get { return _bonuses; }
            set { _bonuses = value; }
        }


        private string _dollars; // wtf? :D
        public string Dollars
        {
            get { return _dollars; }
            set { _dollars = value; }
        }


        private string _prepayment;
        public string Prepayment
        {
            get { return _prepayment; }
            set { _prepayment = value; }
        }


        private string _sickness;
        public string Sickness
        {
            get { return _sickness; }
            set { _sickness = value; }
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
        private EmployerFacade _employerFacade;

        
        public ListingEditingViewModel(ListingFacade listingFacade, EmployerFacade employerFacade, string windowTitle)
        {
            _listingFacade = listingFacade;
            _employerFacade = employerFacade;

            WindowTitle = windowTitle;
            _basicWindowTitle = windowTitle;

            _selectedEmployer = _promptEmployer;
        }


        private void RefreshEmployers()
        {
            _employers = _employerFacade.FindAllEmployers();
            _employers.Insert(0, _promptEmployer);
        }


        public delegate void ListingSaveHandler(object sender, ListingArgs args);
        public event ListingSaveHandler OnListingSuccessfullySaved;
        private void SaveListing()
        {
            if (Listing == null) {
                throw new Exception("No Listing is set!");
            }

            Listing.Name = string.IsNullOrEmpty(Name) ? null : Name.Trim();
            Listing.HourlyWage = _hourlyWage;
            Listing.Employer = _selectedEmployer == _promptEmployer ? null : _selectedEmployer;
            Listing.Vacation = Vacation;
            Listing.Holiday = Holiday;
            Listing.SicknessHours = SicknessHours;
            Listing.VacationDays = VacationDays;
            Listing.Diets = Diets;
            Listing.PaidHolidays = PaidHolidays;
            Listing.Bonuses = Bonuses;
            Listing.Dollars = Dollars;
            Listing.Prepayment = Prepayment;
            Listing.Sickness = Sickness;

            _listingFacade.Save(Listing);

            ListingSaveHandler handler = OnListingSuccessfullySaved;
            if (handler != null) {
                handler(this, new ListingArgs(Listing));
            }
        }


        public delegate void ReturnBackHandler(object sender, ListingArgs args);
        public event ReturnBackHandler OnReturnBackClicked;
        private void ReturnBack()
        {
            ReturnBackHandler handler = OnReturnBackClicked;
            if (handler != null) {
                handler(this, new ListingArgs(Listing));
            }
        }


        // -----


        /*public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName) {
                    case nameof(HourlyWage):
                        if (!string.IsNullOrEmpty(HourlyWage)) {
                            if (int.Parse(HourlyWage) < 0) {
                                return "Hodinová mzda musí být větší než 0";
                            }
                        }
                        break;
                }

                return string.Empty;
            }
        }*/
    }
}
