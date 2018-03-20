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
    public enum ApplicationState
    {
        PREPARED,
        STARTED,
        FINISHED
    }


    public class Application : IApplication
    {
        private AppDomain _appDomain;
        public AppDomain AppDomain
        {
            get { return _appDomain; }
        }


        private string _appDomainName;
        public string AppDomainName
        {
            get { return _appDomainName; }
        }


        private string _appName;
        public string AppName
        {
            get { return _appName; }
        }


        private bool _shadowCopyFiles;
        public bool ShadowCopyFiles
        {
            get { return _shadowCopyFiles; }
        }


        private ApplicationState _state;
        public ApplicationState State
        {
            get { return _state; }
        }


        private string _defaultBasePath;
        private string _appBasePath;
        private string _shadowCopiesDirectory;
        private string _appExecutable;



        private Thread _t;


        public Application(string appName, bool shadowCopyFiles)
        {
            _appName = appName;
            _shadowCopyFiles = shadowCopyFiles;
            _appDomainName = string.Format("{0}Domain", appName);

            _defaultBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _appBasePath = Path.Combine(Path.Combine(_defaultBasePath, "bin"), _appName);
            _shadowCopiesDirectory = Path.Combine(_defaultBasePath, "sc");
            _appExecutable = Path.Combine(_appBasePath, string.Format("{0}.exe", _appName));

            _t = PrepareThread();

            _state = ApplicationState.PREPARED;
        }


        public delegate void TerminatedApplicationHandler(object sender, EventArgs args);
        public event TerminatedApplicationHandler OnTerminatedApplication;
        private Thread PrepareThread()
        {
            Thread t = new Thread(() => {
                string appPath = Path.Combine(_defaultBasePath, AppName);
                AppDomainSetup setup = new AppDomainSetup() { ApplicationName = AppName };
                setup.ShadowCopyFiles = ShadowCopyFiles.ToString();
                setup.ApplicationBase = appPath;
                if (ShadowCopyFiles == true) {
                    setup.CachePath = Path.Combine(_shadowCopiesDirectory, AppName);
                }
                _appDomain = AppDomain.CreateDomain(AppDomainName, null, setup);
                _appDomain.ExecuteAssembly(_appExecutable);

                _state = ApplicationState.FINISHED;
                TerminatedApplicationHandler handler = OnTerminatedApplication;
                if (handler != null) {
                    handler(this, EventArgs.Empty);
                }
            });

            return t;
        }


        public void Start(Action<Thread> threadModifier)
        {
            if (threadModifier != null) {
                threadModifier.Invoke(_t);
            }
            _state = ApplicationState.STARTED;
            _t.Start();
        }

    }
}
