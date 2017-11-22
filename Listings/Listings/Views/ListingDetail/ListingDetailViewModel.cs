using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Exceptions;
using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingDetailViewModel : ViewModel
    {
        private readonly ListingFacade _listingFacade;


        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                _listing = value;
                _dayItems = null;
                _listingFacade.Activate(_listing, 4);
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<DayItem> _dayItems;
        public ObservableCollection<DayItem> DayItems
        {
            get
            {
                if (Listing == null) {
                    throw new InvalidStateException("Listing cannot be null!");
                }

                if (_dayItems == null) {
                    _dayItems = new ObservableCollection<DayItem>();
                    for (int day = 0; day < DateTime.DaysInMonth(Listing.Year, Listing.Month); day++) {
                        ListingItem listingItem = Listing.GetItemByDay(day + 1);
                        DayItem dayItem;
                        if (listingItem == null) {
                            dayItem = new DayItem(Listing, day + 1);
                        } else {
                            dayItem = new DayItem(listingItem);
                        }

                        _dayItems.Add(dayItem);
                    }
                }

                return _dayItems;
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


        public ListingDetailViewModel(ListingFacade listingFacade, string windowTitle)
        {
            _listingFacade = listingFacade;
            WindowTitle = windowTitle;
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
            _dayItems[day - 1] = new DayItem(Listing, day);

            _listingFacade.Save(Listing);
        }

    }
}
