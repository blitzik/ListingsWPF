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
    public class Application
    {
        private AppDomain _evidooAppDomain;


        public delegate void ApplicationTerminatedHandler(object sender, EventArgs args);
        public event ApplicationTerminatedHandler OnApplicationTermination;
        public void Start()
        {
            string appBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string updateBasepath = Path.Combine(appBasePath, "update");

            Thread t = new Thread(() => {
                AppDomainSetup setup = new AppDomainSetup()
                {
                    ApplicationName = "Listings",
                    ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                    ShadowCopyFiles = true.ToString(),
                    CachePath = Path.Combine(appBasePath, "sc_listings")
                };
                _evidooAppDomain = AppDomain.CreateDomain("ListingsDomain", null, setup);
                _evidooAppDomain.ExecuteAssembly("Listings.exe");

                ApplicationTerminatedHandler handler = OnApplicationTermination;
                if (handler != null) {
                    handler(this, EventArgs.Empty);
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }

    }
}
