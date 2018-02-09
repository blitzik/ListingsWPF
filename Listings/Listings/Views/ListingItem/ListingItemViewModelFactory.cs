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
        private readonly ListingFacade _listingFacade;
        private readonly SettingFacade _settingFacade;


        public ListingItemViewModelFactory(ListingFacade listingFacade, SettingFacade settingFacade)
        {
            _listingFacade = listingFacade;
            _settingFacade = settingFacade;
        }


        public ListingItemViewModel Create(string windowTitle, DayItem dayItem)
        {
            return new ListingItemViewModel(windowTitle, dayItem, _listingFacade, _settingFacade);
        }
    }
}
