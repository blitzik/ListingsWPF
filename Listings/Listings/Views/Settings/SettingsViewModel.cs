﻿using Caliburn.Micro;
using Listings.Commands;
using Listings.Domain;
using Listings.EventArguments;
using Listings.Facades;
using Listings.Services;
using Listings.Services.IO;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Listings.Views
{
    public class SettingsViewModel : ScreenBaseViewModel
    {
        private WorkedTimeSettingViewModel _workedTimeViewModel;
        public WorkedTimeSettingViewModel WorkedTimeViewModel
        {
            get { return _workedTimeViewModel; }
        }


        private DelegateCommand<object> _saveSettingsCommand;
        public DelegateCommand<object> SaveSettingsCommand
        {
            get
            {
                if (_saveSettingsCommand == null) {
                    _saveSettingsCommand = new DelegateCommand<object>(
                        p => SaveSettings(),
                        p => HasSettingsChanged()
                    );
                }
                return _saveSettingsCommand;
            }
        }


        private DelegateCommand<object> _cancelChangesCommand;
        public DelegateCommand<object> CancelChangesCommand
        {
            get
            {
                if (_cancelChangesCommand == null) {
                    _cancelChangesCommand = new DelegateCommand<object>(
                        p => CancelChanges(),
                        p => HasSettingsChanged()
                    );
                }
                return _cancelChangesCommand;
            }
        }


        private DelegateCommand<object> _backupDataCommand;
        public DelegateCommand<object> BackupDataCommand
        {
            get
            {
                if (_backupDataCommand == null) {
                    _backupDataCommand = new DelegateCommand<object>(p => CreateBackup());
                }
                return _backupDataCommand;
            }
        }


        private string _backupFilePath;
        public string BackupFilePath
        {
            get { return _backupFilePath; }
            set
            {
                _backupFilePath = value;
                NotifyOfPropertyChange(() => BackupFilePath);
                ImportDataCommand.RaiseCanExecuteChanged();
            }
        }


        private DelegateCommand<object> _browseCommand;
        public DelegateCommand<object> BrowseCommand
        {
            get
            {
                if (_browseCommand == null) {
                    _browseCommand = new DelegateCommand<object>(p => Browse());
                }
                return _browseCommand;
            }
        }


        private string _importDataResultMessage;
        public string ImportDataResultMessage
        {
            get { return _importDataResultMessage; }
            set
            {
                _importDataResultMessage = value;
                NotifyOfPropertyChange(() => ImportDataResultMessage);
            }
        }


        private DelegateCommand<object> _importDataCommand;
        public DelegateCommand<object> ImportDataCommand
        {
            get
            {
                if (_importDataCommand == null) {
                    _importDataCommand = new DelegateCommand<object>(
                        p => ImportBackup(),
                        p => !string.IsNullOrEmpty(BackupFilePath) && File.Exists(BackupFilePath)
                    );
                }
                return _importDataCommand;
            }
        }


        private DefaultListingPdfReportSetting _pdfSetting;
        public DefaultListingPdfReportSetting PdfSetting
        {
            get { return _pdfSetting; }
            set
            {
                _pdfSetting = value;
                NotifyOfPropertyChange(() => PdfSetting);
                CancelChangesCommand.RaiseCanExecuteChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }


        // -----


        private DefaultSettings _defaultSetting;

        private IWindowManager _windowManager;
        private ISavingFilePathSelector _savingFilePathSelector;
        private IOpeningFilePathSelector _openingFilePathSelector;
        private SettingFacade _settingFacade;


        public SettingsViewModel(
            IEventAggregator eventAggregator,
            IWindowManager windowManager,
            SettingFacade settingFacade,
            ISavingFilePathSelector savingFilePathSelector,
            IOpeningFilePathSelector openingFilePathSelector
        ) : base(eventAggregator) {
            _windowManager = windowManager;
            _settingFacade = settingFacade;
            _savingFilePathSelector = savingFilePathSelector;
            _openingFilePathSelector = openingFilePathSelector;
            BaseWindowTitle = "Nastavení";

            Reset();
        }


        public void Reset()
        {
            _defaultSetting = _settingFacade.GetDefaultSettings();

            PdfSetting = CreateNewPdfSetting(_defaultSetting.Pdfsetting);

            if (_workedTimeViewModel == null) {
                _workedTimeViewModel = new WorkedTimeSettingViewModel(_eventAggregator, _defaultSetting.Time, _defaultSetting.Time, _defaultSetting.TimeTickInMinutes);
                _workedTimeViewModel.OnTimeChanged += (object sender, WorkedTimeEventArgs args) =>
                {
                    CancelChangesCommand.RaiseCanExecuteChanged();
                    SaveSettingsCommand.RaiseCanExecuteChanged();
                };
                _workedTimeViewModel.OnTimeTickChanged += (object sender, EventArgs args) =>
                {
                    CancelChangesCommand.RaiseCanExecuteChanged();
                    SaveSettingsCommand.RaiseCanExecuteChanged();
                };
            }
            _workedTimeViewModel.SetTime(_defaultSetting.Time);
            _workedTimeViewModel.SelectedTimeTickInMinutes = _defaultSetting.TimeTickInMinutes;

            ImportDataResultMessage = null;
        }


        private bool HasSettingsChanged()
        {
            if (!_workedTimeViewModel.IsTimeEqual(_defaultSetting.Time)) {
                return true;
            }

            if (_workedTimeViewModel.SelectedTimeTickInMinutes != _defaultSetting.TimeTickInMinutes) {
                return true;
            }

            if (!_pdfSetting.IsEqual(_defaultSetting.Pdfsetting)) {
                return true;
            }

            return false;
        }


        private void SaveSettings()
        {
            _defaultSetting.Time = new TimeSetting(
                new Time(_workedTimeViewModel.StartTime),
                new Time(_workedTimeViewModel.EndTime),
                new Time(_workedTimeViewModel.LunchStart),
                new Time(_workedTimeViewModel.LunchEnd),
                new Time(_workedTimeViewModel.OtherHours)
            );
            _defaultSetting.TimeTickInMinutes = _workedTimeViewModel.SelectedTimeTickInMinutes;
            _defaultSetting.Pdfsetting.UpdateBy(_pdfSetting);
            
            _settingFacade.SaveDefaultSetting(_defaultSetting);

            CancelChangesCommand.RaiseCanExecuteChanged();
            SaveSettingsCommand.RaiseCanExecuteChanged();
        }


        private void CancelChanges()
        {
            WorkedTimeViewModel.SetTime(_defaultSetting.Time);
            WorkedTimeViewModel.SelectedTimeTickInMinutes = _defaultSetting.TimeTickInMinutes;

            PdfSetting = CreateNewPdfSetting(_defaultSetting.Pdfsetting);
        }


        private void Browse()
        {
            string filePath = _openingFilePathSelector.GetFilePath(null, obj => {
                OpenFileDialog d = (OpenFileDialog)obj;
                d.DefaultExt = "." + Db4oObjectContainerFactory.DATABASE_EXTENSION;
                d.Filter = "Evidoo data (*.evdo)|*.evdo";
            });
            if (string.IsNullOrEmpty(filePath)) {
                return;
            }

            BackupFilePath = filePath;
        }


        private void CreateBackup()
        {
            DateTime now = DateTime.Now;
            string filePath = _savingFilePathSelector.GetFilePath(
                string.Format("Záloha dat - {0}-{1}-{2}", now.Day, now.Month, now.Year),
                obj => {
                    SaveFileDialog d = (SaveFileDialog)obj;
                    d.Filter = "Evidoo data (*.evdo)|*.evdo";
                }
            );
            if (string.IsNullOrEmpty(filePath)) {
                return;
            }

            ProgressBarWindowViewModel pb = new ProgressBarWindowViewModel(EventAggregator);
            Task.Run(async () => {
                _settingFacade.BackupData(filePath);

                pb.Success = true;
                await Task.Delay(pb.ResultIconDelay);

                pb.TryClose();
            });

            _windowManager.ShowDialog(pb);
        }


        private async void ImportBackup()
        {
            ProgressBarWindowViewModel pb = new ProgressBarWindowViewModel(EventAggregator);
            Task<ResultObject> t = Task<ResultObject>.Run(async () => {
                ResultObject r = _settingFacade.ImportBackup(BackupFilePath);

                pb.Success = r.Success;
                await Task.Delay(pb.ResultIconDelay);

                pb.TryClose();

                return r;
            });

            _windowManager.ShowDialog(pb);

            ResultObject ro = await t;

            _defaultSetting = _settingFacade.GetDefaultSettings();
            Reset();

            BackupFilePath = null;
            ImportDataResultMessage = ro.GetLastMessage();
        }


        private DefaultListingPdfReportSetting CreateNewPdfSetting(DefaultListingPdfReportSetting oldSetting)
        {
            DefaultListingPdfReportSetting setting = new DefaultListingPdfReportSetting(oldSetting);
            setting.OnPropertyChanged += (object sender, EventArgs args) => {
                CancelChangesCommand.RaiseCanExecuteChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            };

            return setting;
        }
    }
}
