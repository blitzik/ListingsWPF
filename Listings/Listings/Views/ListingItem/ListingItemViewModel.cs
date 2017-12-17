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
using System.Windows.Input;

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
                UpdateCommandsCanExecute();
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
                UpdateCommandsCanExecute();
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
                UpdateCommandsCanExecute();
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
                UpdateCommandsCanExecute();
            }
        }


        private int _otherHours;
        public int OtherHours
        {
            get { return _otherHours; }
            set
            {
                _otherHours = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WorkedHours));
                UpdateCommandsCanExecute();
            }
        }


        private bool _noLunch;
        public bool NoLunch
        {
            get { return _noLunch; }
            set
            {
                _noLunch = value;
                RaisePropertyChanged();
                if (value == true) {
                    LunchStart = 0;
                    LunchEnd = 0;

                } else {
                    LunchStart = StartTime;
                    LunchEnd = EndTime;
                }
            }
        }


        public Time WorkedHours
        {
            get { return new Time(_endTime - _startTime - (_lunchEnd - _lunchStart) + _otherHours); }
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


        private DelegateCommand<object> _shiftStartAddCommand;
        public DelegateCommand<object> ShiftStartAddCommand
        {
            get
            {
                if (_shiftStartAddCommand == null) {
                    _shiftStartAddCommand = new DelegateCommand<object>(
                        p => StartTime += HoursTick,
                        p => (NoLunch == false && StartTime < LunchStart) || (NoLunch == true && (StartTime + HoursTick) < EndTime)
                    );
                }
                return _shiftStartAddCommand;
            }
        }


        private DelegateCommand<object> _shiftStartSubCommand;
        public DelegateCommand<object> ShiftStartSubCommand
        {
            get
            {
                if (_shiftStartSubCommand == null) {
                    _shiftStartSubCommand = new DelegateCommand<object>(
                        p => StartTime -= HoursTick,
                        p => StartTime > 0
                    );
                }
                return _shiftStartSubCommand;
            }
        }


        private DelegateCommand<object> _shiftEndAddCommand;
        public DelegateCommand<object> ShiftEndAddCommand
        {
            get
            {
                if (_shiftEndAddCommand == null) {
                    _shiftEndAddCommand = new DelegateCommand<object>(
                        p => EndTime += HoursTick,
                        p => EndTime < 86400 // EndTime < 24:00
                    );
                }
                return _shiftEndAddCommand;
            }
        }


        private DelegateCommand<object> _shiftEndSubCommand;
        public DelegateCommand<object> ShiftEndSubCommand
        {
            get
            {
                if (_shiftEndSubCommand == null) {
                    _shiftEndSubCommand = new DelegateCommand<object>(
                        p => EndTime -= HoursTick,
                        p => (NoLunch == false && EndTime > LunchEnd) || (NoLunch == true && EndTime > (StartTime + HoursTick))
                    );
                }
                return _shiftEndSubCommand;
            }
        }


        private DelegateCommand<object> _lunchStartAddCommand;
        public DelegateCommand<object> LunchStartAddCommand
        {
            get
            {
                if (_lunchStartAddCommand == null) {
                    _lunchStartAddCommand = new DelegateCommand<object>(
                        p => LunchStart += HoursTick,
                        p => NoLunch == false && (LunchStart + HoursTick) < LunchEnd
                    );
                }
                return _lunchStartAddCommand;
            }
        }


        private DelegateCommand<object> _lunchStartSubCommand;
        public DelegateCommand<object> LunchStartSubCommand
        {
            get
            {
                if (_lunchStartSubCommand == null) {
                    _lunchStartSubCommand = new DelegateCommand<object>(
                        p => LunchStart -= HoursTick,
                        p => NoLunch == false && LunchStart > StartTime
                    );
                }
                return _lunchStartSubCommand;
            }
        }


        private DelegateCommand<object> _lunchEndAddCommand;
        public DelegateCommand<object> LunchEndAddCommand
        {
            get
            {
                if (_lunchEndAddCommand == null) {
                    _lunchEndAddCommand = new DelegateCommand<object>(
                        p => LunchEnd += HoursTick,
                        p => NoLunch == false && LunchEnd < EndTime
                    );
                }
                return _lunchEndAddCommand;
            }
        }


        private DelegateCommand<object> _lunchEndSubCommand;
        public DelegateCommand<object> LunchEndSubCommand
        {
            get
            {
                if (_lunchEndSubCommand == null) {
                    _lunchEndSubCommand = new DelegateCommand<object>(
                        p => LunchEnd -= HoursTick,
                        p => NoLunch == false && LunchEnd > (LunchStart + HoursTick)
                    );
                }
                return _lunchEndSubCommand;
            }
        }


        private DelegateCommand<object> _otherHoursAddCommand;
        public DelegateCommand<object> OtherHoursAddCommand
        {
            get
            {
                if (_otherHoursAddCommand == null) {
                    _otherHoursAddCommand = new DelegateCommand<object>(
                        p => OtherHours += HoursTick,
                        p => WorkedHours >= 0
                    );
                }
                return _otherHoursAddCommand;
            }
        }


        private DelegateCommand<object> _otherHoursSubCommand;
        public DelegateCommand<object> OtherHoursSubCommand
        {
            get
            {
                if (_otherHoursSubCommand == null) {
                    _otherHoursSubCommand = new DelegateCommand<object>(
                        p => OtherHours -= HoursTick,
                        p => OtherHours > 0
                    );
                }
                return _otherHoursSubCommand;
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
                _otherHours = l.OtherHours.TotalSeconds;
                _locality = l.Locality;
            } else {
                _startTime = (new Time("06:00").TotalSeconds);
                _endTime = (new Time("16:00").TotalSeconds);
                _lunchStart = (new Time("11:00").TotalSeconds);
                _lunchEnd = (new Time("12:00").TotalSeconds);
                _otherHours = (new Time().TotalSeconds);
            }

            _noLunch = false;
            if (_lunchStart == 0 && _lunchEnd == 0) {
                _noLunch = true;
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
            Time oh = new Time(_otherHours);

            ListingItem newItem = _dayItem.Listing.ReplaceItem(_dayItem.Day, _locality, s, e, ls, le, oh);

            _listingFacade.Save(_dayItem.Listing);

            SaveListingItemHandler handler = OnListingItemSaving;
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


        private void UpdateCommandsCanExecute()
        {
            _shiftStartAddCommand.RaiseCanExecuteChanged();
            _shiftStartSubCommand.RaiseCanExecuteChanged();
            _shiftEndAddCommand.RaiseCanExecuteChanged();
            _shiftEndSubCommand.RaiseCanExecuteChanged();

            _lunchStartAddCommand.RaiseCanExecuteChanged();
            _lunchStartSubCommand.RaiseCanExecuteChanged();
            _lunchEndAddCommand.RaiseCanExecuteChanged();
            _lunchEndSubCommand.RaiseCanExecuteChanged();

            _otherHoursAddCommand.RaiseCanExecuteChanged();
            _otherHoursSubCommand.RaiseCanExecuteChanged();
        }

        
    }
}
