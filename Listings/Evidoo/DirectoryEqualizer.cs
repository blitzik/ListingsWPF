using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidooApp
{
    public static class DirectoryEqualizer
    {
        public static void Equalize(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            // [F:\dev\c#\Listings\Listings\Launcher\bin\Debug\update\new]\folder\file.dll - source
            // [F:\dev\c#\Listings\Listings\Launcher\bin\Debug]\file.dll - destination
            Directory.Delete(destinationDirectoryPath);
            IEnumerable<string> filePaths = ScanDirectory(sourceDirectoryPath);
            foreach (string sourceFilePath in filePaths) {
                FileInfo sourceFile = new FileInfo(sourceFilePath);
                string x = sourceFilePath.Substring(sourceDirectoryPath.Length + 1);
                string s = Path.Combine(destinationDirectoryPath, x);
                FileInfo destFile = new FileInfo(s);
                if (!Directory.Exists(destFile.Directory.FullName)) {
                    Directory.CreateDirectory(destFile.Directory.FullName);
                }
                sourceFile.CopyTo(destFile.FullName);
            }
        }


        public static IEnumerable<string> ScanDirectory(string directory)
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
