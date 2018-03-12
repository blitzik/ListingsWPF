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
        
        private static bool _isAppFinished = false;


        static void Main(string[] args)
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false)) {
                return;
            }

            Application app = new Application();
            app.OnApplicationTermination += (object sender, EventArgs a) =>
            {
                _isAppFinished = true;
            };
            app.Start();

            string appBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string updateBasePath = Path.Combine(appBasePath, "update");

            string localManifestFilePath = Path.Combine(appBasePath, "manifest.json");

            VersionChecker versionChecker = new VersionChecker();
            Updater updater = new Updater();
            int updateCheckTick = 300; // check every 5 minutes
            // intentional subtraction -> we want to check for update right when application starts, we dont want to wait 5 minutes for first check
            long nextUpdateCheckTime = DateTimeOffset.Now.ToUnixTimeSeconds() - updateCheckTick;
            while (_isAppFinished == false) {
                if (nextUpdateCheckTime <= DateTimeOffset.Now.ToUnixTimeSeconds()) {
                    if (versionChecker.IsNewVersionAvailable(localManifestFilePath)) {
                        updater.UpdateApp(localManifestFilePath, appBasePath, updateBasePath);
                    }
                    nextUpdateCheckTime = DateTimeOffset.Now.ToUnixTimeSeconds() + updateCheckTick;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
