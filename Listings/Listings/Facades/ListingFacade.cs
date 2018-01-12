using Db4objects.Db4o;
using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Foundation;
using Listings.EventArguments;
using Listings.Utils;

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
            _db.Commit();
        }


        public List<Listing> FindAllListings()
        {
            IEnumerable<Listing> listings = from Listing l in _db select l;

            return new List<Listing>(listings);
        }


        private delegate int ListingsComparisonHandler(Listing l1, Listing l2);
        public List<Listing> FindListings(int year, string order = "DESC")
        {
            ListingsComparisonHandler comparer = LoadInASC;
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
            foreach (Listing l in listings) {
                l.OnReplacedListingItem += OnChangedListingItem;
                l.OnRemovedListingItem += OnChangedListingItem;
                l.OnSummaryTimeChanged += OnTimeChanged;
            }

            return new List<Listing>(listings);
        }


        private void OnChangedListingItem(object sender, ListingItemArgs args)
        {
            _db.Delete(args.ListingItem);
        }


        private void OnTimeChanged(object sender, Time time)
        {
            _db.Delete(time);
        }


        public void DeleteListing(Listing listing)
        {
            _db.Delete(listing);
            _db.Commit();
        }


        private int LoadInASC(Listing l1, Listing l2)
        {
            return l1.Month.CompareTo(l2.Month);
        }


        private  int LoadInDESC(Listing l1, Listing l2)
        {
            return l2.Month.CompareTo(l1.Month);
        }


        public void Activate(Listing listing, int depth)
        {
            _db.Activate(listing, depth);
        }

    }
}
