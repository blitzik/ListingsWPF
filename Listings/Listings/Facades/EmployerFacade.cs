using Db4objects.Db4o;
using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o.Linq;
using System.Threading.Tasks;

namespace Listings.Facades
{
    public class EmployerFacade
    {
        private IObjectContainer _db;


        public EmployerFacade(IObjectContainer db)
        {
            _db = db;
        }


        public void Save(Employer employer)
        {
            _db.Store(employer);
            _db.Commit();
        }


        public void Delete(Employer employer)
        {
            _db.Delete(employer);
            _db.Commit();
        }


        public List<Employer> FindAllEmployers()
        {
            IEnumerable<Employer> employers = from Employer e in _db select e;

            return employers.ToList<Employer>();
        }
    }
}
