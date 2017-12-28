using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class DefaultSettings
    {
        private TimeSetting _time;
        public TimeSetting Time
        {
            get { return _time; }
            set { _time = value; }
        }


        public DefaultSettings(TimeSetting timeSetting)
        {
            _time = timeSetting;
        }
    }
}
