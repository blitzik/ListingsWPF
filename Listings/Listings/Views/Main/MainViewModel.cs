using Db4objects.Db4o;
using Listings.Commands;
using Listings.EventArguments;
using Listings.Domain;
using Listings.Facades;
using Listings.Services;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Listings.Views
{
    public class MainViewModel : ViewModel
    {
        private ViewModel _currentViewModel;
        public ViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (_currentViewModel == value) return;

                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }


        private DelegateCommand<string> _navigationCommand;
        public DelegateCommand<string> NavigationCommand
        {
            get
            {
                if (_navigationCommand == null) {
                    _navigationCommand = new DelegateCommand<string>(p => ChangeView(p));
                }

                return _navigationCommand;
            }
        }


        private ListingsOverviewViewModel _listingsOverviewViewModel;
        private ListingsOverviewViewModel ListingsOverviewViewModel
        {
            get
            {
                if (_listingsOverviewViewModel == null) {
                    _listingsOverviewViewModel = new ListingsOverviewViewModel(_listingFacade, "Přehled výčetek");
                    _listingsOverviewViewModel.OnListingSelected += (object sender, ListingArgs args) => {
                        ListingDetailViewModel.Listing = args.Listing;
                        ChangeView(nameof(ListingDetailViewModel));
                    };
                }

                return _listingsOverviewViewModel;
            }
        }


        private ListingViewModel _listingViewModel;
        private ListingViewModel ListingViewModel
        {
            get
            {
                if (_listingViewModel == null) {
                    _listingViewModel = new ListingViewModel(_listingFacade, _employerFacade, "Nová výčetka");
                    _listingViewModel.OnListingCreation += (object sender, ListingArgs args) => {
                        ListingDetailViewModel.Listing = args.Listing;
                        ChangeView(nameof(ListingDetailViewModel));
                    };
                }

                return _listingViewModel;
            }
        }


        private ListingDetailViewModel _listingDetailViewModel;
        private ListingDetailViewModel ListingDetailViewModel
        {
            get
            {
                if (_listingDetailViewModel == null) {
                    _listingDetailViewModel = new ListingDetailViewModel(_listingFacade, "Detail výčetky");
                    _listingDetailViewModel.OnListingItemClicked += (object sender, SelectedDayItemArgs args) => {
                        _listingItemViewModel = new ListingItemViewModel(_listingFacade, args.DayItem, _defaultSettings, "Detail položky");
                        _listingItemViewModel.OnListingItemSaved += (object s, ListingItemArgs a) => {
                            _listingDetailViewModel.ReplaceDayInListBy(a.ListingItem);
                            ChangeView(nameof(ListingDetailViewModel));
                        };
                        _listingItemViewModel.OnReturnBackToListingDetail += (object s, EventArgs a) => {
                            ChangeView(nameof(ListingDetailViewModel));
                        };
                        ChangeView(nameof(ListingItemViewModel));
                    };

                    _listingDetailViewModel.OnListingPdfGenerationClicked += (object sender, ListingArgs args) => {
                        ListingPdfGenerationViewModel.Listing = args.Listing;
                        ChangeView(nameof(ListingPdfGenerationViewModel));
                    };

                    _listingDetailViewModel.OnListingEditingClicked += (object sender, ListingArgs args) => {
                        ListingEditingViewModel.Listing = args.Listing;
                        ChangeView(nameof(ListingEditingViewModel));
                    };

                    _listingDetailViewModel.OnListingDeletionClicked += (object sender, ListingArgs args) => {
                        ListingDeletionViewModel.Listing = args.Listing;
                        ChangeView(nameof(ListingDeletionViewModel));
                    };
                }

                return _listingDetailViewModel;
            }
        }


        private ListingEditingViewModel _listingEditingViewModel;
        public ListingEditingViewModel ListingEditingViewModel
        {
            get
            {
                if (_listingEditingViewModel == null) {
                    _listingEditingViewModel = new ListingEditingViewModel(_listingFacade, _employerFacade, "Úprava výčetky");
                    _listingEditingViewModel.OnListingSuccessfullySaved += (object sender, ListingArgs args) => {
                        ChangeView(nameof(ListingDetailViewModel));
                    };
                    _listingEditingViewModel.OnReturnBackClicked += (object sender, ListingArgs args) => {
                        ChangeView(nameof(ListingDetailViewModel));
                    };
                }
                return _listingEditingViewModel;
            }
        }


        private ListingDeletionViewModel _listingDeletionViewModel;
        public ListingDeletionViewModel ListingDeletionViewModel
        {
            get
            {
                if (_listingDeletionViewModel == null) {
                    _listingDeletionViewModel = new ListingDeletionViewModel(_listingFacade, "Odstranění výčetky");
                    _listingDeletionViewModel.OnDeletedListing += (object sender, EventArgs args) => {
                        ChangeView(nameof(ListingsOverviewViewModel));
                    };
                    _listingDeletionViewModel.OnReturnBackClicked += (object sender, EventArgs args) => {
                        ChangeView(nameof(ListingDetailViewModel));
                    };
                }

                return _listingDeletionViewModel;
            }
        }


        private ListingItemViewModel _listingItemViewModel;
        private ListingItemViewModel ListingItemViewModel
        {
            get
            {
                return _listingItemViewModel;
            }
        }


        private EmployersViewModel _employersViewModel;
        public EmployersViewModel EmployersViewModel
        {
            get
            {
                if (_employersViewModel == null) {
                    _employersViewModel = new EmployersViewModel(_employerFacade, "Správa zaměstnavatelů");
                }
                return _employersViewModel;
            }
        }


        private SettingsViewModel _settingsViewModel;
        public SettingsViewModel SettingsViewModel
        {
            get
            {
                if (_settingsViewModel == null) {
                    _settingsViewModel = new SettingsViewModel(_listingFacade, _settingFacade, "Nastavení");
                    /*_settingsViewModel.OnCanceledChanges += (object sender, EventArgs args) => {
                        ChangeView();
                    };*/
                }
                return _settingsViewModel;
            }
        }


        private ListingPdfGenerationViewModel _listingPdfGenerationViewModel;
        public ListingPdfGenerationViewModel ListingPdfGenerationViewModel
        {
            get
            {
                if (_listingPdfGenerationViewModel == null) {
                    _listingPdfGenerationViewModel = new ListingPdfGenerationViewModel(_settingFacade, "Generování PDF dokumentu");
                }
                return _listingPdfGenerationViewModel;
            }
        }
        

        private ListingFacade _listingFacade;
        private EmployerFacade _employerFacade;
        private SettingFacade _settingFacade;

        private DefaultSettings _defaultSettings;


        public MainViewModel(ListingFacade listingFacade, EmployerFacade employersFacade, SettingFacade settingFacade)
        {
            _listingFacade = listingFacade;
            _employerFacade = employersFacade;
            _settingFacade = settingFacade;

            _defaultSettings = _settingFacade.GetDefaultSettings();

            ChangeView(nameof(ListingsOverviewViewModel));
        }


        private void ChangeView(string viewCode)
        {
            // we dont want the same menu item to be clicked more than once
            if (CurrentViewModel != null && CurrentViewModel.GetType().Name == viewCode) {
                return;
            }

            switch (viewCode) {
                case nameof(ListingsOverviewViewModel):
                    ListingsOverviewViewModel.RefreshList();
                    CurrentViewModel = ListingsOverviewViewModel;
                    break;

                case nameof(ListingViewModel):
                    ListingViewModel.RefreshEmployers();
                    CurrentViewModel = ListingViewModel;
                    break;

                case nameof(ListingEditingViewModel):
                    CurrentViewModel = ListingEditingViewModel;
                    break;

                case nameof(ListingDetailViewModel):
                    ListingDetailViewModel.RefreshWindowTitle();
                    CurrentViewModel = ListingDetailViewModel;
                    break;

                case nameof(ListingItemViewModel):
                    CurrentViewModel = ListingItemViewModel;
                    break;

                case nameof(ListingDeletionViewModel):
                    CurrentViewModel = ListingDeletionViewModel;
                    break;

                case nameof(EmployersViewModel):
                    EmployersViewModel.RestoreDefaultStates();
                    CurrentViewModel = EmployersViewModel;
                    break;

                case nameof(SettingsViewModel):
                    CurrentViewModel = SettingsViewModel;
                    break;

                case nameof(ListingPdfGenerationViewModel):
                    CurrentViewModel = ListingPdfGenerationViewModel;
                    break;
            }

            WindowTitle = CurrentViewModel.WindowTitle;
        }
    }
}
