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


        private TimeSetting _timeSetting;
        public TimeSetting TimeSetting
        {
            get { return _timeSetting; }
            private set { _timeSetting = value; }
        }


        private string _locality;
        public string Locality
        {
            get { return _locality; }
            private set { _locality = value; }
        }


        public ListingItem(Listing listing, int day, string locality, Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            Listing = listing;

            Date = new DateTime(listing.Year, listing.Month, day);

            _day = day;
            _locality = locality;
            
            _timeSetting = new TimeSetting(start, end, lunchStart, lunchEnd, otherHours);
        }

    }
}