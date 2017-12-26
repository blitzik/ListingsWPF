using Listings.Commands;
using Listings.EventArguments;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Services
{
    public class WorkedTimeViewModel : Views.ViewModel
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


        private bool _noTime;
        public bool NoTime
        {
            get { return _noTime; }
            set
            {
                _noTime = value;
                RaisePropertyChanged();
                if (value == true) {
                    StartTime = 0;
                    EndTime = 0;
                    LunchStart = 0;
                    LunchEnd = 0;
                    OtherHours = 0;
                    NoLunch = false;
                } else {
                    SetDefaultTimes();
                }
            }
        }


        public Time WorkedHours
        {
            get { return new Time(_endTime - _startTime - (_lunchEnd - _lunchStart) + _otherHours); }
        }


        private int HoursTick = (new Time("00:05:00")).TotalSeconds;


        private DelegateCommand<object> _shiftStartAddCommand;
        public DelegateCommand<object> ShiftStartAddCommand
        {
            get
            {
                if (_shiftStartAddCommand == null) {
                    _shiftStartAddCommand = new DelegateCommand<object>(
                        p => StartTime += HoursTick,
                        p => ((NoLunch == false && StartTime < LunchStart) || (NoLunch == true && (StartTime + HoursTick) < EndTime)) && NoTime == false
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
                        p => StartTime > 0 && NoTime == false
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
                        p => EndTime < 86400 && NoTime == false // EndTime < 24:00
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
                        p => ((NoLunch == false && EndTime > LunchEnd) || (NoLunch == true && EndTime > (StartTime + HoursTick))) && NoTime == false
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
                        p => NoLunch == false && (LunchStart + HoursTick) < LunchEnd && NoTime == false
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
                        p => NoLunch == false && LunchStart > StartTime && NoTime == false
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
                        p => NoLunch == false && LunchEnd < EndTime && NoTime == false
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
                        p => NoLunch == false && LunchEnd > (LunchStart + HoursTick) && NoTime == false
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
                        p => WorkedHours >= 0 && NoTime == false
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
                        p => OtherHours > 0 && NoTime == false
                    );
                }
                return _otherHoursSubCommand;
            }
        }


        // Default Times
        public WorkedTimeViewModel()
        {
            SetDefaultTimes();
        }


        public WorkedTimeViewModel(Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            StartTime = start.TotalSeconds;
            EndTime = end.TotalSeconds;
            LunchStart = lunchStart.TotalSeconds;
            LunchEnd = lunchEnd.TotalSeconds;
            OtherHours = otherHours.TotalSeconds;

            _noTime = false;
            _noLunch = false;
            if (start == 0 && end == 0 && lunchStart == 0 && lunchEnd == 0 && otherHours == 0) {
                _noTime = true;
            } else {
                if (lunchStart == 0 && lunchEnd == 0) {
                    _noLunch = true;
                }
            }
        }


        private void SetDefaultTimes()
        {
            StartTime = (new Time("06:00").TotalSeconds);
            EndTime = (new Time("16:00").TotalSeconds);
            LunchStart = (new Time("11:00").TotalSeconds);
            LunchEnd = (new Time("12:00").TotalSeconds);
            OtherHours = (new Time().TotalSeconds);
        }


        private void UpdateCommandsCanExecute()
        {
            ShiftStartAddCommand.RaiseCanExecuteChanged();
            ShiftStartSubCommand.RaiseCanExecuteChanged();
            ShiftEndAddCommand.RaiseCanExecuteChanged();
            ShiftEndSubCommand.RaiseCanExecuteChanged();

            LunchStartAddCommand.RaiseCanExecuteChanged();
            LunchStartSubCommand.RaiseCanExecuteChanged();
            LunchEndAddCommand.RaiseCanExecuteChanged();
            LunchEndSubCommand.RaiseCanExecuteChanged();

            OtherHoursAddCommand.RaiseCanExecuteChanged();
            OtherHoursSubCommand.RaiseCanExecuteChanged();
        }
    }

}
