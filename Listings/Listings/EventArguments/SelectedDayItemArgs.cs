using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.EventArguments
{
    public class SelectedDayItemArgs : EventArgs
    {
        public DayItem DayItem { get; private set; }

        public SelectedDayItemArgs(DayItem dayItem)
        {
            DayItem = dayItem;
        }
    }
}
