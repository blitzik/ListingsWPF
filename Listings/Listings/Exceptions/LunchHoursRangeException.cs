using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Exceptions
{
    public class LunchHoursRangeException : Exception
    {
        public LunchHoursRangeException()
        {
        }


        public LunchHoursRangeException(string message) : base(message)
        {
        }
    }
}
