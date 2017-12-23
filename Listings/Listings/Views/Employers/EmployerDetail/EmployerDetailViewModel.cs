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
    public class EmployerDetailViewModel : ViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
                EmployerNameSaveCommand.RaiseCanExecuteChanged();
            }
        }


        private Employer _employer;
        public Employer Employer
        {
            get { return _employer; }
        }


        private DelegateCommand<object> _employerDeletionCommand;
        public DelegateCommand<object> EmployerDeletionCommand
        {
            get
            {
                if (_employerDeletionCommand == null) {
                    _employerDeletionCommand = new DelegateCommand<object>(p => DisplayDeletion());
                }
                return _employerDeletionCommand;
            }
        }


        private DelegateCommand<object> _employerNameSaveCommand;
        public DelegateCommand<object> EmployerNameSaveCommand
        {
            get
            {
                if (_employerNameSaveCommand == null) {
                    _employerNameSaveCommand = new DelegateCommand<object>(
                        p => SaveEmployerChanges(),
                        p => _name != _employer.Name
                    );
                }
                return _employerNameSaveCommand;
            }
        }


        private EmployerFacade _employerFacade;


        public EmployerDetailViewModel(EmployerFacade employerFacade, Employer employer)
        {
            _employer = employer;
            Name = employer.Name;

            _employerFacade = employerFacade;
        }


        private void SaveEmployerChanges()
        {
            _employer.Name = Name;
            _employerFacade.Save(_employer);

            EmployerNameSaveCommand.RaiseCanExecuteChanged();
        }


        public delegate void DisplayEmployerDeletionHandler(object sender, EventArgs args);
        public event DisplayEmployerDeletionHandler OnDeletionClicked;
        private void DisplayDeletion()
        {
            DisplayEmployerDeletionHandler handler = OnDeletionClicked;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

    }
}
