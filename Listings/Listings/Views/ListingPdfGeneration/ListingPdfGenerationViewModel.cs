using Caliburn.Micro;
using Listings.Commands;
using Listings.Domain;
using Listings.Facades;
using Listings.Messages;
using Listings.Services;
using Listings.Services.IO;
using Listings.Utils;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Listings.Views
{
    public class ListingPdfGenerationViewModel : ScreenBaseViewModel, IHandle<ListingMessage>
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                _listing = value;

                _defaultSettings = _settingFacade.GetDefaultSettings();
                OwnerName = _defaultSettings.OwnerName;

                ResetSettings();
            }
        }


        private string _ownerName;
        public string OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; NotifyOfPropertyChange(() => OwnerName); }
        }


        private DelegateCommand<object> _generatePdfCommand;
        public DelegateCommand<object> GeneratePdfCommand
        {
            get
            {
                if (_generatePdfCommand == null) {
                    _generatePdfCommand = new DelegateCommand<object>(p => GeneratePdf());
                }
                return _generatePdfCommand;
            }
        }


        private DelegateCommand<object> _resetSettingsCommand;
        public DelegateCommand<object> ResetSettingsCommand
        {
            get
            {
                if (_resetSettingsCommand == null) {
                    _resetSettingsCommand = new DelegateCommand<object>(p => ResetSettings());
                }
                return _resetSettingsCommand;
            }
        }


        private DelegateCommand<object> _returnBackCommand;
        public DelegateCommand<object> ReturnBackCommand
        {
            get
            {
                if (_returnBackCommand == null) {
                    _returnBackCommand = new DelegateCommand<object>(p => ReturnBack());
                }
                return _returnBackCommand;
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
            }
        }


        private ISavingFilePathSelector _savingFilePathSelector;
        private SettingFacade _settingFacade;
        private IWindowManager _windowManager;


        private DefaultSettings _defaultSettings;


        public ListingPdfGenerationViewModel(IEventAggregator eventAggregator, SettingFacade settingFacade, IWindowManager windowManager, ISavingFilePathSelector savingFilePathSelector) : base(eventAggregator)
        {
            eventAggregator.Subscribe(this);

            BaseWindowTitle = "Generování PDF dokumentu";

            _settingFacade = settingFacade;
            _windowManager = windowManager;
            _savingFilePathSelector = savingFilePathSelector;

            _defaultSettings = settingFacade.GetDefaultSettings();

            PdfSetting = new DefaultListingPdfReportSetting(_defaultSettings.Pdfsetting);
        }


        private void GeneratePdf()
        {
            DefaultListingPdfReport report = new DefaultListingPdfReport(Listing);
            report.OwnerName = OwnerName;
            report.Setting = _pdfSetting;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            pdfRenderer.Document = report.Document;

            string filePath = _savingFilePathSelector.GetFilePath(string.Format("{0} {1} - {2}", Date.Months[12 - Listing.Month], Listing.Year, Listing.Name), PrepareDialog);
            if (filePath == null) {
                pdfRenderer = null;
                report = null;
                return;
            }

            ProgressBarWindowViewModel pb = new ProgressBarWindowViewModel() { Text = "Vytváří se Váš PDF dokument..." };
            Task.Run(() => {
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(filePath);

                pdfRenderer = null;
                report = null;

                pb.TryClose();
            });

            _windowManager.ShowDialog(pb);
        }


        private void PrepareDialog(object obj)
        {
            SaveFileDialog d = (SaveFileDialog)obj;
            d.Filter = "PDF dokument (*.pdf)|*.pdf";
        }


        private string GetFilePath()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "_BlitzikListings");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            return path;
        }


        private void ResetSettings()
        {
            OwnerName = _defaultSettings.OwnerName;

            PdfSetting.UpdateBy(_defaultSettings.Pdfsetting);
        }


        private void ReturnBack()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(ListingDetailViewModel)));
        }


        // -----


        public void Handle(ListingMessage message)
        {
            Listing = message.Listing;
        }
    }
}
