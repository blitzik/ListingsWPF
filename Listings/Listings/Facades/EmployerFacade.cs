using Db4objects.Db4o;
using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o.Linq;
using System.Threading.Tasks;
using Listings.Services;
using Perst;

namespace Listings.Facades
{
    public class EmployerFacade : BaseFacade
    {
        public EmployerFacade(StoragePool db) : base (db)
        {
        }


        public Employer CreateEmployer(string name)
        {
            Employer e = new Employer(Storage(), name);

            Storage().Store(e);
            Root().Employers.Add(e);
            Storage().Commit();

            return e;
        }


        public void Update(Employer employer)
        {
            Storage().Modify(employer);
            Storage().Commit();
        }


        public void Delete(Employer employer)
        {
            Root().Employers.Remove(employer);
            foreach (Listing l in employer.GetListings()) {
                l.Employer = null;
                Storage().Modify(l);
            }
            Storage().Deallocate(employer);
            Storage().Commit();
        }


        public List<Employer> FindAllEmployers()
        {            
            return new List<Employer>(from Employer e in Root().Employers select e);
        }
    }
}
