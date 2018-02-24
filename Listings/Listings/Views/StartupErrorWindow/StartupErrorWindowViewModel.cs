using Caliburn.Micro;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Listings.Commands;
using Listings.Domain;
using Listings.Facades;
using Listings.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class StartupErrorWindowViewModel : ScreenBaseViewModel
    {
        private DelegateCommand<object> _closeAppCommand;
        public DelegateCommand<object> CloseAppCommand
        {
            get
            {
                if (_closeAppCommand == null) {
                    _closeAppCommand = new DelegateCommand<object>(p => TryClose());
                }
                return _closeAppCommand;
            }
        }


        private DelegateCommand<object> _openAppAnywayCommand;
        public DelegateCommand<object> OpenAppAnywayCommand
        {
            get
            {
                if (_openAppAnywayCommand == null) {
                    _openAppAnywayCommand = new DelegateCommand<object>(p => CreateDefaultConnection());
                }
                return _openAppAnywayCommand;
            }
        }


        private readonly Db4oObjectContainerFactory _dbFactory;
        private readonly IWindowManager _windowManager;

        public StartupErrorWindowViewModel(
            IEventAggregator eventAggregator,
            Db4oObjectContainerFactory dbFactory,
            IWindowManager windowManager
        ) : base (eventAggregator) {
            _dbFactory = dbFactory;
            _windowManager = windowManager;
        }


        private void CreateDefaultConnection()
        {
            string directory = Db4oObjectContainerFactory.GetDatabaseDirectoryPath();

            string activeDbFilePath = Path.Combine(directory, (Db4oObjectContainerFactory.MAIN_DATABASE_NAME + "." + (Db4oObjectContainerFactory.DATABASE_EXTENSION)));

            DateTime now = DateTime.Now;
            string oldDbBackupFileName = string.Format("before_default_{0}_{1}_{2}_{3}_{4}_{5}.{6}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, Db4oObjectContainerFactory.DATABASE_EXTENSION);

            File.Move(activeDbFilePath, Path.Combine(directory, oldDbBackupFileName));

            // we do not need to create new database here, because new database will be created at the start of the application

            TryClose();
        }
    }
}
