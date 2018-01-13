using Db4objects.Db4o;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Services
{
    public class ObjectContainerRegistry
    {
        private Dictionary<string, IObjectContainer> _objectContainers;


        public ObjectContainerRegistry()
        {
            _objectContainers = new Dictionary<string, IObjectContainer>();
        }


        public void Add(string name, IObjectContainer db)
        {
            _objectContainers.Add(name, db);
        }


        public bool Close(string name)
        {
            if (!_objectContainers.ContainsKey(name)) {
                return false;
            }

            bool result = _objectContainers[name].Close();
            _objectContainers.Remove(name);

            return result;
        }


        public void CloseAll()
        {
            foreach (KeyValuePair<string, IObjectContainer> entry in _objectContainers) {
                entry.Value.Close();
            }

            _objectContainers.Clear();
        }


        public bool ContainsByName(string name)
        {
            return _objectContainers.ContainsKey(name);
        }


        public IObjectContainer GetByName(string name)
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
