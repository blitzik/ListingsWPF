using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class ListingSetting
    {
        private TimeSetting _timeSetting;
        public TimeSetting TimeSetting
        {
            get { return _timeSetting; }
            private set { _timeSetting = value; }
        }


        public ListingSetting(TimeSetting timeSetting)
        {
            TimeSetting = TimeSetting;
        }
    }
}
