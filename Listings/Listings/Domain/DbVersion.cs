using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    /// <summary>
    /// Contains versions of application that can work with this database
    /// </summary>
    public class DbVersion
    {
        public const string UNIQUE_KEY = "db_version";


        private string _id;
        public string ID
        {
            get { return _id; }
        }


        private List<string> _supportedAppVersions;
        public List<string> SupportedAppVersions
        {
            get { return _supportedAppVersions; }
        }


        public DbVersion(string id, List<string> supportedAppVersions)
        {
            _id = id;
            _supportedAppVersions = supportedAppVersions;
        }
    }
}
