using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listings.Domain;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Ext;
using System.IO;
using System.Reflection;

namespace Listings.Services.Backup
{
    public class BackupImport : IBackupImport
    {
        private readonly Db4oObjectContainerFactory _factory;


        public BackupImport(Db4oObjectContainerFactory factory)
        {
            _factory = factory;
        }


        public ResultObject Import(string importFilePath, string appDBDirectory, string activeDBName, string activeDBExtension)
        {
            ResultObject ro;
            try {
                IObjectContainer db = _factory.OpenConnection(importFilePath);

                ro = new ResultObject(true, db);
                ro.AddMessage("Import dat proběhl úspěšně!");

                db.Close();

            } catch (IncompatibleFileFormatException e) {
                ro = new ResultObject(false);
                ro.AddMessage("Import dat selhal. Špatný formát dat.");

            } catch (DatabaseReadOnlyException e) {
                ro = new ResultObject(false);
                ro.AddMessage("Import dat selhal. Nelze importovat data jen pro čtení.");

            } catch (Exception e) {
                ro = new ResultObject(false);
                ro.AddMessage("Zvolená data nelze importovat.");
            }

            if (!ro.Success) {
                return ro;
            }

            DateTime now = DateTime.Now;

            string activeDbFilePath = Path.Combine(appDBDirectory, activeDBName + "." + activeDBExtension);

            string oldDbBackupFileName = string.Format("backup_{0}_{1}_{2}_{3}_{4}_{5}_v{6}.{7}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", "-"), activeDBExtension);
            string lastWorkingDbBackupPath = Path.Combine(appDBDirectory, oldDbBackupFileName);

            File.Move(activeDbFilePath, lastWorkingDbBackupPath);
            File.Copy(importFilePath, activeDbFilePath);

            return ro;
        }
    }
}
