using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class InternalBackup : PropertyChangedBase
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
        }


        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                NotifyOfPropertyChange(() => FilePath);
            }
        }


        public InternalBackup(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;

            _createdAt = DateTime.Now;

        }
    }
}
