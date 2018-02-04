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
    public class EmployerItemViewModel : ViewModel
    {
        private ViewModel _currentViewModel;
        public ViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (_currentViewModel == value) return;

                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }


        private EmployerDetailViewModel _employerDetailViewModel;
        public EmployerDetailViewModel EmployerDetailViewModel
        {
            get
            {
                if (_employerDetailViewModel == null) {
                    _employerDetailViewModel = new EmployerDetailViewModel(_employerFacade, Employer);
                    _employerDetailViewModel.OnDeletionClicked += (object sender, EventArgs args) => {
                        ChangeView(nameof(EmployerDeletionViewModel));
                    };
                }
                return _employerDetailViewModel;
            }
        }



        public delegate void EmployerDeletionHandler(object sender, EventArgs args);
        public event EmployerDeletionHandler OnDeletedEmployer;
        private EmployerDeletionViewModel _employerDeletionViewModel;
        public EmployerDeletionViewModel EmployerDeletionViewModel
        {
            get
            {
                if (_employerDeletionViewModel == null) {
                    _employerDeletionViewModel = new EmployerDeletionViewModel(_employerFacade, Employer);
                    _employerDeletionViewModel.OnDeletedEmployer += (object sender, EventArgs args) => {
                        EmployerDeletionHandler handler = OnDeletedEmployer;
                        if (handler != null) {
                            handler(this, EventArgs.Empty);
                        }
                    };

                    _employerDeletionViewModel.OnReturnBackClicked += (object sender, EventArgs args) => {
                        ChangeView(nameof(EmployerDetailViewModel));
                    };
                }
                return _employerDeletionViewModel;
            }
        }


        private DelegateCommand<string> _navigationCommand;
        public DelegateCommand<string> NavigationCommand
        {
            get
            {
                if (_navigationCommand == null) {
                    _navigationCommand = new DelegateCommand<string>(p => ChangeView(p));
                }

                return _navigationCommand;
            }
        }


        private Employer _employer;
        public Employer Employer
        {
            get { return _employer; }
        }


        private EmployerFacade _employerFacade;


        public EmployerItemViewModel(EmployerFacade employerFacade, Employer employer)
        {
            _employer = employer;
            _employerFacade = employerFacade;
            ChangeView(nameof(EmployerDetailViewModel));
        }


        public override void Reset()
        {
            ChangeView(nameof(EmployerDetailViewModel));
            _employerDetailViewModel.ResetName();
        }


        private void ChangeView(string viewCode)
        {
            // we dont want the same menu item to be clicked more than once
            if (CurrentViewModel != null && CurrentViewModel.GetType().Name == viewCode) {
                return;
            }

            switch (viewCode) {
                case nameof(EmployerDetailViewModel):
                    CurrentViewModel = EmployerDetailViewModel;
                    break;

                case nameof(EmployerDeletionViewModel):
                    CurrentViewModel = EmployerDeletionViewModel;
                    break;
            }
        }
    }
}
