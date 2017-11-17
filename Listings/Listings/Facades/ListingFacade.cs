using Db4objects.Db4o;
using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db4objects.Db4o.Linq;

namespace Listings.Facades
{
    public class ListingFacade
    {
        private IObjectContainer _db;


        public ListingFacade(IObjectContainer db)
        {
            _db = db;
        }


        public void Save(Listing listing)
        {
            _db.Store(listing);
        }


        public List<Listing> FindListings()
        {
            IEnumerable<Listing> listings = from Listing l in _db select l;

            return new List<Listing>(listings);
        }

    }
}
