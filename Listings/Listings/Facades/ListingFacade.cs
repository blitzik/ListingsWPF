using Db4objects.Db4o;
using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Foundation;

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


        public List<Listing> FindAllListings()
        {
            IEnumerable<Listing> listings = from Listing l in _db select l;

            return new List<Listing>(listings);
        }


        private delegate int OnListingsLoad(Listing l1, Listing l2);
        public List<Listing> FindListings(int year, string order = "DESC")
        {
            OnListingsLoad comparer = LoadInASC;
            switch (order) {
                case "DESC":
                    comparer = LoadInDESC;
                    break;

                default:
                    comparer = LoadInASC;
                    break;
            }

            Comparison<Listing> comp = new Comparison<Listing>(comparer);
            IEnumerable<Listing> listings = _db.Query<Listing>(l => l.Year == year, comp);


            return new List<Listing>(listings);
        }


        private int LoadInASC(Listing l1, Listing l2)
        {
            return l1.CreatedAt.CompareTo(l2.CreatedAt);
        }


        private  int LoadInDESC(Listing l1, Listing l2)
        {
            return l2.CreatedAt.CompareTo(l1.CreatedAt);
        }

    }
}
