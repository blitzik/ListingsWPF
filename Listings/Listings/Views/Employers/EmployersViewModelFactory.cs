using Caliburn.Micro;
using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class EmployersViewModelFactory
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly EmployerFacade _employerFacade;


        public EmployersViewModelFactory(IEventAggregator eventAggregator, EmployerFacade employerFacade)
        {
            _eventAggregator = eventAggregator;
            _employerFacade = employerFacade;
        }


        public EmployersViewModel Create(string windowTitle)
        {
            EmployersViewModel vm = new EmployersViewModel(_eventAggregator, _employerFacade);
            vm.BaseWindowTitle = windowTitle;

            return vm;
        }
    }
}
