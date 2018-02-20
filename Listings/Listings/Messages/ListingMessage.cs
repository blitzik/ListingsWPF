using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Messages
{
    public class ListingMessage
    {
        private readonly Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
        }


        public ListingMessage(Listing listing)
        {
            _listing = listing;
        }
    }
}
