using Caliburn.Micro;
using Listings.Messages;
using System;
using System.Collections.Generic;

namespace Listings.Views
{
    public class MainWindowViewModel : Conductor<IViewModel>.Collection.OneActive, IHandle<DisplayViewMessage>, IHandle<ChangeViewMessage>
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            private set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }


        private Dictionary<string, IViewModel> _viewModels;


        private ListingsOverviewViewModel _listingsOverviewViewModel;
        private ListingViewModel _listingViewModel;
        private EmployersViewModel _employersViewModel;
        private SettingsViewModel _settingsViewModel;


        public MainWindowViewModel(
            IEventAggregator eventAggregator,
            ListingsOverviewViewModelFactory listingsOverviewViewModelFactory,
            ListingViewModelFactory listingViewModelFactory,
            EmployersViewModelFactory employersViewModelFactory,
            SettingsViewModelFactory settingsViewModelFactory
        ) {
            _viewModels = new Dictionary<string, IViewModel>();

            eventAggregator.Subscribe(this);

            _listingsOverviewViewModel = listingsOverviewViewModelFactory.Create("Přehled výčetek");
            _listingViewModel = listingViewModelFactory.Create("Nová výčetka");
            _employersViewModel = employersViewModelFactory.Create("Správa zaměstnavatelů");
            _settingsViewModel = settingsViewModelFactory.Create("Nastavení");

            DisplayListingsOverview();
        }


        public void DisplayListingsOverview()
        {
            ActivateItem(_listingsOverviewViewModel);
        }


        public void DisplayListingCreation()
        {
            ActivateItem(_listingViewModel);
        }


        public void DisplayEmployersList()
        {
            ActivateItem(_employersViewModel);
        }


        public void DisplaySettings()
        {
            ActivateItem(_settingsViewModel);
        }


        public override void ActivateItem(IViewModel item)
        {
            string typeName = item.GetType().Name;
            if (!_viewModels.ContainsKey(typeName)) {
                _viewModels.Add(typeName, item);
            }

            Title = item.WindowTitle;
            base.ActivateItem(item);
        }


        // -----


        public void Handle(DisplayViewMessage message)
        {
            ActivateItem(message.ViewModel);
        }
        

        public void Handle(ChangeViewMessage message)
        {
            if (!_viewModels.ContainsKey(message.ViewModelName)) {
                throw new Exception("ViewModel with given name does NOT exist. You have to display it by DisplayViewMessage first.");
            }
            ActivateItem(_viewModels[message.ViewModelName]);
        }

    }
}
