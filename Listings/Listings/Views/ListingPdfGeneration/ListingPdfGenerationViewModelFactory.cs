using Caliburn.Micro;
using Listings.Facades;
using Listings.Services.IO;
using Listings.Services.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingPdfGenerationViewModelFactory
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly SettingFacade _settingFacade;
        private readonly IWindowManager _windowManager;
        private readonly ISavingFilePathSelector _savingFilePathSelector;
        private readonly IListingPdfDocumentFactory _listingPdfDocumentFactory;
        private readonly IListingReportGenerator _listingReportGenerator;

        public ListingPdfGenerationViewModelFactory(IEventAggregator eventAggregator, SettingFacade settingFacade, IWindowManager windowManager, ISavingFilePathSelector savingFilePathSelector, IListingPdfDocumentFactory listingPdfDocumentFactory, IListingReportGenerator listingReportGenerator)
        {
            _eventAggregator = eventAggregator;
            _settingFacade = settingFacade;
            _windowManager = windowManager;
            _savingFilePathSelector = savingFilePathSelector;
            _listingPdfDocumentFactory = listingPdfDocumentFactory;
            _listingReportGenerator = listingReportGenerator;
        }


        public ListingPdfGenerationViewModel Create(string windowTitle)
        {
            ListingPdfGenerationViewModel vm = new ListingPdfGenerationViewModel(_eventAggregator, _settingFacade, _windowManager, _savingFilePathSelector, _listingPdfDocumentFactory, _listingReportGenerator);

            return vm;
        }
    }
}
