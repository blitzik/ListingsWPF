using Listings.Exceptions;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Models
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
            set
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
            set
            {
                if (value < 1 || value > 12) {
                    throw new OutOfRangeException();
                }
                _month = value;
            }
        }


        private int _workedDays;
        public int WorkedDays
        {
            get { return _workedDays; }
            private set { _workedDays = value; }
        }


        private Time _workedHours;
        public Time WorkedHours
        {
            get { return _workedHours; }
            private set { _workedHours = value; }
        }


        public Listing(int year, int month)
        {
            Year = year;
            Month = month;
        }
    }
}
