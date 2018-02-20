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
            _eventAggregator = eventAggregator;
            _settingFacade = settingFacade;
        }


        public SettingsViewModel Create(string windowTitle)
        {
            SettingsViewModel vm = new SettingsViewModel(_eventAggregator, _settingFacade);
            vm.BaseWindowTitle = windowTitle;

            return vm;
        }
    }
}
