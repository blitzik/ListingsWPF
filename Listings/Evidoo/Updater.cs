using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Evidoo
{
    public class Updater
    {
        public void UpdateApp(string localManifestFilePath, string appBasepath, string updateTempDirectoryPath)
        {
            string localManifestJson = File.ReadAllText(localManifestFilePath);
            Manifest localManifest = JsonConvert.DeserializeObject<Manifest>(JToken.Parse(localManifestJson).ToString());

            using (WebClient client = new WebClient()) {
                if (Directory.Exists(updateTempDirectoryPath)) {
                    Directory.Delete(updateTempDirectoryPath, true);
                }
                Directory.CreateDirectory(updateTempDirectoryPath);
                client.Headers["security-token-code"] = localManifest.AccessToken;
                try {
                    string downloadedUpdatePath = Path.Combine(updateTempDirectoryPath, "current.zip");
                    client.DownloadFile(localManifest.PayloadUri, downloadedUpdatePath);

                    string extractedFilesPath = Path.Combine(updateTempDirectoryPath, "new");
                    ZipFile.ExtractToDirectory(downloadedUpdatePath, extractedFilesPath);

                    string backupDirectoryPath = Path.Combine(appBasepath, "backup");
                    if (Directory.Exists(backupDirectoryPath)) {
                        Directory.Delete(backupDirectoryPath, true);
                    } else {
                        Directory.CreateDirectory(backupDirectoryPath);
                    }
                    ReplaceFiles(extractedFilesPath, appBasepath, backupDirectoryPath);

                } catch (InvalidDataException idex) {
                    if (Directory.Exists(updateTempDirectoryPath)) {
                        Directory.Delete(updateTempDirectoryPath, true);
                    }
                    // todo log
                } catch (Exception ex) {
                    // todo log
                }
            }
        }


        private void ReplaceFiles(string sourceDirectoryPath, string destinationDirectoryPath, string backupPathForOriginalDirectory)
        {
            // [F:\dev\c#\Listings\Listings\Launcher\bin\Debug\update\new]\folder\file.dll - source
            // [F:\dev\c#\Listings\Listings\Launcher\bin\Debug]\file.dll - destination
            IEnumerable<string> filePaths = ScanDirectory(sourceDirectoryPath);
            foreach (string sourceFilePath in filePaths) {
                FileInfo sourceFile = new FileInfo(sourceFilePath);
                string x = sourceFilePath.Substring(sourceDirectoryPath.Length + 1);
                string s = Path.Combine(destinationDirectoryPath, x);
                FileInfo destFile = new FileInfo(s);
                if (destFile.Exists) {
                    FileInfo a = new FileInfo(Path.Combine(backupPathForOriginalDirectory, x));
                    if (!Directory.Exists(a.DirectoryName)) {
                        Directory.CreateDirectory(a.DirectoryName);
                    }
                    sourceFile.Replace(destFile.FullName, a.FullName);
                } else {
                    sourceFile.MoveTo(sourceFile.Name);
                }
            }
        }


        private IEnumerable<string> ScanDirectory(string directory)
        {
            IEnumerable<string> filePaths = Directory.GetFiles(directory);
            IEnumerable<string> directoryPaths = Directory.GetDirectories(directory);
            foreach (string directoryPath in directoryPaths) {
                filePaths = filePaths.Concat(ScanDirectory(directoryPath));
            }

            return filePaths;
        }
    }
}
