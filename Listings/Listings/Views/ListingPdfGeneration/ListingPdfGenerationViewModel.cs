using Listings.Commands;
using Listings.Domain;
using Listings.Facades;
using Listings.Services;
using Listings.Utils;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Listings.Views
{
    public class ListingPdfGenerationViewModel : ViewModel
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
            set { _ownerName = value; RaisePropertyChanged(); }
        }


        private bool IsGeneratePdfButtonActive = true;
        private DelegateCommand<object> _generatePdfCommand;
        public DelegateCommand<object> GeneratePdfCommand
        {
            get
            {
                if (_generatePdfCommand == null) {
                    _generatePdfCommand = new DelegateCommand<object>(
                        p => GeneratePdf(),
                        p => IsGeneratePdfButtonActive == true
                    );
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
                RaisePropertyChanged();
            }
        }


        private SettingFacade _settingFacade;

        private DefaultSettings _defaultSettings;


        public ListingPdfGenerationViewModel(SettingFacade settingFacade, string windowTitle)
        {
            _settingFacade = settingFacade;
            WindowTitle = windowTitle;

            _defaultSettings = settingFacade.GetDefaultSettings();

            PdfSetting = new DefaultListingPdfReportSetting(_defaultSettings.Pdfsetting);
        }


        private void GeneratePdf()
        {
            IsGeneratePdfButtonActive = false;
            GeneratePdfCommand.RaiseCanExecuteChanged();

            DefaultListingPdfReport report = new DefaultListingPdfReport(Listing);
            report.OwnerName = OwnerName;

            report.Setting = _pdfSetting;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            pdfRenderer.Document = report.Document;

            Thread t = new Thread(delegate () { pdfRenderer.RenderDocument(); });
            t.Start();
            //pdfRenderer.RenderDocument();
            //pdfRenderer.PdfDocument.Save(Path.Combine(path, "test.pdf"));

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF dokument (*.pdf)|*.pdf";
            saveFileDialog.FileName = string.Format("{0} {1} - {2}", Date.Months[12 - Listing.Month], Listing.Year, Listing.Name);
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                pdfRenderer.PdfDocument.Save(saveFileDialog.FileName);
            }

            IsGeneratePdfButtonActive = true;
            GeneratePdfCommand.RaiseCanExecuteChanged();
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


        public delegate void ReturnBackHandler(object sender, EventArgs args);
        public event ReturnBackHandler OnReturnBackClicked;
        private void ReturnBack()
        {
            ReturnBackHandler handler = OnReturnBackClicked;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
