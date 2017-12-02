using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Utils
{
    public class Date
    {
        private static readonly List<string> _months = new List<string>() {
            "Prosinec", "Listopad", "Říjen", "Září",
            "Srpen", "Červenec", "Červen", "Květen",
            "Duben", "Březen", "Únor", "Leden"
        };
        public static List<string> Months
        {
            get { return _months; }
        }


        private static readonly List<string> _daysOfWeek = new List<string>() {
            "Neděle", "Pondělí", "Úterý", "Středa", "Čtvrtek", "Pátek", "Sobota"
        };
        public static List<string> DaysOfWeek
        {
            get { return _daysOfWeek; }
        }


        public static List<int> GetLastYears(int numberOfYears)
        {
            int stopYear = DateTime.Now.Year - numberOfYears;
            List<int> list = new List<int>();
            for (int year = DateTime.Now.Year; year > stopYear; year--) {
                list.Add(year);
            }

            return list;
        }
    }
}
