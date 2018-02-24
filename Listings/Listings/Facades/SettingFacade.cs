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


        public ResultObject ImportBackup(string filePath)
        {
            _dbRegistry.CloseAll();

            ResultObject ro = new ResultObject(true);
            try {
                _dbRegistry.Add("temp", (new Db4oObjectContainerFactory()).OpenConnection(filePath));
                IObjectContainer tempDb = _dbRegistry.GetByName("temp");
                IEnumerable<DefaultSettings> x = from DefaultSettings s in tempDb where s.ID == "main" select s;
                DefaultSettings ds = x.FirstOrDefault();
                if (ds != null && ds.SupportedAppVersions.Contains(Bootstrapper.Version)) {
                    ro.AddMessage("Import dat proběhl úspěšně!");
                } else {
                    ro = new ResultObject(false);
                    ro.AddMessage("Import nelze provést. Nesouhlasí verze importovaných dat.");
                }

            } catch (IncompatibleFileFormatException e) {
                ro = new ResultObject(false);
                ro.AddMessage("Import dat selhal. Špatný formát souboru.");

            } catch (DatabaseReadOnlyException e) {
                ro = new ResultObject(false);
                ro.AddMessage("Import dat selhal. Nelze importovat databázi jen pro čtení.");

            } catch (Exception e) {
                ro = new ResultObject(false);
                ro.AddMessage("Zvolená data nelze importovat.");
            }

            _dbRegistry.CloseAll();

            if (!ro.Success) {
                _dbRegistry.Add(Db4oObjectContainerFactory.MAIN_DATABASE_NAME, (new Db4oObjectContainerFactory()).Create(Db4oObjectContainerFactory.MAIN_DATABASE_NAME));
                return ro;
            }
            
            DateTime now = DateTime.Now;
            string directory = Db4oObjectContainerFactory.GetDatabaseDirectoryPath();

            string activeDbFilePath = Path.Combine(directory, Db4oObjectContainerFactory.MAIN_DATABASE_FILE_NAME);

            string lastWorkingDbFilePath = string.Format("{0}_{1}_{2}_{3}_{4}_{5}.{6}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, Db4oObjectContainerFactory.DATABASE_EXTENSION);
            string lastWorkingBackupPath = Path.Combine(directory, lastWorkingDbFilePath);

            File.Move(activeDbFilePath, lastWorkingBackupPath);

            File.Copy(filePath, activeDbFilePath);

            _dbRegistry.Add(Db4oObjectContainerFactory.MAIN_DATABASE_NAME, (new Db4oObjectContainerFactory()).Create(Db4oObjectContainerFactory.MAIN_DATABASE_NAME));

            return ro;
        }


        private ResultObject CreateConnection(string connectionName)
        {
            ResultObject ro;
            try {
                IObjectContainer db = new Db4oObjectContainerFactory().Create(connectionName);
                IEnumerable<DefaultSettings> x = from DefaultSettings s in db where s.ID == "main" select s;
                DefaultSettings ds = x.FirstOrDefault();
                if (ds != null && ds.SupportedAppVersions.Contains(Bootstrapper.Version)) {
                    ro = new ResultObject(true, db);
                    ro.AddMessage("Import dat proběhl úspěšně!");
                } else {
                    ro = new ResultObject(false);
                    ro.AddMessage("Import nelze provést. Nesouhlasí verze importovaných dat.");
                }

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

            return ro;
        }

    }
}
