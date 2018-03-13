﻿using Caliburn.Micro;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Linq;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Listings
{
    public class Bootstrapper : BootstrapperBase
    {
        public static readonly string APP_VERSION = "1.0.1";
        public static readonly List<string> SUPPORTED_DBS = new List<string>() {
            "1.0.0",
            APP_VERSION
        };

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
            _container.Singleton<StartupErrorWindowViewModel>();

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


            // ViewModel's factories
            _container.Singleton<EmployersViewModelFactory>();
            _container.Singleton<ListingViewModelFactory>();
            _container.Singleton<ListingDeletionViewModelFactory>();
            _container.Singleton<ListingDetailViewModelFactory>();
            _container.Singleton<ListingEditingViewModelFactory>();
            _container.Singleton<ListingItemViewModelFactory>();
            _container.Singleton<ListingPdfGenerationViewModelFactory>();
            _container.Singleton<ListingsOverviewViewModelFactory>();
            _container.Singleton<SettingsViewModelFactory>();

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
                do {
                    IEnumerable<DbVersion> x = from DbVersion v in db where v.ID == DbVersion.UNIQUE_KEY select v;
                    DbVersion version = x.FirstOrDefault();
                    if (version == null || !SUPPORTED_DBS.Contains(version.Version)) {
                        ro = new ResultObject(false);
                        db.Close();
                        break;
                    }

                    ocr.Add(Db4oObjectContainerFactory.MAIN_DATABASE_NAME, db);
                    _container.GetInstance<IWindowManager>().ShowWindow(_container.GetInstance<MainWindowViewModel>());

                } while (false);


            } catch (DatabaseFileLockedException ex) { // todo
                System.Windows.Application.Current.Shutdown();

            } catch (Exception ex) {
                ro = new ResultObject(false);
            }

            if (!ro.Success) {
                _container.GetInstance<IWindowManager>().ShowDialog(_container.GetInstance<StartupErrorWindowViewModel>());
            }
        }


        protected override void OnExit(object sender, EventArgs e)
        {
            ObjectContainerRegistry ocr = _container.GetInstance<ObjectContainerRegistry>();
            ocr.CloseAll();
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
