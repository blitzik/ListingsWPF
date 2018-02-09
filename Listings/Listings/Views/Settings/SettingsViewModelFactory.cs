using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class SettingsViewModelFactory
    {
        private readonly SettingFacade _settingFacade;


        public SettingsViewModelFactory(SettingFacade settingFacade)
        {
            _settingFacade = settingFacade;
        }


        public SettingsViewModel Create(string windowTitle)
        {
            return new SettingsViewModel(windowTitle, _settingFacade);
        }
    }
}
