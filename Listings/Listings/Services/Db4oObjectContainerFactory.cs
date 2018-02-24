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
        public const string DATABASE_EXTENSION = "evdo";
        public const string MAIN_DATABASE_NAME = "data";
        public const string MAIN_DATABASE_FILE_NAME = MAIN_DATABASE_NAME + "." + DATABASE_EXTENSION;


        public IObjectContainer Create(string databaseName)
        {
            string filePath = Path.Combine(GetDatabaseDirectoryPath(), databaseName + "." + DATABASE_EXTENSION);
            IObjectContainer db = Db4oEmbedded.OpenFile(PrepareConfig(), filePath);
            
            return db;
        }


        public IObjectContainer OpenConnection(string filePath)
        {
            return Db4oEmbedded.OpenFile(PrepareConfig(), filePath);
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


        public static string GetDatabaseDirectoryPath()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "_evidooApp");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}
