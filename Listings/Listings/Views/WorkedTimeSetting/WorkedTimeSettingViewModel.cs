using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class WorkedTimeSettingViewModel : Views.ViewModel
    {
        private int _startTime;
        public int StartTime
        {
            get { return _startTime; }
            set
            {
                TimeSetting.CheckTime(value, EndTime, LunchStart, LunchEnd, OtherHours);

                _startTime = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WorkedHours));
                SetFlags();
                UpdateCommandsCanExecute();
                ProcessEventOnTimeChanged();
            }
        }


        private int _endTime;
        public int EndTime
        {
            get { return _endTime; }
            set
            {
                TimeSetting.CheckTime(StartTime, value, LunchStart, LunchEnd, OtherHours);

                _endTime = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WorkedHours));
                SetFlags();
                UpdateCommandsCanExecute();
                ProcessEventOnTimeChanged();
            }
        }


        private int _lunchStart;
        public int LunchStart
        {
            get { return _lunchStart; }
            set
            {
                TimeSetting.CheckTime(StartTime, EndTime, value, LunchEnd, OtherHours);

                _lunchStart = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WorkedHours));
                SetFlags();
                UpdateCommandsCanExecute();
                ProcessEventOnTimeChanged();
            }
        }


        private int _lunchEnd;
        public int LunchEnd
        {
            get { return _lunchEnd; }
            set
            {
                TimeSetting.CheckTime(StartTime, EndTime, LunchStart, value, OtherHours);

                _lunchEnd = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WorkedHours));
                SetFlags();
                UpdateCommandsCanExecute();
                ProcessEventOnTimeChanged();
            }
        }


        private int _otherHours;
        public int OtherHours
        {
            get { return _otherHours; }
            set
            {
                TimeSetting.CheckTime(StartTime, EndTime, LunchStart, LunchEnd, value);

                _otherHours = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WorkedHours));
                SetFlags();
                UpdateCommandsCanExecute();
                ProcessEventOnTimeChanged();
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
                    _lunchStart = 0;
                    _lunchEnd = 0;

                } else {
                    _lunchStart = StartTime;
                    _lunchEnd = EndTime;
                }

                RaisePropertyChanged(nameof(LunchStart));
                RaisePropertyChanged(nameof(LunchEnd));
                RaisePropertyChanged(nameof(WorkedHours));

                ProcessEventOnTimeChanged();
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
                    _startTime = 0;
                    _endTime = 0;
                    _lunchStart = 0;
                    _lunchEnd = 0;
                    _otherHours = 0;
                    _noLunch = false;

                    RaisePropertyChanged(nameof(StartTime));
                    RaisePropertyChanged(nameof(EndTime));
                    RaisePropertyChanged(nameof(LunchStart));
                    RaisePropertyChanged(nameof(LunchEnd));
                    RaisePropertyChanged(nameof(OtherHours));
                    RaisePropertyChanged(nameof(WorkedHours));
                    RaisePropertyChanged(nameof(NoLunch));
                    UpdateCommandsCanExecute();

                } else {
                    SetDefaultTimes();
                }

                ProcessEventOnTimeChanged();
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


        public delegate void TimeChangedHandler(object sender, WorkedTimeEventArgs args);
        public event TimeChangedHandler OnTimeChanged;
        

        private TimeSetting _defaultTimeSettings;


        public WorkedTimeSettingViewModel(TimeSetting defaultTimeSettings, TimeSetting timeSetting)
        {
            _defaultTimeSettings = defaultTimeSettings;

            _startTime = timeSetting.Start.TotalSeconds;
            _endTime = timeSetting.End.TotalSeconds;
            _lunchStart = timeSetting.LunchStart.TotalSeconds;
            _lunchEnd = timeSetting.LunchEnd.TotalSeconds;
            _otherHours = timeSetting.OtherHours.TotalSeconds;

            SetFlags();
        }


        private void SetDefaultTimes()
        {
            TimeSetting setting;
            if (_defaultTimeSettings.HasNoTime) {
                // if default time has no time, we have to set some time otherwise there is no way of setting different time from default "HasNoTime"
                setting = new TimeSetting(
                    new Time("06:00"),
                    new Time("14:30"),
                    new Time("10:30"),
                    new Time("11:00"),
                    new Time("00:00")
                );

            } else {
                setting = _defaultTimeSettings;
            }

            _startTime = (setting.Start.TotalSeconds);
            RaisePropertyChanged(nameof(StartTime));

            _endTime = (setting.End.TotalSeconds);
            RaisePropertyChanged(nameof(EndTime));

            _lunchStart = (setting.LunchStart.TotalSeconds);
            RaisePropertyChanged(nameof(LunchStart));

            _lunchEnd = (setting.LunchEnd.TotalSeconds);
            RaisePropertyChanged(nameof(LunchEnd));

            _otherHours = (setting.OtherHours.TotalSeconds);
            RaisePropertyChanged(nameof(OtherHours));

            RaisePropertyChanged(nameof(WorkedHours));

            SetFlags();
            UpdateCommandsCanExecute();
        }


        private void SetFlags()
        {
            if (_startTime == 0 && _endTime == 0 && _lunchStart == 0 && _lunchEnd == 0 && _otherHours == 0) {
                _noTime = true;
                _noLunch = false;

            } else {
                _noTime = false;
                if (_lunchStart == 0 && _lunchEnd == 0) {
                    _noLunch = true;
                } else {
                    _noLunch = false;
                }
            }

            RaisePropertyChanged(nameof(NoTime));
            RaisePropertyChanged(nameof(NoLunch));
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


        private void ProcessEventOnTimeChanged()
        {
            if (OnTimeChanged != null) {
                OnTimeChanged(this, new WorkedTimeEventArgs(new Time(StartTime), new Time(EndTime), new Time(LunchStart), new Time(LunchEnd), new Time(OtherHours)));
            }
        }
    }

}
