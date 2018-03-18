using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Evidoo
{
    public class ApplicationsLauncher
    {
        private Dictionary<string, IApplication> _apps;
        private List<IApplication> _appsInOrder;


        public ApplicationsLauncher()
        {
            _apps = new Dictionary<string, IApplication>();
            _appsInOrder = new List<IApplication>();
        }


        public Application Add(string appName, bool shadowCopyFiles)
        {
            if (_apps.ContainsKey(appName)) {
                throw new Exception("Application name must be unique.");
            }
            Application app = new Application(appName, shadowCopyFiles);
            _apps.Add(appName, app);
            _appsInOrder.Add(app);

            return app;
        }


        public bool AreAllAppsFinished()
        {
            foreach (IApplication app in _apps.Values) {
                if (app.State != ApplicationState.FINISHED) {
                    return false;
                }
            }

            return true;
        }


        public void StartApp(string appName, Action<Thread> threadModifier)
        {
            if (!_apps.ContainsKey(appName)) {
                throw new Exception(string.Format("Launcher does NOT contain an Application called \"{{0}\"", appName));
            }
            _apps[appName].Start(threadModifier);
        }


        public void StartApps(Dictionary<string, Action<Thread>> threadModifiers)
        {
            Action<Thread> tm;
            foreach (IApplication app in _apps.Values) {
                tm = null;
                if (threadModifiers.ContainsKey(app.AppName)) {
                    tm = threadModifiers[app.AppName];
                }
                app.Start(tm);
            }
        }

    }
}
