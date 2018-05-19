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
        protected string _databaseName = Db4oObjectContainerFactory.MAIN_DATABASE_NAME;


        public BaseFacade(Storage db)
        {
            _storage = db;
        }


        /*protected IObjectContainer Db(string name = null)
        {
            if (string.IsNullOrEmpty(name)) {
                return _dbRegistry.GetByName(_databaseName);
            }

            return _dbRegistry.GetByName(name);
        }*/
    }
}
