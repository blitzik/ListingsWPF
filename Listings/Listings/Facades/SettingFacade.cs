﻿using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Listings.Domain;
using Listings.Utils;
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


        public void SaveDefaultSetting(DefaultSettings setting)
        {
            _db.Store(setting);
            _db.Commit();
        }


        public DefaultSettings GetDefaultSettings()
        {
            IEnumerable<DefaultSettings> x = from DefaultSettings ds in _db select ds;
            DefaultSettings settings = x.FirstOrDefault();
            if (settings == null) {
                settings = new DefaultSettings();
                SaveDefaultSetting(settings);
            }

            _db.Activate(settings, 4);
            
            return settings;
        }
    }
}
