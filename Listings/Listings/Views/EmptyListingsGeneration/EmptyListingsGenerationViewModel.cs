using Caliburn.Micro;
using Listings.Commands;
using Listings.Domain;
using Listings.Services.IO;
using Listings.Services.Pdf;
using Listings.Utils;
using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Listings.Views
{
    public class EmptyListingsGenerationViewModel : ScreenBaseViewModel
    {
        private List<int> _years;
        public List<int> Years
        {
            get { return _years; }
        }


        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                NotifyOfPropertyChange(() => SelectedYear);
            }
        }


        private DelegateCommand<object> _generatePdfsCommand;
        public DelegateCommand<object> GeneratePdfsCommand
        {
            get
            {
                if (_generatePdfsCommand == null) {
                    _generatePdfsCommand = new DelegateCommand<object>(p => GeneratePdfs());
                }
                return _generatePdfsCommand;
            }
        }


        private readonly IWindowManager _windowManager;
        private readonly ISavingFilePathSelector _savingFilePathSelector;
        private readonly IMultipleListingReportFactory _multipleListingReportFactory;
        private readonly IListingReportGenerator _listingReportGenerator;


        public EmptyListingsGenerationViewModel(
            IEventAggregator eventAggregator,
            IWindowManager windowManager,
            ISavingFilePathSelector savingFilePathSelector,
            IMultipleListingReportFactory multipleListingReportFactory,
            IListingReportGenerator listingReportGenerator
        ) : base(eventAggregator) {
            BaseWindowTitle = "Generování prázných výčetek";
            SelectedYear = DateTime.Now.Year;

            _windowManager = windowManager;
            _savingFilePathSelector = savingFilePathSelector;
            _multipleListingReportFactory = multipleListingReportFactory;
            _listingReportGenerator = listingReportGenerator;

            _years = Date.GetYears(2010, "DESC");
            _years.Insert(0, _years[0] + 1);
        }


        private void GeneratePdfs()
        {
            string filePath = _savingFilePathSelector.GetFilePath(
                string.Format("Výčetky {0}", SelectedYear),
                obj => {
                    SaveFileDialog d = (SaveFileDialog)obj;
                    d.Filter = "PDF dokument (*.pdf)|*.pdf";
                }
            );

            if (string.IsNullOrEmpty(filePath)) {
                return;
            }

            ProgressBarWindowViewModel pb = new ProgressBarWindowViewModel();
            Task.Run(() => {
                List<Listing> list = new List<Listing>();
                for (int month = 0; month < 12; month++) {
                    list.Add(new Listing(SelectedYear, month + 1));
                }

                Document doc = _multipleListingReportFactory.Create(list, new DefaultListingPdfReportSetting());
                _listingReportGenerator.Save(filePath, doc);

                pb.TryClose();
            });

            _windowManager.ShowDialog(pb);
        }

    }
}
