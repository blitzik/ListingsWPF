using Listings.Exceptions;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class Listing
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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


        private List<ListingItem> _items;


        private int _workedDays = 0;
        public int WorkedDays
        {
            get { return _workedDays; }
            private set { _workedDays = value; }
        }


        private Time _workedHours = new Time("00:00");
        public Time WorkedHours
        {
            get { return _workedHours; }
            private set { _workedHours = value; }
        }


        public Listing(int year, int month)
        {
            Year = year;
            Month = month;

            _items = new List<ListingItem>();
        }


        public void AddItem(int day, string locality, Time start, Time end, Time lunchStart, Time lunchEnd)
        {
            if (_items.Exists(i => i.Day == day)) {
                throw new ListingItemAlreadyExistsException();
            }

            _items.Insert(day, new ListingItem(this, day, locality, start, end, lunchStart, lunchEnd));
        }


        public ListingItem GetItemByDay(int day)
        {
            return _items.ElementAt(day);
        }
        
    }
}
