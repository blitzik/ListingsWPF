using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Exceptions;
using Listings.Facades;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingItemViewModel : ViewModel
    {
        private int _startTime;
        public int StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WorkedHours));
            }
        }


        private int _endTime;
        public int EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WorkedHours));
            }
        }


        private int _lunchStart;
        public int LunchStart
        {
            get { return _lunchStart; }
            set
            {
                _lunchStart = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WorkedHours));
            }
        }


        private int _lunchEnd;
        public int LunchEnd
        {
            get { return _lunchEnd; }
            set
            {
                _lunchEnd = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WorkedHours));
            }
        }


        private int _workedHours;
        public int WorkedHours
        {
            get { return _endTime - _startTime - (_lunchEnd - _lunchStart); }
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


        private int HoursTick = (new Time("00:15:00")).TotalSeconds;


        private DelegateCommand _shiftStartAddCommand;
        public DelegateCommand ShiftStartAddCommand
        {
            get
            {
                if (_shiftStartAddCommand == null) {
                    _shiftStartAddCommand = new DelegateCommand(
                        p => {
                            StartTime += HoursTick;
                        }, 
                        p => {
                            return StartTime < LunchStart;
                        });
                }
                return _shiftStartAddCommand;
            }
        }


        private DelegateCommand _shiftStartSubCommand;
        public DelegateCommand ShiftStartSubCommand
        {
            get
            {
                if (_shiftStartSubCommand == null) {
                    _shiftStartSubCommand = new DelegateCommand(
                        p => {
                            StartTime -= HoursTick;
                        },
                        p => {
                            return StartTime > 0;
                        });
                }
                return _shiftStartSubCommand;
            }
        }


        private DelegateCommand _shiftEndAddCommand;
        public DelegateCommand ShiftEndAddCommand
        {
            get
            {
                if (_shiftEndAddCommand == null) {
                    _shiftEndAddCommand = new DelegateCommand(
                        p => {
                            EndTime += HoursTick;
                        },
                        p => {
                            return EndTime < 84600;
                        });
                }
                return _shiftEndAddCommand;
            }
        }


        private DelegateCommand _shiftEndSubCommand;
        public DelegateCommand ShiftEndSubCommand
        {
            get
            {
                if (_shiftEndSubCommand == null) {
                    _shiftEndSubCommand = new DelegateCommand(
                        p => {
                            EndTime -= HoursTick;
                        },
                        p => {
                            return EndTime > LunchEnd;
                        });
                }
                return _shiftEndSubCommand;
            }
        }


        private DelegateCommand _lunchStartAddCommand;
        public DelegateCommand LunchStartAddCommand
        {
            get
            {
                if (_lunchStartAddCommand == null) {
                    _lunchStartAddCommand = new DelegateCommand(
                        p => {
                            LunchStart += HoursTick;
                        },
                        p => {
                            return LunchStart < LunchEnd;
                        });
                }
                return _lunchStartAddCommand;
            }
        }


        private DelegateCommand _lunchStartSubCommand;
        public DelegateCommand LunchStartSubCommand
        {
            get
            {
                if (_lunchStartSubCommand == null) {
                    _lunchStartSubCommand = new DelegateCommand(
                        p => {
                            LunchStart -= HoursTick;
                        },
                        p => {
                            return LunchStart > StartTime;
                        });
                }
                return _lunchStartSubCommand;
            }
        }


        private DelegateCommand _lunchEndAddCommand;
        public DelegateCommand LunchEndAddCommand
        {
            get
            {
                if (_lunchEndAddCommand == null) {
                    _lunchEndAddCommand = new DelegateCommand(
                        p => {
                            LunchEnd += HoursTick;
                        },
                        p => {
                            return LunchEnd < EndTime;
                        });
                }
                return _lunchEndAddCommand;
            }
        }


        private DelegateCommand _lunchEndSubCommand;
        public DelegateCommand LunchEndSubCommand
        {
            get
            {
                if (_lunchEndSubCommand == null) {
                    _lunchEndSubCommand = new DelegateCommand(
                        p => {
                            LunchEnd -= HoursTick;
                        },
                        p => {
                            return LunchEnd > LunchStart;
                        });
                }
                return _lunchEndSubCommand;
            }
        }


        private DelegateCommand _saveListingItemCommand;
        public DelegateCommand SaveListingItemCommand
        {
            get
            {
                if (_saveListingItemCommand == null) {
                    _saveListingItemCommand = new DelegateCommand(p => SaveListingItem());
                }
                return _saveListingItemCommand;
            }
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
                _startTime = l.ShiftStart.TotalSeconds;
                _endTime = l.ShiftEnd.TotalSeconds;
                _lunchStart = l.ShiftLunchStart.TotalSeconds;
                _lunchEnd = l.ShiftLunchEnd.TotalSeconds;
                _locality = l.Locality;
            } else {
                _startTime = (new Time("06:00").TotalSeconds);
                _endTime = (new Time("16:00").TotalSeconds);
                _lunchStart = (new Time("11:00").TotalSeconds);
                _lunchEnd = (new Time("12:00").TotalSeconds);
            }
        }


        public delegate void SaveListingItemHandler(object sender, ListingItemArgs args);
        public event SaveListingItemHandler OnListingItemSaving;
        private void SaveListingItem()
        {
            Time s = new Time(_startTime);
            Time e = new Time(_endTime);
            Time ls = new Time(_lunchStart);
            Time le = new Time(_lunchEnd);

            ListingItem newItem = _dayItem.Listing.ReplaceItem(_dayItem.Day, _locality, s, e, ls, le);

            _listingFacade.Save(_dayItem.Listing);

            SaveListingItemHandler handler = OnListingItemSaving;
            if (handler != null) {
                handler(this, new ListingItemArgs(newItem));
            }
        }

        
    }
}
