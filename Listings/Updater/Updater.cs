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

namespace Updater
{
    public class Updater
    {
        public void UpdateApp(Manifest localManifest, string appBasepath, string updateTempDirectoryPath)
        {
            if (Directory.Exists(updateTempDirectoryPath)) {
                Directory.Delete(updateTempDirectoryPath, true);
            }
            Directory.CreateDirectory(updateTempDirectoryPath);

            string tempBackupPath = Path.Combine(appBasepath, "backup");
            if (Directory.Exists(tempBackupPath)) {
                Directory.Delete(tempBackupPath);
            }
            Directory.CreateDirectory(tempBackupPath);

            using (WebClient client = new WebClient()) {
                client.Headers["security-token-code"] = VersionChecker.ACCESS_TOKEN;
                try {
                    string downloadedUpdatePath = Path.Combine(updateTempDirectoryPath, "patch.zip");
                    client.DownloadFile(localManifest.PatchUrl, downloadedUpdatePath);

                    string extractedFilesPath = Path.Combine(updateTempDirectoryPath, "new");
                    ZipFile.ExtractToDirectory(downloadedUpdatePath, extractedFilesPath);

                    CopyFiles(extractedFilesPath, appBasepath, tempBackupPath);

                } catch (InvalidDataException idex) {
                    // todo log

                } catch (WebException we) {
                    // todo log

                } catch (Exception ex) {
                    // todo log
                }

                if (Directory.Exists(updateTempDirectoryPath)) {
                    Directory.Delete(updateTempDirectoryPath, true);
                }
            }
        }


        private void CopyFiles(string sourceDirectoryPath, string destinationDirectoryPath, string tempBackupPath)
        {
            // [F:\dev\c#\Listings\Listings\Launcher\bin\Debug\update\new]\folder\file.dll - source
            // [F:\dev\c#\Listings\Listings\Launcher\bin\Debug]\file.dll - destination
            IEnumerable<string> filePaths = ScanDirectory(sourceDirectoryPath);
            foreach (string sourceFilePath in filePaths) {
                FileInfo sourceFile = new FileInfo(sourceFilePath);
                string relativeSourceFilePath = sourceFilePath.Substring(sourceDirectoryPath.Length + 1);
                string destinationFilePath = Path.Combine(destinationDirectoryPath, relativeSourceFilePath);
                FileInfo destFile = new FileInfo(destinationFilePath);
                if (destFile.Exists) {
                    string relativeDestFilePath = destinationFilePath.Substring(destinationDirectoryPath.Length + 1);
                    FileInfo destTempBackupFile = new FileInfo(Path.Combine(tempBackupPath, relativeDestFilePath));
                    if (!destTempBackupFile.Directory.Exists) {
                        Directory.CreateDirectory(destTempBackupFile.Directory.FullName);
                    }
                    destFile.MoveTo(destTempBackupFile.FullName);
                }
                sourceFile.CopyTo(destinationFilePath);
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
