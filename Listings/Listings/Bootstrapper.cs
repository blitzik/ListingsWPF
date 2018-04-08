using Caliburn.Micro;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Listings.Domain;
using Listings.Facades;
using Listings.Services;
using Listings.Services.Backup;
using Listings.Services.IO;
using Listings.Services.Pdf;
using Listings.Services.ViewModelResolver;
using Listings.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Listings
{
    public class Bootstrapper : BootstrapperBase
    {
        static Mutex mutex = new Mutex(false, "34515d3d-cdda-4d87-aa0c-eeaab04ba20a");

        private SimpleContainer _container;


        public Bootstrapper()
        {
            Initialize();
        }


        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            // Services
            _container.Singleton<Db4oObjectContainerFactory>();
            _container.Singleton<ObjectContainerRegistry>();
            _container.Singleton<IViewModelResolver<IViewModel>, ViewModelResolver>();
            _container.Singleton<IOpeningFilePathSelector, OpenFilePathSelector>();
            _container.Singleton<ISavingFilePathSelector, SaveFilePathSelector>();
            _container.Singleton<IListingSectionFactory, ListingSectionFactory>();
            _container.Singleton<IListingPdfDocumentFactory, DefaultListingPdfReportFactory>();
            _container.Singleton<IMultipleListingReportFactory, MultipleListingReportFactory>();
            _container.Singleton<IListingReportGenerator, ListingReportGenerator>();
            _container.Singleton<IBackupImport, BackupImport>();

            // facades
            _container.Singleton<ListingFacade>();
            _container.Singleton<SettingFacade>();
            _container.Singleton<EmployerFacade>();

            // Windows
            _container.Singleton<MainWindowViewModel>();
            _container.PerRequest<StartupErrorWindowViewModel>();

            // ViewModels
            _container.Singleton<ListingsOverviewViewModel>(nameof(ListingsOverviewViewModel));
            _container.Singleton<EmployersViewModel>(nameof(EmployersViewModel));
            _container.Singleton<ListingViewModel>(nameof(ListingViewModel));
            _container.Singleton<ListingDeletionViewModel>(nameof(ListingDeletionViewModel));
            _container.Singleton<ListingDetailViewModel>(nameof(ListingDetailViewModel));
            _container.Singleton<ListingEditingViewModel>(nameof(ListingEditingViewModel));
            _container.Singleton<ListingItemViewModel>(nameof(ListingItemViewModel));
            _container.Singleton<ListingPdfGenerationViewModel>(nameof(ListingPdfGenerationViewModel));
            _container.Singleton<SettingsViewModel>(nameof(SettingsViewModel));
            _container.Singleton<EmptyListingsGenerationViewModel>(nameof(EmptyListingsGenerationViewModel));


            _container.Instance(_container);
        }


        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false) || AppDomain.CurrentDomain.IsDefaultAppDomain() == true) {
                System.Windows.Application.Current.Shutdown();
            }

            ObjectContainerRegistry ocr = _container.GetInstance<ObjectContainerRegistry>();

            ResultObject ro = new ResultObject(true);
            try {
                IObjectContainer db = _container.GetInstance<Db4oObjectContainerFactory>().Create(Db4oObjectContainerFactory.MAIN_DATABASE_NAME);

                ocr.Add(Db4oObjectContainerFactory.MAIN_DATABASE_NAME, db);
                var vm = _container.GetInstance<MainWindowViewModel>();
                _container.BuildUp(vm);
                _container.GetInstance<IWindowManager>().ShowWindow(vm);

            } catch (DatabaseFileLockedException ex) {
                ro = new ResultObject(false);
                ro.AddMessage("Nelze načíst Vaše data. Soubor je využíván jiným procesem.");

            } catch (Exception ex) {
                ro = new ResultObject(false);
                ro.AddMessage("Při spouštění aplikace došlo k neočekávané chybě");
            }

            if (!ro.Success) {
                StartupErrorWindowViewModel errw = _container.GetInstance<StartupErrorWindowViewModel>();
                errw.Text = ro.GetLastMessage();
                _container.GetInstance<IWindowManager>().ShowDialog(errw);
            }
        }


        protected override void OnExit(object sender, EventArgs e)
        {
            ObjectContainerRegistry ocr = _container.GetInstance<ObjectContainerRegistry>();
            ocr.CloseAll();

            mutex.ReleaseMutex();
        }


        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }


        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }


        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

    }
}
