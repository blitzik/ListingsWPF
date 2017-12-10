using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Exceptions;
using Listings.Facades;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Listings.Views
{
    public class ListingDetailViewModel : ViewModel
    {
        private readonly ListingFacade _listingFacade;

        private string _basicWindowTitle;
        
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                _listing = value;
                InitializeData();
                WindowTitle = string.Format("{0} [{1} {2} {3}]", _basicWindowTitle, Date.Months[12 - value.Month], value.Year, string.Format("- {0}", value.Name));
                RaisePropertyChanged();
            }
        }


        private Week _selectedWeek;
        public Week SelectedWeek
        {
            get { return _selectedWeek; }
            set {
                _selectedWeek = value;
                if (value == null) {
                    DisplayableItems = new ObservableCollection<DayItem>(_dayItems);
                } else {
                    Week w = _weeksInMonth.Find(we => we.WeekNumber == SelectedWeek.WeekNumber);
                    DisplayableItems = new ObservableCollection<DayItem>(w.DayItems);
                }
                RaisePropertyChanged();
            }
        }


        private List<Week> _weeksInMonth;
        public List<Week> WeeksInMonth
        {
            get { return _weeksInMonth; }
        }


        private List<DayItem> _dayItems;


        private ObservableCollection<DayItem> _displayableItems;
        public ObservableCollection<DayItem> DisplayableItems
        {
            get { return _displayableItems; }
            set
            {
                _displayableItems = value;
                RaisePropertyChanged();
            }
        }


        private DelegateCommand _openListingItemDetailCommand;
        public DelegateCommand OpenListingItemDetailCommand
        {
            get
            {
                if (_openListingItemDetailCommand == null) {
                    _openListingItemDetailCommand = new DelegateCommand(p => OpenListingItemDetail((int)p));
                }

                return _openListingItemDetailCommand;
            }
        }


        private DelegateCommand _removeItemCommand;
        public DelegateCommand RemoveItemCommand
        {
            get
            {
                if (_removeItemCommand == null) {
                    _removeItemCommand = new DelegateCommand(p => RemoveItemByDay((int)p));
                }

                return _removeItemCommand;
            }
        }


        private DelegateCommand _copyItemDownCommand;
        public DelegateCommand CopyItemDownCommand
        {
            get
            {
                if (_copyItemDownCommand == null) {
                    _copyItemDownCommand = new DelegateCommand(p => CopyItemDown((int)p));
                }

                return _copyItemDownCommand;
            }
        }


        public ListingDetailViewModel(ListingFacade listingFacade, string windowTitle)
        {
            _listingFacade = listingFacade;
            WindowTitle = windowTitle;
            _basicWindowTitle = windowTitle;

            _dayItems = new List<DayItem>();
            DisplayableItems = new ObservableCollection<DayItem>();
        }


        public void ReplaceDayInListBy(ListingItem item)
        {
            _dayItems[item.Day - 1].Update(item);
        }


        public delegate void OpenListingItemDetailHandler(object sender, SelectedDayItemArgs args);
        public event OpenListingItemDetailHandler OnListingItemClicked;
        private void OpenListingItemDetail(int day)
        {
            OpenListingItemDetailHandler handler = OnListingItemClicked;
            if (handler != null) {
                handler(this, new SelectedDayItemArgs(_dayItems[day - 1]));
            }
        }


        private void RemoveItemByDay(int day)
        {
            Listing.RemoveItemByDay(day);
            _dayItems[day - 1].Reset();

            _listingFacade.Save(Listing);
        }


        private void CopyItemDown(int day)
        {
            DayItem dayItem = _dayItems[day - 1];
            ListingItem newItem = Listing.ReplaceItem(
                day + 1,
                dayItem.Locality,
                dayItem.ShiftStart,
                dayItem.ShiftEnd,
                dayItem.ShiftLunchStart,
                dayItem.ShiftLunchEnd
                );
            
            _dayItems[day].Update(newItem);

            _listingFacade.Save(Listing);
        }


        private void InitializeData()
        {
            if (Listing == null) {
                throw new InvalidStateException("Listing cannot be null!");
            }
            _listingFacade.Activate(_listing, 4);

            SelectedWeek = null;

            DateTime now = DateTime.Now;
            int currentWeekNumber = Date.GetWeekNumber(now.Year, now.Month, now.Day);

            _weeksInMonth = new List<Week>();
            _dayItems = new List<DayItem>();
            for (int day = 0; day < DateTime.DaysInMonth(Listing.Year, Listing.Month); day++) {
                ListingItem listingItem = Listing.GetItemByDay(day + 1);
                DayItem dayItem;
                if (listingItem == null) {
                    dayItem = new DayItem(Listing, day + 1);
                } else {
                    dayItem = new DayItem(listingItem);
                }

                _dayItems.Add(dayItem);

                Week week = _weeksInMonth.Find(w => w.WeekNumber == dayItem.Week);
                if (week == null) {
                    week = new Week(dayItem.Week, currentWeekNumber == dayItem.Week);
                    _weeksInMonth.Add(week);

                    if (currentWeekNumber == dayItem.Week) {
                        SelectedWeek = week;
                    }
                }
                week.AddDayItem(dayItem);
            }

            if (SelectedWeek == null) {
                DisplayableItems = new ObservableCollection<DayItem>(_dayItems);
            } else {
                Week w = _weeksInMonth.Find(we => we.WeekNumber == SelectedWeek.WeekNumber);
                DisplayableItems = new ObservableCollection<DayItem>(w.DayItems);
            }
        }

    }
}
