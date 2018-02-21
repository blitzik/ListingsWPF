using Caliburn.Micro;
using Listings.Facades;
using Listings.Services.IO;
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
        private readonly IWindowManager _windowManager;
        private readonly ISavingFilePathSelector _savingFilePathSelector;
        private readonly IOpeningFilePathSelector _openingFilePathSelector;


        public SettingsViewModelFactory(IEventAggregator eventAggregator, IWindowManager windowManager, ISavingFilePathSelector savingFilePathSelector, IOpeningFilePathSelector openingFilePathSelector, SettingFacade settingFacade)
        {
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
            _savingFilePathSelector = savingFilePathSelector;
            _openingFilePathSelector = openingFilePathSelector;
            _settingFacade = settingFacade;
        }


        public SettingsViewModel Create(string windowTitle)
        {
            SettingsViewModel vm = new SettingsViewModel(_eventAggregator, _windowManager, _settingFacade, _savingFilePathSelector, _openingFilePathSelector);
            vm.BaseWindowTitle = windowTitle;

            return vm;
        }
    }
}
