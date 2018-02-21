using Caliburn.Micro;
using Listings.Commands;
using Listings.Domain;
using Listings.Facades;
using Listings.Messages;
using Listings.Services;
using Listings.Services.IO;
using Listings.Services.Pdf;
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

                ResetSettings();
            }
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
                    _resetSettingsCommand = new DelegateCommand<object>(
                        p => ResetSettings(),
                        p => !_defaultSettings.Pdfsetting.IsEqual(_pdfSetting)
                    );
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
        private IListingPdfDocumentFactory _listingPdfDocumentFactory;


        private DefaultSettings _defaultSettings;


        public ListingPdfGenerationViewModel(
            IEventAggregator eventAggregator,
            SettingFacade settingFacade,
            IWindowManager windowManager,
            ISavingFilePathSelector savingFilePathSelector,
            IListingPdfDocumentFactory listingPdfDocumentFactory
        ) : base(eventAggregator) {
            eventAggregator.Subscribe(this);

            BaseWindowTitle = "Generování PDF dokumentu";

            _settingFacade = settingFacade;
            _windowManager = windowManager;
            _savingFilePathSelector = savingFilePathSelector;
            _listingPdfDocumentFactory = listingPdfDocumentFactory;

            _defaultSettings = settingFacade.GetDefaultSettings();

            PdfSetting = new DefaultListingPdfReportSetting(_defaultSettings.Pdfsetting);
            PdfSetting.OnPropertyChanged += (object sender, EventArgs args) => { ResetSettingsCommand.RaiseCanExecuteChanged(); };
        }


        private void GeneratePdf()
        {
            string filePath = _savingFilePathSelector.GetFilePath(string.Format("{0} {1} - {2}", Date.Months[12 - Listing.Month], Listing.Year, Listing.Name), PrepareDialog);
            if (filePath == null) {
                return;
            }

            ProgressBarWindowViewModel pb = new ProgressBarWindowViewModel() { Text = "Vytváří se Váš PDF dokument..." };
            Task.Run(() => {
                Document doc = _listingPdfDocumentFactory.Create(Listing, _pdfSetting);

                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
                pdfRenderer.Document = doc;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(filePath);

                pb.TryClose();
            });

            _windowManager.ShowDialog(pb);
        }


        private void PrepareDialog(object obj)
        {
            SaveFileDialog d = (SaveFileDialog)obj;
            d.Filter = "PDF dokument (*.pdf)|*.pdf";
        }


        private void ResetSettings()
        {
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
