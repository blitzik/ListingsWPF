using Caliburn.Micro;
using Listings.Facades;
using Listings.Services;
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
            ViewModelLocator.ConfigureTypeMappings(new TypeMappingConfiguration() {
                DefaultSubNamespaceForViewModels = "Listings.Views",
                DefaultSubNamespaceForViews = "Listings.Views"
            });

            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            // Services
            _container.Singleton<Db4oObjectContainerFactory>();
            _container.Singleton<ObjectContainerRegistry>();

            // facades
            _container.Singleton<ListingFacade>();
            _container.Singleton<SettingFacade>();
            _container.Singleton<EmployerFacade>();

            // MainWindow
            _container.Singleton<MainWindowViewModel>();

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
