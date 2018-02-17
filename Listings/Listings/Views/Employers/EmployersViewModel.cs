using Caliburn.Micro;
using Listings.Commands;
using Listings.Domain;
using Listings.Facades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class EmployersViewModel : ScreenBaseViewModel
    {
        private EmployerFacade _employerFacade;


        private ObservableCollection<EmployerItemViewModel> _employers;
        public ObservableCollection<EmployerItemViewModel> Employers
        {
            get
            {
                return _employers;
            }
        }


        // -----


        private string _newEmployerName;
        public string NewEmployerName
        {
            get { return _newEmployerName; }
            set
            {
                _newEmployerName = value;
                NotifyOfPropertyChange(() => NewEmployerName);
                _saveNewEmployerCommand.RaiseCanExecuteChanged();
            }
        }


        private DelegateCommand<object> _saveNewEmployerCommand;
        public DelegateCommand<object> SaveNewEmployerCommand
        {
            get
            {
                if (_saveNewEmployerCommand == null) {
                    _saveNewEmployerCommand = new DelegateCommand<object>(p => SaveNewEmployer(), p => !string.IsNullOrEmpty(NewEmployerName));
                }
                return _saveNewEmployerCommand;
            }
        }


        public EmployersViewModel(IEventAggregator eventAggregator, string windowTitle, EmployerFacade employerFacade) : base(eventAggregator, windowTitle)
        {
            _employerFacade = employerFacade;

            List<Employer> foundEmployers = employerFacade.FindAllEmployers();
            _employers = new ObservableCollection<EmployerItemViewModel>();
            foreach (Employer e in foundEmployers) {
                _employers.Add(CreateEmployerItemViewModel(e));
            }
        }


        private void SaveNewEmployer()
        {
            Employer e = new Employer(NewEmployerName.Trim());
            _employerFacade.Save(e);

            Employers.Insert(0, CreateEmployerItemViewModel(e));

            NewEmployerName = null;
        }


        private EmployerItemViewModel CreateEmployerItemViewModel(Employer employer)
        {
            EmployerItemViewModel vm = new EmployerItemViewModel(_eventAggregator, _employerFacade, employer);
            vm.OnDeletedEmployer += OnEmployerDeletion;

            return vm;
        }


        private void OnEmployerDeletion(object sender, EventArgs args)
        {
            _employers.Remove((EmployerItemViewModel)sender);
        }


        // -----


        protected override void OnActivate()
        {
            foreach (EmployerItemViewModel vm in _employers) {
                vm.Reset();
            }
        }
    }
}
