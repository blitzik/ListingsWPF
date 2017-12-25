using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listings.Exceptions;

namespace Listings.Domain
{
    public class ListingItem : BindableObject
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            private set { _listing = value; }
        }


        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            private set { _date = value; }
        }


        private readonly int _day;
        public int Day
        {
            get { return _day; }
        }


        private Time _shiftStart;
        public Time ShiftStart
        {
            get { return _shiftStart; }
            private set { ChangeHours(value, ShiftEnd, ShiftLunchStart, ShiftLunchEnd, OtherHours); }
        }


        private Time _shiftEnd;
        public Time ShiftEnd
        {
            get { return _shiftEnd; }
            private set { ChangeHours(ShiftStart, value, ShiftLunchStart, ShiftLunchEnd, OtherHours); }
        }


        private Time _shiftLunchStart;
        public Time ShiftLunchStart
        {
            get { return _shiftLunchStart; }
            private set { ChangeHours(ShiftStart, ShiftEnd, value, ShiftLunchEnd, OtherHours); }
        }


        private Time _shiftLunchEnd;
        public Time ShiftLunchEnd
        {
            get { return _shiftLunchEnd; }
            private set { ChangeHours(ShiftStart, ShiftEnd, ShiftLunchStart, value, OtherHours); }
        }


        private Time _otherHours;
        public Time OtherHours
        {
            get { return _otherHours; }
            private set
            {
                if ((ShiftEnd - ShiftStart - LunchHours + value) < 0) {
                    throw new Exception("Other hours makes worked hours lower than 0");
                }

                ChangeHours(ShiftStart, ShiftEnd, ShiftLunchStart, ShiftLunchEnd, value);
            }
        }


        private string _locality;
        public string Locality
        {
            get { return _locality; }
            private set { _locality = value; }
        }


        public Time LunchHours
        {
            get { return ShiftLunchEnd - ShiftLunchStart; }
        }


        public Time WorkedHours
        {
            get { return ShiftEnd - ShiftStart - LunchHours + OtherHours; }
        }


        public bool HasNoTime
        {
            get
            {
                if (_shiftStart == 0 && _shiftEnd == 0 && _shiftLunchStart == 0 && _shiftLunchEnd == 0 && _otherHours == 0) {
                    return true;
                }

                return false;
            }
        }


        public ListingItem(Listing listing, int day, string locality, Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            Listing = listing;

            Date = new DateTime(listing.Year, listing.Month, day);

            _day = day;
            _locality = locality;

            ChangeHours(start, end, lunchStart, lunchEnd, otherHours);
        }


        private void ChangeHours(Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            if (!(start == 0 && end == 0 && lunchStart == 0 && lunchEnd == 0 && otherHours == 0)) {
                if (start >= end) {
                    throw new WorkedHoursRangeException();
                }

                if (!(lunchStart == 0 && lunchEnd == 0)) {
                    if (lunchStart >= lunchEnd) {
                        throw new LunchHoursRangeException();
                    }

                    if (lunchStart < start || lunchEnd > end) {
                        throw new LunchHoursOutOfWorkedHoursRangeException();
                    }
                }
            }

            _shiftStart = start;
            _shiftEnd = end;
            _shiftLunchStart = lunchStart;
            _shiftLunchEnd = lunchEnd;
            _otherHours = otherHours;
        }

    }
}