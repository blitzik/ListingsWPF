using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evidoo
{
    public class Manifest
    {
        private string _version;
        public string Version
        {
            get { return _version; }
        }


        private string _remoteManifestUri;
        public string RemoteManifestUri
        {
            get { return _remoteManifestUri; }
        }


        private string _accessToken;
        public string AccessToken
        {
            get { return _accessToken; }
        }


        private string _payloadUri;
        public string PayloadUri
        {
            get { return _payloadUri; }
        }


        public Manifest(string version, string remoteManifestUri, string accessToken, string payloadUri)
        {
            _version = version;
            _remoteManifestUri = remoteManifestUri;
            _accessToken = accessToken;
            _payloadUri = payloadUri;
        }


        public bool HasVersionHigherThan(Manifest manifest)
        {
            double currentVersion = ConvertVersionNumber(Version);
            double otherVersion = ConvertVersionNumber(manifest.Version);

            return currentVersion > otherVersion;
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
