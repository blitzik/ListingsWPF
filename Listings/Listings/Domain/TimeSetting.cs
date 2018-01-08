using Listings.Exceptions;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class TimeSetting
    {
        private Time _start;
        public Time Start
        {
            get { return _start; }
            private set { _start = value; }
        }


        private Time _end;
        public Time End
        {
            get { return _end; }
            private set { _end = value; }
        }


        private Time _lunchStart;
        public Time LunchStart
        {
            get { return _lunchStart; }
            private set { _lunchStart = value; }
        }


        private Time _lunchEnd;
        public Time LunchEnd
        {
            get { return _lunchEnd; }
            private set { _lunchEnd = value; }
        }


        private Time _otherHours;
        public Time OtherHours
        {
            get { return _otherHours; }
            private set { _otherHours = value; }
        }


        public Time LunchHours
        {
            get { return LunchEnd - LunchStart; }
        }


        public Time WorkedHoursWithLunch
        {
            get { return End - Start; }
        }


        public Time WorkedHours
        {
            get { return WorkedHoursWithLunch - LunchHours; }
        }


        public Time TotalWorkedHours
        {
            get { return WorkedHours + OtherHours; }
        }


        public bool HasNoTime
        {
            get
            {
                if (Start == 0 && End == 0 && LunchStart == 0 && LunchEnd == 0 && OtherHours == 0) {
                    return true;
                }

                return false;
            }
        }


        public TimeSetting()
        {
            Start = new Time();
            End = new Time();
            LunchStart = new Time();
            LunchEnd = new Time();
            OtherHours = new Time();
        }


        public TimeSetting(Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            CheckTime(start, end, lunchStart, lunchEnd, otherHours);

            Start = start;
            End = end;
            LunchStart = lunchStart;
            LunchEnd = lunchEnd;
            OtherHours = otherHours;
        }


        public TimeSetting(int start, int end, int lunchStart, int lunchEnd, int otherHours)
        {
            CheckTime(start, end, lunchStart, lunchEnd, otherHours);

            Start = new Time(start);
            End = new Time(end);
            LunchStart = new Time(lunchStart);
            LunchEnd = new Time(lunchEnd);
            OtherHours = new Time(otherHours);
        }


        public static void CheckTime(Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            if (!(start == 0 && end == 0 && lunchStart == 0 && lunchEnd == 0 && otherHours == 0)) {
                if (start >= end) {
                    throw new WorkedHoursRangeException();
                }

                if (!(lunchStart == 0 && lunchEnd == 0)) {
                    if (lunchStart >= lunchEnd) {
                        throw new LunchHoursRangeException();
                    }

                    if (lunchStart < start || lunchEnd > end) {
                        throw new LunchHoursOutOfWorkedHoursRangeException();
                    }
                }

                if ((end - start - (lunchEnd - lunchStart) + otherHours) < 0) {
                    throw new OtherHoursException();
                }
            }
        }


        public static void CheckTime(int start, int end, int lunchStart, int lunchEnd, int otherHours)
        {
            if (!(start == 0 && end == 0 && lunchStart == 0 && lunchEnd == 0 && otherHours == 0)) {
                if (start >= end) {
                    throw new WorkedHoursRangeException();
                }

                if (!(lunchStart == 0 && lunchEnd == 0)) {
                    if (lunchStart >= lunchEnd) {
                        throw new LunchHoursRangeException();
                    }

                    if (lunchStart < start || lunchEnd > end) {
                        throw new LunchHoursOutOfWorkedHoursRangeException();
                    }
                }

                if ((end - start - (lunchEnd - lunchStart) + otherHours) < 0) {
                    throw new OtherHoursException();
                }
            }
        }

    }
}
