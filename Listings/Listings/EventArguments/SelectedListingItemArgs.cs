using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.EventArguments
{
    public class SelectedListingItemArgs : EventArgs
    {
        public ListingItem ListingItem { get; private set; }

        public SelectedListingItemArgs(ListingItem listingItem)
        {
            ListingItem = listingItem;
        }
    }
}
