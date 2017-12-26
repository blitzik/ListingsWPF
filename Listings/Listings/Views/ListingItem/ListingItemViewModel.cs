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


        private WorkedTimeViewModel _workedTimeViewModel;
        public WorkedTimeViewModel WorkedTimeViewModel
        {
            get { return _workedTimeViewModel; }
        }


        private ListingFacade _listingFacade;
        private DayItem _dayItem;


        public ListingItemViewModel(ListingFacade listingFacade, DayItem dayItem, string windowTitle)
        {
            _listingFacade = listingFacade;
            WindowTitle = windowTitle;

            _dayItem = dayItem;
            if (dayItem.ListingItem != null) {
                ListingItem l = dayItem.ListingItem;
                _locality = l.Locality;

                _workedTimeViewModel = new WorkedTimeViewModel(
                    l.ShiftStart,
                    l.ShiftEnd,
                    l.ShiftLunchStart,
                    l.ShiftLunchEnd,
                    l.OtherHours
                );

            } else {
                _workedTimeViewModel = new WorkedTimeViewModel();
            }
        }


        public delegate void ListingItemSavedHandler(object sender, ListingItemArgs args);
        public event ListingItemSavedHandler OnListingItemSaved;
        private void SaveListingItem()
        {
            ListingItem newItem = _dayItem.Listing.ReplaceItem(
                _dayItem.Day,
                _locality,
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
