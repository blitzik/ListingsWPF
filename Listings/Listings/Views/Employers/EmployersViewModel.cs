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
    public class EmployersViewModel : ViewModel
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
                RaisePropertyChanged();
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


        public EmployersViewModel(EmployerFacade employerFacade, string windowTitle)
        {
            _employerFacade = employerFacade;
            WindowTitle = windowTitle;

            List<Employer> foundEmployers = employerFacade.FindAllEmployers();
            _employers = new ObservableCollection<EmployerItemViewModel>();
            foreach (Employer e in foundEmployers) {
                _employers.Add(CreateEmployerItemViewModel(e));
            }
        }


        public void RestoreDefaultState()
        {
            List<Employer> foundEmployers = _employerFacade.FindAllEmployers();

            _employers = new ObservableCollection<EmployerItemViewModel>();
            foreach (Employer e in foundEmployers) {
                EmployerItemViewModel vm = CreateEmployerItemViewModel(e);
                vm.RestoreDefaultState();

                _employers.Add(vm);
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
            EmployerItemViewModel vm = new EmployerItemViewModel(_employerFacade, employer);
            vm.OnDeletedEmployer += OnEmployerDeletion;

            return vm;
        }


        private void OnEmployerDeletion(object sender, EventArgs args)
        {
            _employers.Remove((EmployerItemViewModel)sender);
        }
    }
}
