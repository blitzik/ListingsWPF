using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Exceptions;
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
    public class ListingItemViewModel : ViewModel
    {
        private WorkedTimeSettingViewModel _workedTimeViewModel;
        public WorkedTimeSettingViewModel WorkedTimeViewModel
        {
            get { return _workedTimeViewModel; }
        }


        private string _locality;
        public string Locality
        {
            get { return _locality; }
            set
            {
                _locality = value;
                RaisePropertyChanged();
            }
        }


        private DelegateCommand<object> _returnBackToListingDetailCommand;
        public DelegateCommand<object> ReturnBackToListingDetailCommand
        {
            get
            {
                if (_returnBackToListingDetailCommand == null) {
                    _returnBackToListingDetailCommand = new DelegateCommand<object>(p => ReturnBackToListingDetail());
                }
                return _returnBackToListingDetailCommand;
            }
        }


        private DelegateCommand<object> _saveListingItemCommand;
        public DelegateCommand<object> SaveListingItemCommand
        {
            get
            {
                if (_saveListingItemCommand == null) {
                    _saveListingItemCommand = new DelegateCommand<object>(p => SaveListingItem());
                }
                return _saveListingItemCommand;
            }
        }


        private ListingFacade _listingFacade;
        private SettingFacade _settingFacade;
        
        private DayItem _dayItem;
        private DefaultSettings _defaultSettings;


        public ListingItemViewModel(string windowTitle, DayItem dayItem, ListingFacade listingFacade, SettingFacade settingFacade) : base(windowTitle)
        {
            _listingFacade = listingFacade;
            _settingFacade = settingFacade;
            _defaultSettings = settingFacade.GetDefaultSettings();

            _dayItem = dayItem;
            if (dayItem.ListingItem != null) {
                ListingItem l = dayItem.ListingItem;
                _locality = l.Locality;

                _workedTimeViewModel = new WorkedTimeSettingViewModel(_defaultSettings.Time, l.TimeSetting, _defaultSettings.TimeTickInMinutes);

            } else {
                _workedTimeViewModel = new WorkedTimeSettingViewModel(_defaultSettings.Time, _defaultSettings.Time, _defaultSettings.TimeTickInMinutes);
            }
        }


        public delegate void ListingItemSavedHandler(object sender, ListingItemArgs args);
        public event ListingItemSavedHandler OnListingItemSaved;
        private void SaveListingItem()
        {
            ListingItem newItem = _dayItem.Listing.ReplaceItem(
                _dayItem.Day,
                string.IsNullOrEmpty(_locality) ? null : _locality.Trim(),
                new Time(WorkedTimeViewModel.StartTime),
                new Time(WorkedTimeViewModel.EndTime),
                new Time(WorkedTimeViewModel.LunchStart),
                new Time(WorkedTimeViewModel.LunchEnd),
                new Time(WorkedTimeViewModel.OtherHours)
            );

            _listingFacade.Save(_dayItem.Listing);

            ListingItemSavedHandler handler = OnListingItemSaved;
            if (handler != null) {
                handler(this, new ListingItemArgs(newItem));
            }
        }



        public delegate void ReturnBackToListingDetailHandler(object sender, EventArgs args);
        public event ReturnBackToListingDetailHandler OnReturnBackToListingDetail;
        private void ReturnBackToListingDetail()
        {
            ReturnBackToListingDetailHandler handler = OnReturnBackToListingDetail;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
        
    }
}
