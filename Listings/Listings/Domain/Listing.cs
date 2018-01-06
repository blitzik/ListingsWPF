using Listings.Exceptions;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class Listing : BindableObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value)) {
                    _name = "Bez názvu";
                } else {
                    _name = value;
                }
                RaisePropertyChanged();
            }
        }


        private int _year;
        public int Year
        {
            get { return _year; }
            private set
            {
                if (value < 1) {
                    throw new OutOfRangeException("Only numbers higher than 0 can pass");
                }

                _year = value;
            }
        }


        private int _month;
        public int Month
        {
            get { return _month; }
            private set
            {
                if (value < 1 || value > 12) {
                    throw new OutOfRangeException();
                }
                _month = value;
            }
        }


        private Employer _employer;
        public Employer Employer
        {
            get { return _employer; }
            set
            {
                _employer = value;
                RaisePropertyChanged();
            }
        }


        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            private set { _createdAt = value; }
        }


        public int DaysInMonth
        {
            get
            {
                return DateTime.DaysInMonth(Year, Month);
            }
        }


        private List<ListingItem> _items;
        public List<ListingItem> Items
        {
            get { return _items; }
            private set
            {
                _items = value;
            }
        }


        private int _workedDays = 0;
        public int WorkedDays
        {
            get { return _workedDays; }
            private set
            {
                _workedDays = value;
                RaisePropertyChanged();
            }
        }


        private Time _workedHours;
        public Time WorkedHours
        {
            get { return _workedHours ?? new Time("00:00"); }
            private set
            {
                _workedHours = value;
                RaisePropertyChanged();
            }
        }


        private Time _otherHours;
        public Time OtherHours
        {
            get { return _otherHours ?? new Time("00:00"); }
            set { _otherHours = value; }
        }


        private Time _lunchHours;
        public Time LunchHours
        {
            get { return _lunchHours ?? new Time("00:00"); }
            set { _lunchHours = value; }
        }


        private Time _totalWorkedHours;
        public Time TotalWorkedHours
        {
            get { return _totalWorkedHours ?? new Time("00:00"); }
            private set
            {
                _totalWorkedHours = value;
                RaisePropertyChanged();
            }
        }


        // ----- Summary hours


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


        // -----


        private int? _hourlyWage;
        public int? HourlyWage
        {
            get { return _hourlyWage; }
            set
            {
                _hourlyWage = value;
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


        // -----


        public Listing(int year, int month)
        {
            Year = year;
            Month = month;
            CreatedAt = DateTime.Now;

            _items = new List<ListingItem>();
        }


        public ListingItem AddItem(int day, string locality, Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            if (_items.Exists(i => i.Day == day)) {
                throw new ListingItemAlreadyExistsException();
            }

            ListingItem newItem = new ListingItem(this, day, locality, start, end, lunchStart, lunchEnd, otherHours);
            _items.Add(newItem);
            WorkedDays++;
            WorkedHours += newItem.TimeSetting.WorkedHours;
            LunchHours += newItem.TimeSetting.LunchHours;
            OtherHours += newItem.TimeSetting.OtherHours;
            TotalWorkedHours += newItem.TimeSetting.TotalWorkedHours;

            return newItem;
        }


        public ListingItem AddItem(int day, string locality, TimeSetting timeSetting)
        {
            return AddItem(day, locality, timeSetting.Start, timeSetting.End, timeSetting.LunchStart, timeSetting.LunchEnd, timeSetting.OtherHours);
        }


        public ListingItem ReplaceItem(int day, string locality, Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            if (_items.Exists(i => i.Day == day)) {
                ListingItem currentItem = GetItemByDay(day);
                ListingItem newItem = new ListingItem(this, day, locality, start, end, lunchStart, lunchEnd, otherHours);

                WorkedHours += newItem.TimeSetting.WorkedHours - currentItem.TimeSetting.WorkedHours;
                LunchHours += newItem.TimeSetting.LunchHours - currentItem.TimeSetting.LunchHours;
                OtherHours += newItem.TimeSetting.OtherHours - currentItem.TimeSetting.OtherHours;
                TotalWorkedHours += newItem.TimeSetting.TotalWorkedHours - currentItem.TimeSetting.TotalWorkedHours;

                _items[_items.IndexOf(currentItem)] = newItem;
                return newItem;
            }

            return AddItem(day, locality, start, end, lunchStart, lunchEnd, otherHours);
        }


        public ListingItem ReplaceItem(int day, string locality, TimeSetting timeSetting)
        {
            return ReplaceItem(day, locality, timeSetting.Start, timeSetting.End, timeSetting.LunchStart, timeSetting.LunchEnd, timeSetting.OtherHours);
        }


        public void RemoveItemByDay(int day)
        {
            int index = _items.FindIndex(i => i.Day == day);
            if (index == -1) { // there is no listing item for that day
                return;
            }

            ListingItem item = _items[index];
            WorkedDays--;
            WorkedHours -= item.TimeSetting.WorkedHours;
            LunchHours -= item.TimeSetting.LunchHours;
            OtherHours -= item.TimeSetting.OtherHours;
            TotalWorkedHours -= item.TimeSetting.TotalWorkedHours;

            _items.RemoveAt(index);
        }


        public ListingItem GetItemByDay(int day)
        {
            return _items.FirstOrDefault<ListingItem>(i => i.Day == day);
        }
        
    }
}
