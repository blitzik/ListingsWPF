using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Evidoo
{
    class Program
    {
        static Mutex mutex = new Mutex(false, "70fa546b-8b99-4340-a23b-7e04b2a0aceb");


        static void Main(string[] args)
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false)) {
                return;
            }

            string appBaseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string updateBaseDir = Path.Combine(appBaseDir, "patch");
            string shadowCopiesDir = Path.Combine(appBaseDir, "sc");
            string tempBackupDir = Path.Combine(appBaseDir, "backup");
            if (Directory.Exists(tempBackupDir)) {
                Directory.Delete(tempBackupDir, true);
                if (Directory.Exists(shadowCopiesDir)) {
                    Directory.Delete(shadowCopiesDir, true);
                }
            }
            if (Directory.Exists(updateBaseDir)) {
                Directory.Delete(updateBaseDir, true);
            }

            ApplicationsLauncher al = new ApplicationsLauncher();
            Application listingsApp = al.Add("Listings", true);
            Application updaterApp = al.Add("Updater", true);

            al.StartApps(new Dictionary<string, Action<Thread>>() {
                {
                    "Listings", (t) =>
                    {
                        t.SetApartmentState(ApartmentState.STA);
                        t.IsBackground = true;
                    }
                },
                {
                    "Updater", (t) =>
                    {
                        t.SetApartmentState(ApartmentState.STA);
                        t.IsBackground = true;
                    }
                }
            });

            while (true) {
                if (listingsApp.State == ApplicationState.FINISHED && !Directory.Exists(updateBaseDir)) {
                    AppDomain.Unload(updaterApp.AppDomain);
                    break;
                }
                
                Thread.Sleep(5000);
            }
        }
    }
}
