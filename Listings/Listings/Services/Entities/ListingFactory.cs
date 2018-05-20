using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listings.Domain;
using Perst;

namespace Listings.Services.Entities
{
    public class ListingFactory : IListingFactory
    {
        private Storage _storage;


        public ListingFactory(Storage storage)
        {
            _storage = storage;
        }


        public Listing Create(int year, int month)
        {
            return new Listing(_storage, year, month);
        }
    }
}
