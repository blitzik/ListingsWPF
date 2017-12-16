using Listings.Utils;
using System;

namespace Listings.Domain
{
    public class DayItem : BindableObject
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


        private int _week;
        public int Week
        {
            get { return _week; }
        }


        public string ShortDayName
        {
            get
            {
                return Utils.Date.DaysOfWeek[(int)Date.DayOfWeek].Substring(0, 2);
            }
        }


        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
        }


        private string _locality;
        public string Locality
        {
            get { return _locality; }
            private set
            {
                _locality = value;
                RaisePropertyChanged();
            }
        }


        private Time _shiftStart;
        public Time ShiftStart
        {
            get { return _shiftStart; }
            private set
            {
                _shiftStart = value;
                RaisePropertyChanged();
            }
        }


        private Time _shiftEnd;
        public Time ShiftEnd
        {
            get { return _shiftEnd; }
            private set
            {
                _shiftEnd = value;
                RaisePropertyChanged();
            }
        }


        private Time _shiftLunchStart;
        public Time ShiftLunchStart
        {
            get { return _shiftLunchStart; }
            private set
            {
                _shiftLunchStart = value;
                RaisePropertyChanged();
            }
        }


        private Time _shiftLunchEnd;
        public Time ShiftLunchEnd
        {
            get { return _shiftLunchEnd; }
            private set
            {
                _shiftLunchEnd = value;
                RaisePropertyChanged();
            }
        }


        private Time _otherHours;
        public Time OtherHours
        {
            get { return _otherHours; }
            private set
            {
                _otherHours = value;
                RaisePropertyChanged();
            }
        }


        public Time LunchHours
        {
            get { return _listingItem == null ? null : ShiftLunchEnd - ShiftLunchStart; }
        }


        public Time WorkedHours
        {
            get { return _listingItem == null ? null : ShiftEnd - ShiftStart - LunchHours + OtherHours; }
        }


        private ListingItem _listingItem;
        public ListingItem ListingItem
        {
            get { return _listingItem; }
            private set { _listingItem = value; }
        }


        public bool CanBeRemoved
        {
            get { return _listingItem != null; }
        }


        public bool CanBeCopiedDown
        {
            get { return _listingItem != null && Day < Listing.DaysInMonth; }
        }


        public bool IsWeekendDay
        {
            get { return (Date.DayOfWeek == DayOfWeek.Sunday || Date.DayOfWeek == DayOfWeek.Saturday); }
        }


        public bool IsCurrentDay
        {
            get
            {
                DateTime now = DateTime.Now;
                if (now.Day != Date.Day) {
                    return false;
                }

                if (now.Month != Date.Month) {
                    return false;
                }

                if (now.Year != Date.Year) {
                    return false;
                }

                return true;
            }
        }


        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
        }


        public DayItem(Listing listing, int day)
        {
            _listing = listing;
            _year = listing.Year;
            _month = listing.Month;
            _day = day;
            _date = new DateTime(_year, _month, day);
            _week = PrepareWeek(_year, _month, _day);
        }


        public DayItem(ListingItem item)
        {
            _listingItem = item;
            _listing = item.Listing;

            _year = item.Date.Year;
            _month = item.Date.Month;
            _day = item.Day;
            _date = new DateTime(_year, _month, _day);
            _week = PrepareWeek(_year, _month, _day);

            Locality = item.Locality;
            ShiftStart = item.ShiftStart;
            ShiftEnd = item.ShiftEnd;
            ShiftLunchStart = item.ShiftLunchStart;
            ShiftLunchEnd = item.ShiftLunchEnd;
            OtherHours = item.OtherHours;            
        }


        public void Update(ListingItem item)
        {
            ArgumentException e = new ArgumentException();
            if (item.Listing != Listing) {
                throw e;
            }

            if (item.Date.Day != Day) {
                throw e;
            }

            ListingItem = item;

            Locality = item.Locality;
            ShiftStart = item.ShiftStart;
            ShiftEnd = item.ShiftEnd;
            ShiftLunchStart = item.ShiftLunchStart;
            ShiftLunchEnd = item.ShiftLunchEnd;
            OtherHours = item.OtherHours;

            NotifyPropertiesChanged();
        }


        public void Reset()
        {
            ListingItem = null;

            Locality = null;
            ShiftStart = null;
            ShiftEnd = null;
            ShiftLunchStart = null;
            ShiftLunchEnd = null;
            OtherHours = null;

            NotifyPropertiesChanged();
        }


        private void NotifyPropertiesChanged()
        {
            RaisePropertyChanged(nameof(LunchHours));
            RaisePropertyChanged(nameof(WorkedHours));
            RaisePropertyChanged(nameof(CanBeCopiedDown));
            RaisePropertyChanged(nameof(CanBeRemoved));
        }


        private int PrepareWeek(int year, int month, int day)
        {
            int weekNumber = Listings.Utils.Date.GetWeekNumber(year, month, day);
            //DateTime now = DateTime.Now;
            return weekNumber;
        }

    }
}
