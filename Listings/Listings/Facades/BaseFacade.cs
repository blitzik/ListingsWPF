using Db4objects.Db4o;
using Listings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Facades
{
    abstract public class BaseFacade
    {
        protected ObjectContainerRegistry _dbRegistry;
        protected string _databaseName = Db4oObjectContainerFactory.MAIN_DATABASE_NAME;


        protected IObjectContainer Db(string name = null)
        {
            if (string.IsNullOrEmpty(name)) {
                return _dbRegistry.GetByName(_databaseName);
            }

            return _dbRegistry.GetByName(name);
        }
    }
}
