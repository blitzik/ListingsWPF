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
        private readonly SettingFacade _settingFacade;


        public ListingPdfGenerationViewModelFactory(SettingFacade settingFacade)
        {
            _settingFacade = settingFacade;
        }


        public ListingPdfGenerationViewModel Create(string windowTitle)
        {
            return new ListingPdfGenerationViewModel(windowTitle, _settingFacade);
        }
    }
}
