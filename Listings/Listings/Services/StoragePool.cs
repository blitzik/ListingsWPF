using Db4objects.Db4o;
using Perst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Services
{
    public class StoragePool
    {
        private Dictionary<string, Storage> _storages;


        public StoragePool()
        {
            _storages = new Dictionary<string, Storage>();
        }


        public void Add(string name, Storage db)
        {
            _storages.Add(name, db);
        }


        public void Close(string name)
        {
            if (!_storages.ContainsKey(name)) {
                return;
            }
            _storages[name].Close();
            _storages.Remove(name);
        }


        public void CloseAll()
        {
            foreach (KeyValuePair<string, Storage> entry in _storages) {
                entry.Value.Close();
            }

            _storages.Clear();
        }


        public bool ContainsByName(string name)
        {
            return _storages.ContainsKey(name);
        }


        public Storage GetByName(string name)
        {
            if (_storages.ContainsKey(name)) {
                return _storages[name];
            }

            return null;
        } 


        public int Count()
        {
            return _storages.Count;
        }
    }
}
