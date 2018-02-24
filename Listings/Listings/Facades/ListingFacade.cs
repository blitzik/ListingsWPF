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
using Db4objects.Db4o.Ext;
using Listings.Services;

namespace Listings.Facades
{
    public class ListingFacade : BaseFacade
    {
        public ListingFacade(ObjectContainerRegistry dbRegistry) : base (dbRegistry)
        {
            _dbRegistry = dbRegistry;
        }


        public void Save(Listing listing)
        {
            Db().Store(listing);
            Db().Commit();
        }


        public List<Listing> FindAllListings()
        {
            IEnumerable<Listing> listings = from Listing l in Db() select l;

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
            IEnumerable<Listing> listings = Db().Query<Listing>(l => l.Year == year, comp);
            foreach (Listing l in listings) {
                l.OnReplacedListingItem += OnChangedListingItem;
                l.OnRemovedListingItem += OnChangedListingItem;
                l.OnSummaryTimeChanged += OnTimeChanged;
            }

            return new List<Listing>(listings);
        }


        private void OnChangedListingItem(object sender, ListingItemArgs args)
        {
            Db().Delete(args.ListingItem);
        }


        private void OnTimeChanged(object sender, Time time)
        {
            Db().Delete(time);
        }


        public void DeleteListing(Listing listing)
        {
            Db().Delete(listing);
            Db().Commit();
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
            Db().Activate(listing, depth);
        }


        public void RefreshListing(Listing listing)
        {
            Db().Ext().Refresh(listing, 4);
        }


        // -----


        public Dictionary<string, int> DisplayStats()
        {
            IEnumerable<Listing> list = from Listing l in Db() where l.Month == 5 && l.Year == 2013 select l;

            /*Random r = new Random();
            for (int i = 0; i < 100000; i++) {
                Listing l = new Listing(r.Next(2010, 2019), r.Next(1, 13));
                Db().Store(l);
            }
            Db().Commit();*/


            Dictionary<string, int> stats = new Dictionary<string, int>();

            stats.Add("Listing", (from Listing l in Db() select l).Count());
            stats.Add("ListingItem", (from ListingItem l in Db() select l).Count());
            stats.Add("Time", (from Time l in Db() select l).Count());
            stats.Add("TimeSetting", (from TimeSetting l in Db() select l).Count());
            stats.Add("DefaultSettings", (from DefaultSettings l in Db() select l).Count());
            stats.Add("Employer", (from Employer l in Db() select l).Count());
            stats.Add("DefaultListingPdfReportSetting", (from DefaultListingPdfReportSetting l in Db() select l).Count());

            return stats;
        }

    }
}
