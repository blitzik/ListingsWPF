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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Listings.Views
{
    public class SettingsViewModel : ViewModel
    {
        private WorkedTimeSettingViewModel _workedTimeViewModel;
        public WorkedTimeSettingViewModel WorkedTimeViewModel
        {
            get { return _workedTimeViewModel; }
        }


        private string _ownerName;
        public string OwnerName
        {
            get { return _ownerName; }
            set
            {
                _ownerName = value;
                RaisePropertyChanged();
                CancelChangesCommand.RaiseCanExecuteChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
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
                RaisePropertyChanged();
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
                RaisePropertyChanged();
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
                RaisePropertyChanged();
                CancelChangesCommand.RaiseCanExecuteChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }


        // -----


        private DefaultSettings _defaultSetting;

        private ListingFacade _listingFacade;
        private SettingFacade _settingFacade;
        private IFilePathSelector _openFileDialogPathSelector;


        public SettingsViewModel(ListingFacade listingFacade, SettingFacade settingFacade, IFilePathSelector openFileDialogPathSelector, string windowTitle)
        {
            _listingFacade = listingFacade;
            _settingFacade = settingFacade;
            _openFileDialogPathSelector = openFileDialogPathSelector;
            WindowTitle = windowTitle;

            RefreshSettings();
        }


        public void RefreshSettings()
        {
            _defaultSetting = _settingFacade.GetDefaultSettings();

            OwnerName = _defaultSetting.OwnerName;

            PdfSetting = CreateNewPdfSetting(_defaultSetting.Pdfsetting);

            if (_workedTimeViewModel == null) {
                _workedTimeViewModel = new WorkedTimeSettingViewModel(_defaultSetting.Time, _defaultSetting.Time, _defaultSetting.TimeTickInMinutes);
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

            if (!string.Equals(_defaultSetting.OwnerName, string.IsNullOrEmpty(OwnerName) ? null : OwnerName)) {
                return true;
            }

            if (!_pdfSetting.IsEqual(_defaultSetting.Pdfsetting)) {
                return true;
            }

            return false;
        }


        public delegate void SaveSettingsHandler(object sender, EventArgs args);
        public event SaveSettingsHandler OnSavedSettings;
        private void SaveSettings()
        {
            _defaultSetting.Time = new TimeSetting(
                new Time(_workedTimeViewModel.StartTime),
                new Time(_workedTimeViewModel.EndTime),
                new Time(_workedTimeViewModel.LunchStart),
                new Time(_workedTimeViewModel.LunchEnd),
                new Time(_workedTimeViewModel.OtherHours)
            );
            _defaultSetting.OwnerName = string.IsNullOrEmpty(OwnerName) ? null : OwnerName;
            _defaultSetting.TimeTickInMinutes = _workedTimeViewModel.SelectedTimeTickInMinutes;
            _defaultSetting.Pdfsetting.UpdateBy(_pdfSetting);
            
            _settingFacade.SaveDefaultSetting(_defaultSetting);

            CancelChangesCommand.RaiseCanExecuteChanged();
            SaveSettingsCommand.RaiseCanExecuteChanged();

            SaveSettingsHandler handler = OnSavedSettings;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }


        public delegate void CancelChangesHandler(object sender, EventArgs args);
        public event CancelChangesHandler OnCanceledChanges;
        private void CancelChanges()
        {
            WorkedTimeViewModel.SetTime(_defaultSetting.Time);
            WorkedTimeViewModel.SelectedTimeTickInMinutes = _defaultSetting.TimeTickInMinutes;
            OwnerName = _defaultSetting.OwnerName;

            PdfSetting = CreateNewPdfSetting(_defaultSetting.Pdfsetting);

            CancelChangesHandler handler = OnCanceledChanges;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }


        private void Browse()
        {
            BackupFilePath = _openFileDialogPathSelector.GetFilePath(o => {
                OpenFileDialog d = (OpenFileDialog)o;
                d.DefaultExt = "." + Db4oObjectContainerFactory.DATABASE_EXTENSION;
                d.Filter = "Evidoo data (*.evdo)|*.evdo";
            });
        }


        private void CreateBackup()
        {

            DateTime now = DateTime.Now;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Evidoo data (*.evdo)|*.evdo";
            saveFileDialog.FileName = string.Format("Záloha dat - {0}-{1}-{2}", now.Day, now.Month, now.Year);
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                ProgressBarWindow pb = new ProgressBarWindow();
                pb.Owner = System.Windows.Application.Current.MainWindow;
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (object sender, DoWorkEventArgs e) => {
                    _settingFacade.BackupData(saveFileDialog.FileName);
                };
                bw.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => {
                    pb.Close();
                };
                bw.RunWorkerAsync();

                pb.ShowDialog();
            }
        }


        public delegate void AfterImportBackupHandler(object sender, EventArgs args);
        public event AfterImportBackupHandler OnAfterBackupImport;
        private void ImportBackup()
        {
            ResultObject ro = _settingFacade.ImportBackup(BackupFilePath);

            _defaultSetting = _settingFacade.GetDefaultSettings();
            RefreshSettings();

            BackupFilePath = null;
            ImportDataResultMessage = ro.GetLastMessage();

            AfterImportBackupHandler handler = OnAfterBackupImport;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
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
