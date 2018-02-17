using Caliburn.Micro;
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
        private readonly IEventAggregator _eventAggregator;


        public SettingsViewModelFactory(IEventAggregator eventAggregator, SettingFacade settingFacade)
        {
            _settingFacade = settingFacade;
        }


        public SettingsViewModel Create(string windowTitle)
        {
            return new SettingsViewModel(_eventAggregator, windowTitle, _settingFacade);
        }
    }
}
