using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public abstract class ConductorOneActiveBaseViewModel : Conductor<IViewModel>.Collection.OneActive, IViewModel
    {
        protected string _viewModelName;
        public string ViewModelName
        {
            get { return _viewModelName; }
        }


        protected string _windowTitle;
        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                NotifyOfPropertyChange(() => WindowTitle);
            }
        }


        protected string _baseWindowTitle;
        public string BaseWindowTitle
        {
            get { return _baseWindowTitle; }
            set
            {
                _baseWindowTitle = value;
                WindowTitle = value;
            }
        }


        protected IEventAggregator _eventAggregator;


        public ConductorOneActiveBaseViewModel(IEventAggregator eventAggregator , string windowTitle)
        {
            _viewModelName = this.GetType().Name;
            BaseWindowTitle = windowTitle;
            _eventAggregator = eventAggregator;
        }

    }
}
