using Db4objects.Db4o;
using Perst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Services
{
    public class ObjectContainerRegistry
    {
        private Dictionary<string, Storage> _objectContainers;


        public ObjectContainerRegistry()
        {
            _objectContainers = new Dictionary<string, Storage>();
        }


        public void Add(string name, Storage db)
        {
            _objectContainers.Add(name, db);
        }


        public void Close(string name)
        {
            if (!_objectContainers.ContainsKey(name)) {
                return;
            }
            _objectContainers[name].Close();
            _objectContainers.Remove(name);
        }


        public void CloseAll()
        {
            foreach (KeyValuePair<string, Storage> entry in _objectContainers) {
                entry.Value.Close();
            }

            _objectContainers.Clear();
        }


        public bool ContainsByName(string name)
        {
            return _objectContainers.ContainsKey(name);
        }


        public Storage GetByName(string name)
        {
            if (_objectContainers.ContainsKey(name)) {
                return _objectContainers[name];
            }

            return null;
        } 


        public int Count()
        {
            return _objectContainers.Count;
        }
    }
}
