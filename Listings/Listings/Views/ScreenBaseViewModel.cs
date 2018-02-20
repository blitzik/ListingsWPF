using Caliburn.Micro;
using Listings.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public abstract class ScreenBaseViewModel : Screen, IViewModel
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


        protected readonly IEventAggregator _eventAggregator;
        public IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
        }


        public ScreenBaseViewModel(IEventAggregator eventAggregator)
        {
            _viewModelName = this.GetType().Name;
            _eventAggregator = eventAggregator;
            WindowTitle = new PageTitle();
        }

    }
}
