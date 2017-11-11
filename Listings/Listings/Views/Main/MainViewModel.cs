using Listings.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Listings.Views.Main
{
    public class MainViewModel : ViewModel
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


        private ICommand _navigationCommand;
        public ICommand NavigationCommand
        {
            get
            {
                if (_navigationCommand == null) {
                    _navigationCommand = new DelegateCommand(p => OnNav((string)p));
                }

                return _navigationCommand;
            }
        }


        private void OnNav(string destination)
        {            
        }
    }
}
