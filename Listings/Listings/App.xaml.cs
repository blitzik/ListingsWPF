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

namespace Listings
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IObjectContainer _db;


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnActivated(e);

            Db4oObjectContainerFactory dbFactory = new Db4oObjectContainerFactory();
            _db = dbFactory.Create("data.dbs");

            ListingFacade listingFacade = new ListingFacade(_db);
            EmployerFacade employersFacade = new EmployerFacade(_db);
            SettingFacade settingFacade = new SettingFacade(_db);

            MainViewModel mainViewModel = new MainViewModel(listingFacade, employersFacade, settingFacade);

            MainWindow mw = new MainWindow
            {
                DataContext = mainViewModel
            };

            mw.Show();
        }


        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _db.Close();
        }
    }
}
