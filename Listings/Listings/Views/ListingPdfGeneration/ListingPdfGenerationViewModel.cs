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
using System.Threading.Tasks;

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


        private DefaultListingPdfReportSetting _pdfSetting;
        public DefaultListingPdfReportSetting PdfSetting
        {
            get { return _pdfSetting; }
        }


        private SettingFacade _settingFacade;

        private DefaultSettings _defaultSettings;


        public ListingPdfGenerationViewModel(SettingFacade settingFacade, string windowTitle)
        {
            _settingFacade = settingFacade;
            WindowTitle = windowTitle;

            _pdfSetting = new DefaultListingPdfReportSetting();
        }


        private void GeneratePdf()
        {
            string path = GetFilePath();

            DefaultListingPdfReport report = new DefaultListingPdfReport(Listing);
            report.OwnerName = OwnerName;

            report.Setting = _pdfSetting;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            pdfRenderer.Document = report.Document;
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save(Path.Combine(path, "test.pdf"));
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

            _pdfSetting.ResetSettings();
        }
    }
}
