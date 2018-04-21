using Caliburn.Micro;
using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Exceptions;
using Listings.Facades;
using Listings.Messages;
using Listings.Services.ViewModelResolver;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Listings.Views
{
    public class ListingDetailViewModel : BaseScreen, IHandle<ListingMessage>
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                _listing = value;
                NotifyOfPropertyChange(() => Listing);
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
                    DisplayableItems = new ObservableCollection<DayItem>(value.DayItems);
                }
                NotifyOfPropertyChange(() => SelectedWeek);
                ExpandItemsCommand.RaiseCanExecuteChanged();
            }
        }


        private List<Week> _weeksInMonth;
        public List<Week> WeeksInMonth
        {
            get { return _weeksInMonth; }
        }


        private ObservableCollection<DayItem> _displayableItems;
        public ObservableCollection<DayItem> DisplayableItems
        {
            get { return _displayableItems; }
            set
            {
                _displayableItems = value;
                NotifyOfPropertyChange(() => DisplayableItems);
            }
        }


        private DelegateCommand<object> _listingDeletionCommand;
        public DelegateCommand<object> ListingDeletionCommand
        {
            get
            {
                if (_listingDeletionCommand == null) {
                    _listingDeletionCommand = new DelegateCommand<object>(p => DisplayListingDeletion());
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


        private DelegateCommand<object> _expandItemsCommand;
        public DelegateCommand<object> ExpandItemsCommand
        {
            get
            {
                if (_expandItemsCommand == null) {
                    _expandItemsCommand = new DelegateCommand<object>(
                        p => ExpandItems(),
                        p => SelectedWeek != null
                    );
                }
                return _expandItemsCommand;
            }
        }


        private List<DayItem> _dayItems;

        private readonly ListingFacade _listingFacade;


        public ListingDetailViewModel(
            ListingFacade listingFacade
        ) {
            _dayItems = new List<DayItem>();
            _listingFacade = listingFacade;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            // instantiation of viewmodels that will get Listing as a message from EventAggregator
            // These viewmodels need to be created before ListingMessage is sent, otherwise IHandle handler is  NOT called
            // because the viewmodel does not exist yet or if it exists, the IHandle handler is called
            // before OnActivation lifecycle method (in which the viewmodels' reset mechanism is called)
            
            ViewModelResolver.Resolve(nameof(ListingEditingViewModel));
            ViewModelResolver.Resolve(nameof(ListingItemViewModel));
            ViewModelResolver.Resolve(nameof(ListingDeletionViewModel));
            ViewModelResolver.Resolve(nameof(ListingPdfGenerationViewModel));
        }


        private void OpenEditing()
        {
            EventAggregator.PublishOnUIThread(new ListingMessage(Listing));
            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(ListingEditingViewModel)));
        }


        private void OpenListingItemDetail(int day)
        {
            EventAggregator.PublishOnUIThread(new EditDayItemMessage(_dayItems[day - 1]));
            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(ListingItemViewModel)));
        }


        private void DisplayListingDeletion()
        {
            EventAggregator.PublishOnUIThread(new ListingMessage(Listing));
            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(ListingDeletionViewModel)));
        }


        private void DisplayPdfGenerationPage()
        {
            EventAggregator.PublishOnUIThread(new ListingMessage(Listing));
            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(ListingPdfGenerationViewModel)));
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
            if (_dayItems[day].IsEqual(dayItem)) {
                return;
            }
            ListingItem newItem = Listing.ReplaceItem(day + 1, dayItem.Locality, dayItem.ListingItem.TimeSetting);

            _dayItems[day].Update(newItem);

            _listingFacade.Save(Listing);
        }


        private void ExpandItems()
        {
            SelectedWeek = null;
        }


        private void Reset(Listing listing)
        {
            WindowTitle.Text = GenerateWindowTitle(listing);
            NotifyOfPropertyChange(() => WindowTitle);

            _dayItems = PrepareDayItems(listing);
            _weeksInMonth = new List<Week>(PrepareWeeks(_dayItems).Values);
            NotifyOfPropertyChange(() => WeeksInMonth);

            DateTime now = DateTime.Now;
            int currentWeekNumber = Date.GetWeekNumber(now.Year, now.Month, now.Day);
            SelectedWeek = _weeksInMonth.Find(w => w.WeekNumber == currentWeekNumber);
        }


        private string GenerateWindowTitle(Listing listing)
        {
            return string.Format("{0} {1} {2}", Date.Months[12 - listing.Month], listing.Year, string.Format("- {0}", listing.Name));
        }


        private List<DayItem> PrepareDayItems(Listing listing)
        {
            _listingFacade.Activate(listing, 5);

            List<DayItem> dayItems = new List<DayItem>();

            DateTime now = DateTime.Now;
            DayItem dayItem;
            for (int day = 0; day < listing.DaysInMonth; day++) {
                dayItem = new DayItem(listing, day + 1);
                dayItems.Add(dayItem);
            }

            return dayItems;
        }


        private Dictionary<int, Week> PrepareWeeks(List<DayItem> items)
        {
            DateTime now = DateTime.Now;
            int currentWeekNumber = Date.GetWeekNumber(now.Year, now.Month, now.Day);

            Dictionary<int, Week> weeks = new Dictionary<int, Week>();
            foreach (DayItem dayItem in items) {
                if (!weeks.ContainsKey(dayItem.Week)) {
                    weeks.Add(dayItem.Week, new Week(dayItem.Week, dayItem.Week == currentWeekNumber));
                }

                Week week = weeks[dayItem.Week];
                week.AddDayItem(dayItem);
            }

            return weeks;
        }


        // -----


        protected override void OnActivate()
        {
            base.OnActivate();

            if (Listing != null) {
                WindowTitle.Text = GenerateWindowTitle(Listing);
            }
        }


        // -----


        public void Handle(ListingMessage message)
        {
            Listing = message.Listing;
            Reset(Listing);
        }
    }
}
