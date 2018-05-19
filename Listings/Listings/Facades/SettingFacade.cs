using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Linq;
using Listings.Domain;
using Listings.Services;
using Listings.Services.Backup;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Facades
{
    public class SettingFacade// : BaseFacade
    {
        private readonly Db4oObjectContainerFactory _dbFactory;
        private IBackupImport _backupImport;


        public SettingFacade(Db4oObjectContainerFactory dbFactory, ObjectContainerRegistry dbRegistry, IBackupImport backupImport)// : base (dbRegistry)
        {
            _dbFactory = dbFactory;
            _backupImport = backupImport;
        }


        public void SaveDefaultSetting(DefaultSettings setting)
        {
            //Db().Store(setting);
            //Db().Commit();
        }


        public DefaultSettings GetDefaultSettings()
        {
            //IEnumerable<DefaultSettings> x = from DefaultSettings ds in Db() where ds.ID == "main" select ds;
            DefaultSettings settings/* = x.FirstOrDefault()*/;
            //if (settings == null) {
                settings = new DefaultSettings(DefaultSettings.MAIN_SETTINGS_ID);
                //SaveDefaultSetting(settings);
            //}

            //Db().Activate(settings, 4);

            //settings.OnTimeSettingUpdate += OnTimeSettingUpdate;
            
            return settings;
        }


        private void OnTimeSettingUpdate(object sender, TimeSetting oldSetting, TimeSetting newSetting)
        {
            //Db().Delete(oldSetting);
        }


        public void BackupData(string filePath)
        {
            //Db().Ext().Backup(filePath);
        }


        public ResultObject ImportBackup(string filePath)
        {
            //_dbRegistry.CloseAll();

            ResultObject ro = _backupImport.Import(filePath, Db4oObjectContainerFactory.GetDatabaseDirectoryPath(), Db4oObjectContainerFactory.MAIN_DATABASE_NAME, Db4oObjectContainerFactory.DATABASE_EXTENSION);

            //_dbRegistry.Add(Db4oObjectContainerFactory.MAIN_DATABASE_NAME, _dbFactory.Create(Db4oObjectContainerFactory.MAIN_DATABASE_NAME));

            return ro;
        }

    }
}
