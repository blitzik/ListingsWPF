using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Linq;
using Listings.Domain;
using Listings.Services;
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
    public class SettingFacade : BaseFacade
    {
        public SettingFacade(ObjectContainerRegistry dbRegistry)
        {
            _dbRegistry = dbRegistry;
        }


        public void SaveDefaultSetting(DefaultSettings setting)
        {
            Db().Store(setting);
            Db().Commit();
        }


        public DefaultSettings GetDefaultSettings()
        {
            IEnumerable<DefaultSettings> x = from DefaultSettings ds in Db() where ds.ID == "main" select ds;
            DefaultSettings settings = x.FirstOrDefault();
            if (settings == null) {
                settings = new DefaultSettings("main");
                SaveDefaultSetting(settings);
            }

            Db().Activate(settings, 4);

            settings.OnTimeSettingUpdate += OnTimeSettingUpdate;
            
            return settings;
        }


        private void OnTimeSettingUpdate(object sender, TimeSetting oldSetting, TimeSetting newSetting)
        {
            Db().Delete(oldSetting);
        }


        public void BackupData(string filePath)
        {
            Db().Ext().Backup(filePath);
        }


        public ResultObject ImportBackup(string fileName)
        {
            _dbRegistry.CloseAll();

            string directory = Db4oObjectContainerFactory.GetDatabaseDirectoryPath();

            DateTime now = DateTime.Now;

            string activeDbFilePath = Path.Combine(directory, Db4oObjectContainerFactory.MAIN_DATABASE_FILE_NAME);
            string lastWorkingBackupPath = Path.Combine(directory, string.Format("lastWorkingBackup.{0}", Db4oObjectContainerFactory.DATABASE_EXTENSION));

            if (File.Exists(activeDbFilePath)) {
                File.Delete(lastWorkingBackupPath);
                File.Move(activeDbFilePath, lastWorkingBackupPath);
            }
            File.Copy(fileName, activeDbFilePath);

            ResultObject ro = new ResultObject(true);
            try {
                _dbRegistry.Add(Db4oObjectContainerFactory.MAIN_DATABASE_NAME, (new Db4oObjectContainerFactory()).Create(Db4oObjectContainerFactory.MAIN_DATABASE_NAME));
                ro.AddMessage("Import dat proběhl úspěšně!");

            } catch (IncompatibleFileFormatException e) {
                ro = new ResultObject(false);
                ro.AddMessage("Import dat selhal. Špatný formát souboru.");

            } catch (DatabaseReadOnlyException e) {
                ro = new ResultObject(false);
                ro.AddMessage("Import dat selhal. Nelze importovat databázi jen pro čtení.");

            } catch (Exception e) {
                ro = new ResultObject(false);
                ro.AddMessage("Při importu dat došlo k chybě.");
            }

            if (!ro.Success) {
                File.Delete(activeDbFilePath);

                ImportBackup(lastWorkingBackupPath);
            }

            return ro;
        }

    }
}
