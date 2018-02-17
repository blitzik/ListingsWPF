using Caliburn.Micro;
using Listings.Facades;
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


        public ListingPdfGenerationViewModelFactory(IEventAggregator eventAggregator, SettingFacade settingFacade)
        {
            _eventAggregator = eventAggregator;
            _settingFacade = settingFacade;
        }


        public ListingPdfGenerationViewModel Create(string windowTitle)
        {
            return new ListingPdfGenerationViewModel(_eventAggregator, windowTitle, _settingFacade);
        }
    }
}
