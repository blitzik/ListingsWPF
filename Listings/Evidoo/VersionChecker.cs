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
    public class VersionChecker
    {
        public bool IsNewVersionAvailable(string localManifestFilePath)
        {
            if (!File.Exists(localManifestFilePath)) {
                return false;
            }

            try {
                string localManifestJson = File.ReadAllText(localManifestFilePath);
                Manifest localManifest = JsonConvert.DeserializeObject<Manifest>(JToken.Parse(localManifestJson).ToString());

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(localManifest.RemoteManifestUri);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers["security-token-code"] = localManifest.AccessToken;
                try {
                    using (System.IO.Stream s = request.GetResponse().GetResponseStream()) {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s)) {
                            string msg = sr.ReadToEnd();
                            ServerMessage m = JsonConvert.DeserializeObject<ServerMessage>(JToken.Parse(msg).ToString());
                            Manifest remoteManifest = JsonConvert.DeserializeObject<Manifest>(JToken.Parse(m.Message).ToString());
                            if (!m.Code.Equals("0")) {
                                return false;

                            } else {
                                if (!remoteManifest.HasVersionHigherThan(localManifest)) {
                                    return false;
                                }
                            }
                        }
                    }
                } catch (WebException we) {
                    return false;
                }
            } catch (FileNotFoundException fnfe) {
                return false;

            } catch (IOException ioe) {
                return false;

            } catch (JsonException je) {
                return false;
            }

            return true;
        }
    }
}
