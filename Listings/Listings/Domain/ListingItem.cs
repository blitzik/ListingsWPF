﻿using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listings.Exceptions;
using Caliburn.Micro;

namespace Listings.Domain
{
    public class ListingItem : PropertyChangedBase
    {
        private readonly DateTime _date;
        public DateTime Date
        {
            get { return _date; }
        }


        private readonly int _day;
        public int Day
        {
            get { return _day; }
        }


        private readonly TimeSetting _timeSetting;
        public TimeSetting TimeSetting
        {
            get { return _timeSetting; }
        }


        private string _locality;
        public string Locality
        {
            get { return _locality; }
            private set { _locality = value; }
        }


        public ListingItem(Listing listing, int day, string locality, Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            _date = new DateTime(listing.Year, listing.Month, day);

            _day = day;
            _locality = locality;
            
            _timeSetting = new TimeSetting(start, end, lunchStart, lunchEnd, otherHours);
        }

    }
}