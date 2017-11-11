using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listings.Exceptions;

namespace Listings.Models
{
    public class ListingItem
    {
        private Listing _listing;
        private Listing Listing
        {
            get { return _listing; }
            set { _listing = value; }
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


        public Time _shiftStart;
        public Time ShiftStart
        {
            get { return _shiftStart; }
            set { ChangeHours(value, ShiftEnd, ShiftLunchStart, ShiftLunchEnd); }
        }


        public Time _shiftEnd;
        public Time ShiftEnd
        {
            get { return _shiftEnd; }
            set { ChangeHours(ShiftStart, value, ShiftLunchStart, ShiftLunchEnd); }
        }


        public Time _shiftLunchStart;
        public Time ShiftLunchStart
        {
            get { return _shiftLunchStart; }
            set { ChangeHours(ShiftStart, ShiftEnd, value, ShiftLunchEnd); }
        }


        public Time _shiftLunchEnd;
        public Time ShiftLunchEnd
        {
            get { return _shiftLunchEnd; }
            set { ChangeHours(ShiftStart, ShiftEnd, ShiftLunchStart, value); }
        }


        public Time LunchHours
        {
            get { return ShiftLunchEnd.Sub(ShiftLunchStart); }
        }


        public Time WorkedHours
        {
            get { return ShiftEnd.Sub(ShiftStart).Sub(LunchHours); }
        }


        public ListingItem(Listing listing, int day, Time start, Time end, Time lunchStart, Time lunchEnd)
        {
            _day = day;
            Date = new DateTime(listing.Year, listing.Month, day);

            Listing = listing;

            ShiftStart = start;
            ShiftEnd = end;
            ShiftLunchStart = lunchStart;
            ShiftLunchEnd = lunchEnd;
        }


        public void ChangeHours(Time start, Time end, Time lunchStart, Time lunchEnd)
        {
            if (start.IsHigherOrEqualTo(end)) {
                throw new WorkedHoursRangeException();
            }

            if (lunchStart.IsHigherOrEqualTo(lunchEnd)) {
                throw new LunchHoursRangeException();
            }

            if (lunchStart.IsLowerOrEqualTo(start) || lunchEnd.IsHigherOrEqualTo(end)) {
                throw new LunchHoursOutOfWorkedHoursRangeException();
            }

            ShiftStart = start;
            ShiftEnd = end;
            ShiftLunchStart = lunchStart;
            ShiftLunchEnd = lunchEnd;
        }

    }
}