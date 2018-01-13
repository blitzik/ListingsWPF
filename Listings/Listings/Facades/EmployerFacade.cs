using Db4objects.Db4o;
using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o.Linq;
using System.Threading.Tasks;
using Listings.Services;

namespace Listings.Facades
{
    public class EmployerFacade : BaseFacade
    {
        public EmployerFacade(ObjectContainerRegistry dbRegistry)
        {
            _dbRegistry = dbRegistry;
        }


        public void Save(Employer employer)
        {
            Db().Store(employer);
            Db().Commit();
        }


        public void Delete(Employer employer)
        {
            Db().Delete(employer);
            Db().Commit();
        }


        public List<Employer> FindAllEmployers(string order = "DESC")
        {
            IEnumerable<Employer> employers;
            switch (order.ToLower()) {
                case "ASC":
                    employers = from Employer e in Db() orderby e.CreatedAt ascending select e;
                    break;

                default:
                    employers = from Employer e in Db() orderby e.CreatedAt descending select e;
                    break;
            }
            
            return employers.ToList<Employer>();
        }
    }
}
