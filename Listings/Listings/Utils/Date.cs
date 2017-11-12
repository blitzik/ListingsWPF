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
    }
}
