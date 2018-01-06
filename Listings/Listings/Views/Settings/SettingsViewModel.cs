using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Facades;
using Listings.Services;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class SettingsViewModel : ViewModel
    {
        private WorkedTimeSettingViewModel _workedTimeViewModel;
        public WorkedTimeSettingViewModel WorkedTimeViewModel
        {
            get { return _workedTimeViewModel; }
        }


        private string _ownerName;
        public string OwnerName
        {
            get { return _ownerName; }
            set {
                _ownerName = value;
                RaisePropertyChanged();
                CancelChangesCommand.RaiseCanExecuteChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }


        private DelegateCommand<object> _saveSettingsCommand;
        public DelegateCommand<object> SaveSettingsCommand
        {
            get
            {
                if (_saveSettingsCommand == null) {
                    _saveSettingsCommand = new DelegateCommand<object>(
                        p => SaveSettings(),
                        p => HasSettingsChanged()
                    );
                }
                return _saveSettingsCommand;
            }
        }


        private DelegateCommand<object> _cancelChangesCommand;
        public DelegateCommand<object> CancelChangesCommand
        {
            get
            {
                if (_cancelChangesCommand == null) {
                    _cancelChangesCommand = new DelegateCommand<object>(
                        p => CancelChanges(),
                        p => HasSettingsChanged()
                    );
                }
                return _cancelChangesCommand;
            }
        }


        private DefaultSettings _defaultSetting;

        private ListingFacade _listingFacade;
        private SettingFacade _settingFacade;

        
        public SettingsViewModel(ListingFacade listingFacade, SettingFacade settingFacade, string windowTitle)
        {
            _listingFacade = listingFacade;
            _settingFacade = settingFacade;
            WindowTitle = windowTitle;

            _defaultSetting = _settingFacade.GetDefaultSettings();

            OwnerName = _defaultSetting.OwnerName;
            _workedTimeViewModel = new WorkedTimeSettingViewModel(_defaultSetting.Time, _defaultSetting.Time);
            _workedTimeViewModel.OnTimeChanged += (object sender, WorkedTimeEventArgs args) => {
                CancelChangesCommand.RaiseCanExecuteChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            };
        }


        private bool HasSettingsChanged()
        {
            if (_defaultSetting.Time.Start != _workedTimeViewModel.StartTime ||
                _defaultSetting.Time.End != _workedTimeViewModel.EndTime ||
                _defaultSetting.Time.LunchStart != _workedTimeViewModel.LunchStart ||
                _defaultSetting.Time.LunchEnd != _workedTimeViewModel.LunchEnd ||
                _defaultSetting.Time.OtherHours != _workedTimeViewModel.OtherHours ) {
                return true;
            }

            if (!string.Equals(_defaultSetting.OwnerName, string.IsNullOrEmpty(OwnerName) ? null : OwnerName)) {
                return true;
            }

            return false;

        }


        public delegate void SaveSettingsHandler (object sender, EventArgs args);
        public event SaveSettingsHandler OnSavedSettings;
        private void SaveSettings()
        {
            _defaultSetting.Time = new TimeSetting(
                new Time(_workedTimeViewModel.StartTime),
                new Time(_workedTimeViewModel.EndTime),
                new Time(_workedTimeViewModel.LunchStart),
                new Time(_workedTimeViewModel.LunchEnd),
                new Time(_workedTimeViewModel.OtherHours)
            );
            _defaultSetting.OwnerName = string.IsNullOrEmpty(OwnerName) ? null : OwnerName;

            _settingFacade.SaveDefaultSetting(_defaultSetting);

            CancelChangesCommand.RaiseCanExecuteChanged();
            SaveSettingsCommand.RaiseCanExecuteChanged();

            SaveSettingsHandler handler = OnSavedSettings;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }


        public delegate void CancelChangesHandler(object sender, EventArgs args);
        public event CancelChangesHandler OnCanceledChanges;
        private void CancelChanges()
        {
            WorkedTimeViewModel.SetTime(_defaultSetting.Time);
            OwnerName = _defaultSetting.OwnerName;

            CancelChangesHandler handler = OnCanceledChanges;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
