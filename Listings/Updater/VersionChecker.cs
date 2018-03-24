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
    public class VersionChecker
    {
        public const string ACCESS_TOKEN = "f4963c8a-8a6f-4d0a-b54b-4c0ab3b1141e";
        public const string REMOTE_MANIFEST_BASE_URI = "https://update.vycetky.eu/manifest";


        public VersionChecker()
        {
        }


        public bool IsNewVersionAvailable(Manifest localManifest)
        {
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(localManifest.CurrentVersionCheckUrl);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers["security-token-code"] = ACCESS_TOKEN;
               
                using (System.IO.Stream s = request.GetResponse().GetResponseStream()) {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(s)) {
                        string msg = sr.ReadToEnd();
                        ServerMessage m = JsonConvert.DeserializeObject<ServerMessage>(JToken.Parse(msg).ToString());
                        if (!m.Code.Equals("0")) {
                            return false;

                        } else {
                            if (localManifest.Compare(m.Message) >= 0) {
                                return false;
                            }
                        }
                    }
                }

            } catch (WebException we) {
                return false;
            
            } catch (JsonException je) {
                return false;
            }

            return true;
        }


        public bool DownloadManifestFile(string localManifestFilePath)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string manifestUri = string.Format("{0}?v={1}", REMOTE_MANIFEST_BASE_URI, version.ToString());
            try {
                using (WebClient client = new WebClient()) {
                    client.Headers["security-token-code"] = ACCESS_TOKEN;
                    client.DownloadFile(manifestUri, localManifestFilePath);
                }

            } catch (WebException we) {
                return false;
            }
            
            return true;
        }
    }
}
