﻿using Listings.Exceptions;
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
        private readonly Time _start;
        public Time Start
        {
            get { return _start; }
        }


        private readonly Time _end;
        public Time End
        {
            get { return _end; }
        }


        private readonly Time _lunchStart;
        public Time LunchStart
        {
            get { return _lunchStart; }
        }


        private readonly Time _lunchEnd;
        public Time LunchEnd
        {
            get { return _lunchEnd; }
        }


        private readonly Time _otherHours;
        public Time OtherHours
        {
            get { return _otherHours; }
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
            _start = new Time();
            _end = new Time();
            _lunchStart = new Time();
            _lunchEnd = new Time();
            _otherHours = new Time();
        }


        public TimeSetting(Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            CheckTime(start, end, lunchStart, lunchEnd, otherHours);

            _start = start;
            _end = end;
            _lunchStart = lunchStart;
            _lunchEnd = lunchEnd;
            _otherHours = otherHours;
        }


        public TimeSetting(int start, int end, int lunchStart, int lunchEnd, int otherHours)
        {
            CheckTime(start, end, lunchStart, lunchEnd, otherHours);

            _start = new Time(start);
            _end = new Time(end);
            _lunchStart = new Time(lunchStart);
            _lunchEnd = new Time(lunchEnd);
            _otherHours = new Time(otherHours);
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
