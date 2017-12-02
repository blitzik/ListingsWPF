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
            private set { ChangeHours(value, ShiftEnd, ShiftLunchStart, ShiftLunchEnd); }
        }


        private Time _shiftEnd;
        public Time ShiftEnd
        {
            get { return _shiftEnd; }
            private set { ChangeHours(ShiftStart, value, ShiftLunchStart, ShiftLunchEnd); }
        }


        private Time _shiftLunchStart;
        public Time ShiftLunchStart
        {
            get { return _shiftLunchStart; }
            private set { ChangeHours(ShiftStart, ShiftEnd, value, ShiftLunchEnd); }
        }


        private Time _shiftLunchEnd;
        public Time ShiftLunchEnd
        {
            get { return _shiftLunchEnd; }
            private set { ChangeHours(ShiftStart, ShiftEnd, ShiftLunchStart, value); }
        }


        private Time _otherHours;
        public Time OtherHours
        {
            get { return _otherHours; }
            private set
            {
                if ((ShiftEnd - ShiftStart - LunchHours + value) < (new Time("00:00"))) {
                    throw new Exception("Other hours makes worked hours lower than 0");
                }

                _otherHours = value;
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
            get { return ShiftEnd - ShiftStart - LunchHours/* + OtherHours*/; }
        }


        public ListingItem(Listing listing, int day, string locality, Time start, Time end, Time lunchStart, Time lunchEnd)
        {
            Listing = listing;

            Date = new DateTime(listing.Year, listing.Month, day);

            _day = day;
            _locality = locality;

            //OtherHours = new Time("00:00");
            ChangeHours(start, end, lunchStart, lunchEnd);
        }


        private void ChangeHours(Time start, Time end, Time lunchStart, Time lunchEnd)
        {
            if (start >= end) {
                throw new WorkedHoursRangeException();
            }

            if (lunchStart >= lunchEnd) {
                throw new LunchHoursRangeException();
            }

            if (lunchStart <= start || lunchEnd >= end) {
                throw new LunchHoursOutOfWorkedHoursRangeException();
            }

            _shiftStart = start;
            _shiftEnd = end;
            _shiftLunchStart = lunchStart;
            _shiftLunchEnd = lunchEnd;
        }

    }
}