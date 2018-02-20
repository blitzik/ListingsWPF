using Caliburn.Micro;
using Listings.Domain;
using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ListingItemViewModelFactory
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ListingFacade _listingFacade;
        private readonly SettingFacade _settingFacade;


        public ListingItemViewModelFactory(IEventAggregator eventAggregator, ListingFacade listingFacade, SettingFacade settingFacade)
        {
            _eventAggregator = eventAggregator;
            _listingFacade = listingFacade;
            _settingFacade = settingFacade;
        }


        public ListingItemViewModel Create(DayItem dayItem)
        {
            ListingItemViewModel vm = new ListingItemViewModel(_eventAggregator, _listingFacade, _settingFacade);

            return vm;
        }
    }
}
