using Caliburn.Micro;
using Listings.Domain;
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


        protected PageTitle _windowTitle;
        public PageTitle WindowTitle
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
                WindowTitle.Text = value;
            }
        }


        protected IEventAggregator _eventAggregator;
        public IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
        }


        public ConductorOneActiveBaseViewModel(IEventAggregator eventAggregator)
        {
            _viewModelName = this.GetType().Name;
            _eventAggregator = eventAggregator;
            _windowTitle = new PageTitle();
        }

    }
}
