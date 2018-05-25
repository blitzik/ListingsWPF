using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Linq;
using Listings.Domain;
using Listings.Services;
using Listings.Services.Backup;
using Listings.Utils;
using Perst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Facades
{
    public class SettingFacade : BaseFacade
    {
        private IBackupImport _backupImport;


        public SettingFacade(StoragePool db, IBackupImport backupImport) : base (db)
        {
            _backupImport = backupImport;
        }


        public ResultObject CreateDefaultSettings(string identifier)
        {
            DefaultSettings ds = new DefaultSettings(identifier);
            ResultObject ro;
            if (Root().DefaultSettings.Put(ds) == false) {
                ro = new ResultObject(false);
                ro.AddMessage(string.Format("Nastavení s názem \"{0}\" již existuje.", identifier));
            } else {
                ro = new ResultObject(true, ds);
                Storage().Commit();
            }

            return ro;
        }


        public void UpdateDefaultSettings(DefaultSettings settings)
        {
            Storage().Modify(settings);
            Storage().Commit();
        }


        public DefaultSettings GetDefaultSettings()
        {
            DefaultSettings ds = Root().DefaultSettings.Get(DefaultSettings.MAIN_SETTINGS_ID);
            if (ds == null) {
                ResultObject ro = CreateDefaultSettings(DefaultSettings.MAIN_SETTINGS_ID);
                if (ro.Success) {
                    ds = (DefaultSettings)ro.Result;
                } // todo
            }
            
            return ds;
        }


        public ResultObject BackupData(string filePath)
        {
            ResultObject ro;
            try {
                Storage().Backup(new FileStream(filePath, FileMode.Open));
                ro = new ResultObject(true);
                ro.AddMessage("Záloha databáze proběhla úspěšně!");

            } catch (IOException e) {
                ro = new ResultObject(false);
                ro.AddMessage("Zálohu databáze nelze dokončit. Došlo k chybě.");
            }

            return ro;
        }


        public ResultObject ImportBackup(string filePath)
        {
            return _backupImport.Import(filePath, PerstStorageFactory.GetDatabaseDirectoryPath(), PerstStorageFactory.MAIN_DATABASE_NAME, PerstStorageFactory.DATABASE_EXTENSION);
        }

    }
}
