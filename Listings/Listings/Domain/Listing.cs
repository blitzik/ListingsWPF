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
            private set {
                _workedDays = value;
                RaisePropertyChanged();
            }
        }


        private Time _workedHours = new Time("00:00");
        public Time WorkedHours
        {
            get { return _workedHours; }
            private set {
                _workedHours = value;
                RaisePropertyChanged();
            }
        }


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
            WorkedHours += newItem.WorkedHours;

            return newItem;
        }


        public ListingItem ReplaceItem(int day, string locality, Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            if (_items.Exists(i => i.Day == day)) {
                ListingItem currentItem = GetItemByDay(day);
                ListingItem newItem = new ListingItem(this, day, locality, start, end, lunchStart, lunchEnd, otherHours);

                WorkedHours += newItem.WorkedHours - currentItem.WorkedHours;

                _items[_items.IndexOf(currentItem)] = newItem;
                return newItem;
            }

            return AddItem(day, locality, start, end, lunchStart, lunchEnd, otherHours);
        }


        public void RemoveItemByDay(int day)
        {
            int index = _items.FindIndex(i => i.Day == day);
            if (index == -1) { // there is no listing item for that day
                return;
            }

            ListingItem item = _items[index];
            WorkedDays--;
            WorkedHours -= item.WorkedHours;

            _items.RemoveAt(index);
        }


        public ListingItem GetItemByDay(int day)
        {
            return _items.FirstOrDefault<ListingItem>(i => i.Day == day);
        }
        
    }
}
