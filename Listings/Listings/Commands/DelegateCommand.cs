using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Listings.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;


        public event EventHandler CanExecuteChanged;


        public DelegateCommand(Action<object> execute) : this(execute, null)
        {
        }


        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }


        public bool CanExecute(object parameters)
        {
            return _canExecute == null ? true : _canExecute(parameters);
        }


        public void Execute(object parameters)
        {
            _execute(parameters);
        }


        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
