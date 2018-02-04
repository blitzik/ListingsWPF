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
                Reset();
                InitializeData();
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


        private DelegateCommand<object> _listingDeletionCommand;
        public DelegateCommand<object> ListingDeletionCommand
        {
            get
            {
                if (_listingDeletionCommand == null) {
                    _listingDeletionCommand = new DelegateCommand<object>(p => DeleteListing());
                }
                return _listingDeletionCommand;
            }
        }


        private DelegateCommand<int> _openListingItemDetailCommand;
        public DelegateCommand<int> OpenListingItemDetailCommand
        {
            get
            {
                if (_openListingItemDetailCommand == null) {
                    _openListingItemDetailCommand = new DelegateCommand<int>(p => OpenListingItemDetail(p));
                }
                return _openListingItemDetailCommand;
            }
        }


        private DelegateCommand<object> _generatePdfCommand;
        public DelegateCommand<object> GeneratePdfCommand
        {
            get
            {
                if (_generatePdfCommand == null) {
                    _generatePdfCommand = new DelegateCommand<object>(p => DisplayPdfGenerationPage());
                }
                return _generatePdfCommand;
            }
        }


        private DelegateCommand<object> _listingEditCommand;
        public DelegateCommand<object> ListingEditCommand
        {
            get
            {
                if (_listingEditCommand == null) {
                    _listingEditCommand = new DelegateCommand<object>(p => OpenEditing());
                }
                return _listingEditCommand;
            }
        }


        private DelegateCommand<int> _removeItemCommand;
        public DelegateCommand<int> RemoveItemCommand
        {
            get
            {
                if (_removeItemCommand == null) {
                    _removeItemCommand = new DelegateCommand<int>(p => RemoveItemByDay(p));
                }
                return _removeItemCommand;
            }
        }


        private DelegateCommand<int> _copyItemDownCommand;
        public DelegateCommand<int> CopyItemDownCommand
        {
            get
            {
                if (_copyItemDownCommand == null) {
                    _copyItemDownCommand = new DelegateCommand<int>(p => CopyItemDown(p));
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


        public delegate void OpenListingEditingHandler(object sender, ListingArgs args);
        public event OpenListingEditingHandler OnListingEditingClicked;
        private void OpenEditing()
        {
            OpenListingEditingHandler handler = OnListingEditingClicked;
            if (handler != null) {
                handler(this, new ListingArgs(Listing));
            }
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


        public delegate void RemoveListingHandler(object sender, ListingArgs args);
        public event RemoveListingHandler OnListingDeletionClicked;
        private void DeleteListing()
        {
            RemoveListingHandler handler = OnListingDeletionClicked;
            if (handler != null) {
                handler(this, new ListingArgs(Listing));
            }
        }


        public delegate void DisplayListingPdfGenerationHandler(object sender, ListingArgs args);
        public event DisplayListingPdfGenerationHandler OnListingPdfGenerationClicked;
        private void DisplayPdfGenerationPage()
        {
            DisplayListingPdfGenerationHandler handler = OnListingPdfGenerationClicked;
            if (handler != null) {
                handler(this, new ListingArgs(Listing));
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
            ListingItem newItem = Listing.ReplaceItem(day + 1, dayItem.Locality, dayItem.ListingItem.TimeSetting);
            
            _dayItems[day].Update(newItem);

            _listingFacade.Save(Listing);
        }


        public override void Reset()
        {
            WindowTitle = string.Format("{0} [{1} {2} {3}]", _basicWindowTitle, Date.Months[12 - Listing.Month], Listing.Year, string.Format("- {0}", Listing.Name));
        }


        private void InitializeData()
        {
            if (Listing == null) {
                throw new InvalidStateException("Listing cannot be null!");
            }
            _listingFacade.Activate(_listing, 5);

            SelectedWeek = null;

            DateTime now = DateTime.Now;
            int currentWeekNumber = Date.GetWeekNumber(now.Year, now.Month, now.Day);

            _weeksInMonth = new List<Week>();
            _dayItems = new List<DayItem>();
            int daysInMonth = DateTime.DaysInMonth(Listing.Year, Listing.Month);
            DayItem dayItem;
            for (int day = 0; day < daysInMonth; day++) {
                dayItem = new DayItem(Listing, day + 1);

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
