using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using System.IO;
using Listings.Domain;
using Listings.Utils;

namespace Listings.Services
{
    public class Db4oObjectContainerFactory
    {
        public IObjectContainer Create(string databaseName)
        {
            string filePath = Path.Combine(GetFilePath(), databaseName);
            IObjectContainer db = Db4oEmbedded.OpenFile(PrepareConfig(), filePath);
            
            return db;
        }


        private IEmbeddedConfiguration PrepareConfig()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();

            config.File.BlockSize = 8;
            config.Common.ActivationDepth = 2;
            config.Common.UpdateDepth = 10;
            config.Common.CallConstructors = false;
            config.Common.TestConstructors = false;
            config.Common.Callbacks = false;
            //config.Common.DetectSchemaChanges = false;

            config.Common.ObjectClass(typeof(Listing)).ObjectField("_year").Indexed(true);

            config.Common.ObjectClass(typeof(Listing)).CascadeOnDelete(true);
            config.Common.ObjectClass(typeof(Listing)).ObjectField("_employer").CascadeOnDelete(false);

            config.Common.ObjectClass(typeof(DefaultSettings)).ObjectField("_id").Indexed(true);

            config.Common.ObjectClass(typeof(ListingItem)).CascadeOnDelete(true);

            config.Common.ObjectClass(typeof(TimeSetting)).CascadeOnDelete(true);


            return config;
        }


        private string GetFilePath()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "_BlitzikListings");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}
