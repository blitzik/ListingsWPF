using Listings.Facades;
using Listings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class SettingsViewModel : ViewModel
    {
        private WorkedTimeViewModel _workedTimeViewModel;
        public WorkedTimeViewModel WorkedTimeViewModel
        {
            get { return _workedTimeViewModel; }
        }


        private ListingFacade _listingFacade;


        public SettingsViewModel(ListingFacade listingFacade, string windowTitle)
        {
            _listingFacade = listingFacade;
            WindowTitle = windowTitle;

            _workedTimeViewModel = new WorkedTimeViewModel();
        }
    }
}
