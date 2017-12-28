﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Exceptions
{
    public class WorkedHoursRangeException : WrongTimeException
    {
        public WorkedHoursRangeException()
        {
        }


        public WorkedHoursRangeException(string message) : base(message)
        {
        }
    }
}
