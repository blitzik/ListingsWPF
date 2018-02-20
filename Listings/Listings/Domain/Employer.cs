using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class Employer : PropertyChangedBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }


        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            private set { _createdAt = value; }
        }


        public Employer(string name)
        {
            Name = name;
            CreatedAt = DateTime.Now;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
