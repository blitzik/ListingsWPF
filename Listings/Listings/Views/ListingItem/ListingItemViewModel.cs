using Caliburn.Micro;
using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Exceptions;
using Listings.Facades;
using Listings.Services;
using Listings.Utils;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Listings.Messages;

namespace Listings.Views
{
    public class ListingItemViewModel : ScreenBaseViewModel
    {
        private string _header;
        public string Header
        {
            get { return _header; }
            private set
            {
                _header = value;
                NotifyOfPropertyChange(() => Header);
            }
        }


        private WorkedTimeSettingViewModel _workedTimeViewModel;
        public WorkedTimeSettingViewModel WorkedTimeViewModel
        {
            get { return _workedTimeViewModel; }
            private set
            {
                _workedTimeViewModel = value;
                NotifyOfPropertyChange(() => WorkedTimeViewModel);
            }
        }


        private string _locality;
        public string Locality
        {
            get { return _locality; }
            set
            {
                _locality = value;
                NotifyOfPropertyChange(() => Locality);
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


        private DayItem _dayItem;
        public DayItem DayItem
        {
            get { return _dayItem; }
            set
            {
                _dayItem = value;
                Reset(value);
            }
        }


        private ListingFacade _listingFacade;
        private SettingFacade _settingFacade;
        
        private DefaultSettings _defaultSettings;


        public ListingItemViewModel(IEventAggregator eventAggregator, string windowTitle, DayItem dayItem, ListingFacade listingFacade, SettingFacade settingFacade) : base(eventAggregator, windowTitle)
        {
            _listingFacade = listingFacade;
            _settingFacade = settingFacade;

            DayItem = dayItem;
        }


        private void Reset(DayItem dayItem)
        {
            string date = CultureInfo.CurrentCulture.TextInfo.ToTitleCase((DateTime.Now).ToLongDateString().ToLower());
            WindowTitle = String.Format("{0} - {1}", date, dayItem.Listing.Name);
            _defaultSettings = _settingFacade.GetDefaultSettings();

            if (dayItem.ListingItem != null) {
                ListingItem l = dayItem.ListingItem;
                Locality = l.Locality;

                WorkedTimeViewModel = new WorkedTimeSettingViewModel(_eventAggregator, _defaultSettings.Time, l.TimeSetting, _defaultSettings.TimeTickInMinutes);

            } else {
                WorkedTimeViewModel = new WorkedTimeSettingViewModel(_eventAggregator, _defaultSettings.Time, _defaultSettings.Time, _defaultSettings.TimeTickInMinutes);
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


        private void ReturnBackToListingDetail()
        {
            _eventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(ListingDetailViewModel)));
        }
        
    }
}
