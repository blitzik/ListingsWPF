using Caliburn.Micro;
using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Facades;
using Listings.Messages;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingEditingViewModel : BaseScreen, IHandle<ListingMessage>
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                _listing = value;
                NotifyOfPropertyChange(() => Listing);
                WindowTitle.Text = string.Format("{0} [{1} {2} {3}]", BaseWindowTitle, Date.Months[12 - value.Month], value.Year, string.Format("- {0}", value.Name));             

                Reset(value);
            }
        }


        private ObservableCollection<int> _years = new ObservableCollection<int>();
        public ObservableCollection<int> Years
        {
            get { return _years; }
        }


        private ObservableCollection<string> _months = new ObservableCollection<string>();
        public ObservableCollection<string> Months
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
                NotifyOfPropertyChange(() => SelectedYear);
            }
        }


        private int _selectedMonth;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                NotifyOfPropertyChange(() => SelectedMonth);
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
                NotifyOfPropertyChange(() => SelectedEmployer);
            }
        }


        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }


        private string _vacation;
        public string Vacation
        {
            get { return _vacation; }
            set { _vacation = value; NotifyOfPropertyChange(() => Vacation); }
        }


        private string _holiday;
        public string Holiday
        {
            get { return _holiday; }
            set { _holiday = value; NotifyOfPropertyChange(() => Holiday); }
        }


        private string _sicknessHours;
        public string SicknessHours
        {
            get { return _sicknessHours; }
            set { _sicknessHours = value; NotifyOfPropertyChange(() => SicknessHours); }
        }


        private int? _hourlyWage;
        public string HourlyWage
        {
            get { return _hourlyWage.ToString(); }
            set
            {
                int result;
                if (string.IsNullOrEmpty(value)) {
                    _hourlyWage = null;
                    NotifyOfPropertyChange(() => HourlyWage);
                    return;
                }

                if (!int.TryParse(value, out result)) {
                    return;
                }

                if (result < 0) {
                    return;

                } else {
                    _hourlyWage = result;
                }

                NotifyOfPropertyChange(() => HourlyWage);
            }
        }


        private string _vacationDays;
        public string VacationDays
        {
            get { return _vacationDays; }
            set { _vacationDays = value; NotifyOfPropertyChange(() => VacationDays); }
        }


        private string _diets;
        public string Diets
        {
            get { return _diets; }
            set { _diets = value; NotifyOfPropertyChange(() => Diets); }
        }


        private string _paidHolidays;
        public string PaidHolidays
        {
            get { return _paidHolidays; }
            set { _paidHolidays = value; NotifyOfPropertyChange(() => PaidHolidays); }
        }


        private string _bonuses;
        public string Bonuses
        {
            get { return _bonuses; }
            set { _bonuses = value; NotifyOfPropertyChange(() => Bonuses); }
        }


        private string _dollars; // better name? :D
        public string Dollars
        {
            get { return _dollars; }
            set { _dollars = value; NotifyOfPropertyChange(() => Dollars); }
        }


        private string _prepayment;
        public string Prepayment
        {
            get { return _prepayment; }
            set { _prepayment = value; NotifyOfPropertyChange(() => Prepayment); }
        }


        private string _sickness;
        public string Sickness
        {
            get { return _sickness; }
            set { _sickness = value; NotifyOfPropertyChange(() => Sickness); }
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

        
        public ListingEditingViewModel(ListingFacade listingFacade, EmployerFacade employerFacade)
        {
            BaseWindowTitle = "Úprava výčetky";

            _listingFacade = listingFacade;
            _employerFacade = employerFacade;

            _selectedEmployer = _promptEmployer;
        }


        protected override void OnInitialize()
        {
            EventAggregator.Subscribe(this);
        }


        private void RefreshEmployers()
        {
            _employers = _employerFacade.FindAllEmployers();
            _employers.Insert(0, _promptEmployer);

            NotifyOfPropertyChange(() => Employers);
        }


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

            if (_hourlyWage != null && _hourlyWage <= 0) {
                HourlyWage = null;
            }

            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(ListingDetailViewModel)));
        }


        private void ReturnBack()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(ListingDetailViewModel)));
        }


        private void Reset(Listing listing)
        {
            RefreshEmployers();

            _years.Clear();
            _years.Add(listing.Year);
            _months.Clear();
            _months.Add(Date.Months[12 - listing.Month]);

            SelectedYear = 0;
            SelectedMonth = 0;


            if (listing.Employer == null || !_employers.Exists(e => e == listing.Employer)) {
                SelectedEmployer = _promptEmployer;
            } else {
                SelectedEmployer = listing.Employer;
            }

            Name = listing.Name;
            HourlyWage = listing.HourlyWage == null ? null : listing.HourlyWage.ToString();

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



        // -----


        public void Handle(ListingMessage message)
        {
            Listing = message.Listing;
        }


        protected override void OnActivate()
        {
            if (Listing != null) {
                Reset(Listing);
            }
        }
    }
}
