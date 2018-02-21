using Caliburn.Micro;
using Listings.Facades;
using Listings.Services;
using Listings.Services.IO;
using Listings.Services.Pdf;
using Listings.Services.ViewModelResolver;
using Listings.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Listings
{
    public class Bootstrapper : BootstrapperBase
    {
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
            _container.Singleton<IListingPdfDocumentFactory, DefaultListingPdfReportFactory>();

            // facades
            _container.Singleton<ListingFacade>();
            _container.Singleton<SettingFacade>();
            _container.Singleton<EmployerFacade>();

            // Windows
            _container.Singleton<MainWindowViewModel>();
            _container.Singleton<ProgressBarWindowViewModel>();

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

            // ViewModel's factories
            /*_container.Singleton<EmployersViewModelFactory>();
            _container.Singleton<ListingViewModelFactory>();
            _container.Singleton<ListingDeletionViewModelFactory>();
            _container.Singleton<ListingDetailViewModelFactory>();
            _container.Singleton<ListingEditingViewModelFactory>();
            _container.Singleton<ListingItemViewModelFactory>();
            _container.Singleton<ListingPdfGenerationViewModelFactory>();
            _container.Singleton<ListingsOverviewViewModelFactory>();
            _container.Singleton<SettingsViewModelFactory>();*/

            _container.Instance(_container);
        }


        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            ObjectContainerRegistry ocr = _container.GetInstance<ObjectContainerRegistry>();
            ocr.Add(Db4oObjectContainerFactory.MAIN_DATABASE_NAME, _container.GetInstance<Db4oObjectContainerFactory>().Create(Db4oObjectContainerFactory.MAIN_DATABASE_NAME));

            _container.GetInstance<IWindowManager>().ShowWindow(_container.GetInstance<MainWindowViewModel>());
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
