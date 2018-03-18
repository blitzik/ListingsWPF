using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    public class Manifest
    {
        private string _version;
        public string Version
        {
            get { return _version; }
        }


        private string _currentVersionCheckUrl;
        public string CurrentVersionCheckUrl
        {
            get { return _currentVersionCheckUrl; }
        }


        private string _patchUrl;
        public string PatchUrl
        {
            get { return _patchUrl; }
        }


        public Manifest(string version, string currentVersionCheckUrl, string patchUrl)
        {
            _version = version;
            _currentVersionCheckUrl = currentVersionCheckUrl;
            _patchUrl = patchUrl;
        }


        public int Compare(string version)
        {
            double currentVersion = ConvertVersionNumber(Version);
            double otherVersion = ConvertVersionNumber(version);

            if (currentVersion > otherVersion) {
                return 1;

            } else if (currentVersion < otherVersion) {
                return -1;

            } else {
                return 0;
            }

        }


        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Version)) {
                return false;
            }


            if (string.IsNullOrEmpty(CurrentVersionCheckUrl)) {
                return false;
            }

            if (string.IsNullOrEmpty(PatchUrl)) {
                return false;
            }

            return true;
        }


        private double ConvertVersionNumber(string version)
        {
            string[] versionNumbers = version.Split('.');
            int l = versionNumbers.Length;

            double versionNumber = 0;
            foreach (string vn in versionNumbers) {
                versionNumber += int.Parse(vn) + Math.Pow(10, l);
                l--;
            }

            return versionNumber;
        }

    }
}
