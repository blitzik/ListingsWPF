using Listings.Views.Main;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnActivated(e);

            MainWindow mw = new MainWindow();
            MainViewModel mainViewModel = new MainViewModel();

            mw.DataContext = mainViewModel;

            mw.Show();
        }
    }
}
