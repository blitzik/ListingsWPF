﻿using System;
using System.Text.RegularExpressions;

namespace Listings.Utils
{
    public class Time
    {
        private int _seconds;
        public int Seconds
        {
            get { return _seconds; }
            private set { _seconds = value; }
        }


        private string _text;
        public string Text
        {
            get { return _text; }
            private set { _text = value; }
        }


        public bool IsNegative
        {
            get { return Seconds < 0; }
        }


        public Time()
        {
            Seconds = 0;
            Text = "00:00:00";
        }


        public Time(int seconds)
        {
            Seconds = seconds;
            Text = Seconds2Time(seconds);
        }


        public Time(string time)
        {
            Text = time;
            Seconds = Time2Seconds(time);
        }


        public Time GetNegative()
        {
            return new Time(Seconds * -1);
        }


        public Time Sum(Time time2sum)
        {
            return new Time(Seconds + time2sum.Seconds);
        }


        public Time Sub(Time time2sub)
        {
            return new Time(Seconds - time2sub.Seconds);
        }


        public int Compare(Time time)
        {
            if (Seconds > time.Seconds) {
                return 1;
            }

            if (Seconds < time.Seconds) {
                return -1;
            }

            return 0;
        }


        public bool IsLowerThan(Time time)
        {
            return Compare(time) < 0;
        }


        public bool IsLowerOrEqualTo(Time time)
        {
            return Compare(time) <= 0;
        }


        public bool IsHigherThan(Time time)
        {
            return Compare(time) > 0;
        }


        public bool IsHigherOrEqualTo(Time time)
        {
            return Compare(time) >= 0;
        }


        public bool IsEqualTo(Time time)
        {
            return Compare(time) == 0;
        }


        public override string ToString()
        {
            return Text;
        }


        public static string Seconds2Time(int seconds)
        {
            int hours = seconds / 3600;
            int minutes = Math.Abs((seconds % 3600) / 60);
            int secs = Math.Abs((seconds % 3600) % 60);

            return string.Format("{0}:{1}:{2}", (hours > -10 && hours < 10) ? hours.ToString("D2") : hours.ToString(), minutes.ToString("D2"), secs.ToString("D2"));
        }


        public static int Time2Seconds(string time)
        {
            MatchCollection matches = Regex.Matches(time, @"^(?<hours>-?[0-9]+):(?<minutes>[0-5][0-9])(:(?<seconds>[0-5][0-9]))?$");
            if (matches.Count == 0) {

            }

            int.TryParse(matches[0].Groups["hours"].Value, out int hours);
            int.TryParse(matches[0].Groups["minutes"].Value, out int minutes);
            int.TryParse(matches[0].Groups["seconds"].Value, out int seconds);

            return ((Math.Abs(hours) * 3600) + (minutes * 60) + seconds) * (hours < 0 ? -1 : 1);
        }

    }
}