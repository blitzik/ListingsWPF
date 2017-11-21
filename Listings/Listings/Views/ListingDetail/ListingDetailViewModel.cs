using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Exceptions;
using Listings.Facades;
using System;
using System.Collections.Generic;
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
                RaisePropertyChanged();
            }
        }


        private List<DayItem> _dayItems;
        public List<DayItem> DayItems
        {
            get
            {
                if (Listing == null) {
                    throw new InvalidStateException("Listing cannot be null!");
                }

                if (_dayItems == null) {
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

    }
}
