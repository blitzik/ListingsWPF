using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Exceptions
{
    public class OutOfRangeException : Exception
    {
        public OutOfRangeException()
        {
        }


        public OutOfRangeException(string message) : base(message)
        {
        }
    }
}
