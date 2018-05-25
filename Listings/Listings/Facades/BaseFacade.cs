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
        private StoragePool _storagePool;
        protected StoragePool StoragePool
        {
            get { return _storagePool; }
            set { _storagePool = value; }
        }
   

        public BaseFacade(StoragePool storagePool)
        {
            _storagePool = storagePool;
        }


        public Storage Storage(string name = null)
        {
            if (string.IsNullOrEmpty(name)) {
                return StoragePool.GetByName(PerstStorageFactory.MAIN_DATABASE_NAME);
            }
            return StoragePool.GetByName(name);
        }


        public Root Root(string name = null)
        {
            if (string.IsNullOrEmpty(name)) {
                return (Root)StoragePool.GetByName(PerstStorageFactory.MAIN_DATABASE_NAME).Root;
            }
            return (Root)StoragePool.GetByName(name).Root;
        }
    }
}
