using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Facades
{
    public class SettingFacade
    {
        private IObjectContainer _db;


        public SettingFacade(IObjectContainer db)
        {
            _db = db;
        }


        /*public TimeSetting GetTimeSetting()
        {
            var x = from TimeSetting s 
        }*/
    }
}
