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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Updater
{
    class Program
    {
        public static Mutex mutex = new Mutex(false, "edf36d8e-adca-4c99-bd6f-1785f79e75c6");

        private VersionChecker _versionChecker;
        private Updater _updater;

        private string _appBasePath;


        static void Main(string[] args)
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false) || AppDomain.CurrentDomain.IsDefaultAppDomain() == true) {
                return;
            }

            Program p = new Program(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location));

            p.Run();
        }


        public Program(string appBasePath)
        {
            _versionChecker = new VersionChecker();
            _updater = new Updater();

            _appBasePath = appBasePath;
        }


        public void Run()
        {
            string updateBasePath = Path.Combine(_appBasePath, "patch");
            if (Directory.Exists(updateBasePath)) {
                Directory.Delete(updateBasePath);
            }

            string localManifestFilePath = Path.Combine(_appBasePath, "manifest.xml");

            Thread t = new Thread(async () => {
                Manifest manifest;
                while (true) {
                    if (!File.Exists(localManifestFilePath)) {
                        await DownloadManifest(localManifestFilePath);
                    }

                    manifest = GetManifest(localManifestFilePath);
                    if (manifest == null) {
                        File.Delete(localManifestFilePath);
                        continue;
                    }

                    while (true) {
                        if (_versionChecker.IsNewVersionAvailable(manifest)) {
                            _updater.UpdateApp(manifest, _appBasePath, updateBasePath);
                            break; // we need to load new version of manifest
                        }
                        Thread.Sleep(60000 * 5);
                    }
                }
            });
            t.IsBackground = true;
            t.Start();

            t.Join();
        }


        private Manifest GetManifest(string localManifestFilePath)
        {
            bool isManifestValid = true;
            XDocument doc;
            using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Updater.manifest.xsd")) {
                XmlSchemaSet xmlSchema = new XmlSchemaSet();
                xmlSchema.Add(string.Empty, XmlReader.Create(resourceStream));

                try {
                    doc = XDocument.Parse(File.ReadAllText(localManifestFilePath));

                } catch (XmlException e) {
                    return null;
                }

                doc.Validate(xmlSchema, (object sender, ValidationEventArgs e) => {                    
                    isManifestValid = false;
                });
            }

            if (isManifestValid == false) {
                return null;
            }

            IEnumerable<Manifest> result = from XElement el in doc.Descendants("Manifest")
                                           select new Manifest(
                                               el.Element("Version").Value,
                                               el.Element("CurrentVersionCheckUrl").Value,
                                               el.Element("PatchUrl").Value
                                           );

            return result.FirstOrDefault();
        }


        private Task<bool> DownloadManifest(string localManifestFilePath)
        {
            Task<bool> tt = Task<bool>.Factory.StartNew(() => {
                while (!_versionChecker.DownloadManifestFile(localManifestFilePath)) {
                    Thread.Sleep(60000);
                }
                return true;
            });

            return tt;
        }
    }
}
