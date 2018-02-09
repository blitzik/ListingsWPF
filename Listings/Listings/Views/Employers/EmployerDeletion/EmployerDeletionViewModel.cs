using Listings.Commands;
using Listings.Domain;
using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class EmployerDeletionViewModel : ViewModel
    {
        private Employer _employer;
        public Employer Employer
        {
            get { return _employer; }
        }


        private DelegateCommand<object> _returnBackCommand;
        public DelegateCommand<object> ReturnBackCommand
        {
            get
            {
                if (_returnBackCommand == null) {
                    _returnBackCommand = new DelegateCommand<object>(p => ReturnBack());
                }
                return _returnBackCommand;
            }
        }


        private DelegateCommand<object> _deleteCommand;
        public DelegateCommand<object> DeleteCommand
        {
            get
            {
                if (_deleteCommand == null) {
                    _deleteCommand = new DelegateCommand<object>(p => DeleteEmployer());
                }
                return _deleteCommand;
            }
        }


        private EmployerFacade _employerFacade;


        public EmployerDeletionViewModel(EmployerFacade employerFacade, Employer employer) : base(null)
        {
            _employer = employer;
            _employerFacade = employerFacade;
        }


        public delegate void EmployerDeletionHandler(object sender, EventArgs args);
        public event EmployerDeletionHandler OnDeletedEmployer;
        private void DeleteEmployer()
        {
            _employerFacade.Delete(Employer);

            EmployerDeletionHandler handler = OnDeletedEmployer;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }


        public delegate void ReturnbackHandler(object sender, EventArgs args);
        public event ReturnbackHandler OnReturnBackClicked;
        private void ReturnBack()
        {
            ReturnbackHandler handler = OnReturnBackClicked;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
