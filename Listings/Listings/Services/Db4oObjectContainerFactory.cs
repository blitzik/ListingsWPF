using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using System.IO;
using Listings.Domain;

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
            config.Common.ActivationDepth = 4;
            config.Common.UpdateDepth = 8;

            config.Common.ObjectClass(typeof(Listing)).ObjectField("_year").Indexed(true);
  
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
