using Caliburn.Micro;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Listings.Commands;
using Listings.Domain;
using Listings.Facades;
using Listings.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class StartupErrorWindowViewModel : ScreenBaseViewModel
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }


        private DelegateCommand<object> _closeAppCommand;
        public DelegateCommand<object> CloseAppCommand
        {
            get
            {
                if (_closeAppCommand == null) {
                    _closeAppCommand = new DelegateCommand<object>(p => TryClose());
                }
                return _closeAppCommand;
            }
        }
        

        private readonly Db4oObjectContainerFactory _dbFactory;
        private readonly IWindowManager _windowManager;

        public StartupErrorWindowViewModel(
            IEventAggregator eventAggregator
        ) : base(eventAggregator) {
        }
    }
}
