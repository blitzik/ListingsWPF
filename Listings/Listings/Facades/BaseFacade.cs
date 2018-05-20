using Db4objects.Db4o;
using Listings.Services;
using Perst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Facades
{
    abstract public class BaseFacade
    {
        protected Storage _storage;
        protected Storage Storage
        {
            get { return _storage; }
        }


        protected Root _root;
        protected Root Root
        {
            get { return _root; }
        }


        public BaseFacade(Storage db)
        {
            _storage = db;
            _root = db.Root as Root;
        }
    }
}
