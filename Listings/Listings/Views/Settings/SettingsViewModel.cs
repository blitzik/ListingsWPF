using Listings.Commands;
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
        private Time _start;
        private Time _end;
        private Time _lucnchStart;
        private Time _lunchEnd;
        private Time _otherHours;


        private WorkedTimeSettingViewModel _workedTimeViewModel;
        public WorkedTimeSettingViewModel WorkedTimeViewModel
        {
            get { return _workedTimeViewModel; }
        }


        private DelegateCommand<object> _saveSettingsCommand;
        public DelegateCommand<object> SaveSettingsCommand
        {
            get
            {
                if (_saveSettingsCommand == null) {
                    _saveSettingsCommand = new DelegateCommand<object>(p => SaveSettings());
                }
                return _saveSettingsCommand;
            }
        }


        private ListingFacade _listingFacade;
        private SettingFacade _settingFacade;


        public SettingsViewModel(ListingFacade listingFacade, SettingFacade settingFacade, string windowTitle)
        {
            _listingFacade = listingFacade;
            _settingFacade = settingFacade;
            WindowTitle = windowTitle;

            _workedTimeViewModel = new WorkedTimeSettingViewModel();
        }


        public delegate void SaveSettingsHandler (object sender, EventArgs args);
        public event SaveSettingsHandler OnSavedSettings;
        private void SaveSettings()
        {


            SaveSettingsHandler handler = OnSavedSettings;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }


        public delegate void CancelChangesHandler(object sender, EventArgs args);
        public event CancelChangesHandler OnCanceledChanges;
        private void CancelChanges()
        {


            CancelChangesHandler handler = OnCanceledChanges;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

    }
}
