using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Updater
{
    class Program
    {
        public static Mutex mutex = new Mutex(false, "edf36d8e-adca-4c99-bd6f-1785f79e75c6");


        static void Main(string[] args)
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false) || AppDomain.CurrentDomain.IsDefaultAppDomain() == true) {
                return;
            }

            string appBasePath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            string updateBasePath = Path.Combine(appBasePath, "patch");
            if (Directory.Exists(updateBasePath)) {
                Directory.Delete(updateBasePath);
            }

            string localManifestFilePath = Path.Combine(appBasePath, "manifest.json");

            VersionChecker versionChecker = new VersionChecker();
            Updater updater = new Updater();

            bool manifestExists;
            do {
                try {
                    manifestExists = File.Exists(localManifestFilePath);
                    if (!manifestExists) {
                        if (!versionChecker.DownloadManifestFile(localManifestFilePath)) {
                            Thread.Sleep(60000);
                            continue;
                        }
                    }

                    try {
                        Manifest manifest = JsonConvert.DeserializeObject<Manifest>(JToken.Parse(File.ReadAllText(localManifestFilePath)).ToString());
                        if (!manifest.IsValid()) {
                            File.Delete(localManifestFilePath);
                            continue;
                        }

                        while (true) {
                            if (versionChecker.IsNewVersionAvailable(manifest)) {
                                updater.UpdateApp(manifest, appBasePath, updateBasePath);
                                manifest = JsonConvert.DeserializeObject<Manifest>(JToken.Parse(File.ReadAllText(localManifestFilePath)).ToString());
                            }
                            Thread.Sleep(60000 * 5);
                        }

                    } catch (FileNotFoundException fex) {
                        continue;

                    } catch (JsonException jex) {
                        File.Delete(localManifestFilePath);
                        continue;
                    }

                } catch (IOException ioex) {
                    Thread.Sleep(5000);
                    continue;
                }

            } while (true);
        }
    }
}
