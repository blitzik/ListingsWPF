using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class DayItem
    {
        private int _year;
        public int Year
        {
            get { return _year; }
        }


        private int _month;
        public int Month
        {
            get { return _month; }
        }


        private int _day;
        public int Day
        {
            get { return _day; }
        }


        private DateTime _date;
        public DateTime Date
        {
            get
            {
                if (_date == null) {
                    _date = new DateTime(_year, _month, _day);
                }

                return _date;
            }
        }


        private string _locality;
        public string Locality
        {
            get { return _locality; }
        }


        private Time _shiftStart;
        public Time ShiftStart
        {
            get { return _shiftStart; }
        }


        private Time _shiftEnd;
        public Time ShiftEnd
        {
            get { return _shiftEnd; }
        }


        private Time _shiftLunchStart;
        public Time ShiftLunchStart
        {
            get { return _shiftLunchStart; }
        }


        private Time _shiftLunchEnd;
        public Time ShiftLunchEnd
        {
            get { return _shiftLunchEnd; }
        }


        public Time LunchHours
        {
            get { return _listingItem == null ? null : ShiftLunchEnd - ShiftLunchStart; }
        }


        public Time WorkedHours
        {
            get { return _listingItem == null ? null : ShiftEnd - ShiftStart - LunchHours; }
        }


        private ListingItem _listingItem;


        public DayItem(int year, int month, int day)
        {
            _year = year;
            _month = month;
            _day = day;
        }


        public DayItem(ListingItem item)
        {
            _listingItem = item;

            _year = item.Date.Year;
            _month = item.Date.Month;
            _day = item.Date.Day;

            _locality = item.Locality;
            _shiftStart = item.ShiftStart;
            _shiftEnd = item.ShiftEnd;
            _shiftLunchStart = item.ShiftLunchStart;
            _shiftLunchEnd = item.ShiftLunchEnd;
        }


    }
}
