using Listings.Utils;
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


        private int _timeTickInMinutes;
        public int TimeTickInMinutes
        {
            get { return _timeTickInMinutes; }
            set { _timeTickInMinutes = value; }
        }


        private DefaultListingPdfReportSetting _pdfSetting;
        public DefaultListingPdfReportSetting Pdfsetting
        {
            get { return _pdfSetting; }
            set
            {
                _pdfSetting = value;
            }
        }


        public DefaultSettings()
        {
            Time = new TimeSetting(
                new Time("06:00"),
                new Time("14:30"),
                new Time("10:30"),
                new Time("11:00"),
                new Time("00:00")
            );

            TimeTickInMinutes = 5;

            Pdfsetting = new DefaultListingPdfReportSetting();
        }


        public DefaultSettings(TimeSetting timeSetting, int timeTickInMinutes)
        {
            _time = timeSetting;
            _timeTickInMinutes = timeTickInMinutes;
        }
    }
}
