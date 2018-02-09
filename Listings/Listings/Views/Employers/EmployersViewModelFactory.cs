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
        private readonly EmployerFacade _employerFacade;


        public EmployersViewModelFactory(EmployerFacade employerFacade)
        {
            _employerFacade = employerFacade;
        }


        public EmployersViewModel Create(string windowTitle)
        {
            return new EmployersViewModel(windowTitle, _employerFacade);
        }
    }
}
