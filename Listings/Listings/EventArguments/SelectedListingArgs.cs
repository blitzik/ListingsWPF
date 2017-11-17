using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.EventArguments
{
    public class SelectedListingArgs : EventArgs
    {
        public Listing SelectedListing { private set; get; }


        public SelectedListingArgs(Listing listing)
        {
            SelectedListing = listing;
        }
    }
}
