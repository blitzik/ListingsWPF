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


        private string _ownerName;
        public string OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; }
        }


        public DefaultSettings(TimeSetting timeSetting)
        {
            _time = timeSetting;
        }
    }
}
