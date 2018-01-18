using Listings.Services;
using Listings.Facades;
using Db4objects.Db4o;
using Listings.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Listings.Domain;
using Listings.Services.IO;

namespace Listings
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ObjectContainerRegistry _dbRegistry;


        public App()
        {
            _dbRegistry = new ObjectContainerRegistry();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnActivated(e);
            
            _dbRegistry.Add(Db4oObjectContainerFactory.MAIN_DATABASE_NAME, (new Db4oObjectContainerFactory()).Create(Db4oObjectContainerFactory.MAIN_DATABASE_NAME));

            OpenFileDialogPathSelector openFileDialogPathSelector = new OpenFileDialogPathSelector();
            SaveFileDialogPathSelector saveFileDialogPathSelector = new SaveFileDialogPathSelector();

            ListingFacade listingFacade = new ListingFacade(_dbRegistry);
            EmployerFacade employersFacade = new EmployerFacade(_dbRegistry);
            SettingFacade settingFacade = new SettingFacade(_dbRegistry);

            MainViewModel mainViewModel = new MainViewModel(
                listingFacade,
                employersFacade,
                settingFacade,
                openFileDialogPathSelector
            );

            MainWindow mw = new MainWindow
            {
                DataContext = mainViewModel
            };

            mw.Show();
        }


        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _dbRegistry.CloseAll();
        }
    }
}
