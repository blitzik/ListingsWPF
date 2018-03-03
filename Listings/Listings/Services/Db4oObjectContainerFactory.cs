using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;
using System.IO;
using Listings.Domain;
using Listings.Utils;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Constraints;

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
            return OpenConnection(filePath);
        }


        public IObjectContainer OpenConnection(string filePath)
        {
            bool dbExists = File.Exists(filePath);
            
            IObjectContainer c = Db4oEmbedded.OpenFile(PrepareConfig(), filePath);
            if (dbExists == false) {
                DbVersion version = new DbVersion(DbVersion.UNIQUE_KEY, Bootstrapper.APP_VERSION);
                c.Store(version);
                c.Commit();
            }

            return c;
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

            config.Common.ObjectClass(typeof(DbVersion)).ObjectField("_id").Indexed(true);
            config.Common.Add(new UniqueFieldValueConstraint(typeof(DbVersion), "_id"));

            config.Common.ObjectClass(typeof(Listing)).ObjectField("_year").Indexed(true);

            config.Common.ObjectClass(typeof(Listing)).CascadeOnDelete(true);
            config.Common.ObjectClass(typeof(Listing)).ObjectField("_employer").CascadeOnDelete(false);

            config.Common.ObjectClass(typeof(DefaultSettings)).ObjectField("_id").Indexed(true);
            config.Common.Add(new UniqueFieldValueConstraint(typeof(DefaultSettings), "_id"));

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
