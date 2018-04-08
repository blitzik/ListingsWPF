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
    public abstract class BaseScreen : Screen, IViewModel
    {
        protected PageTitle _windowTitle = new PageTitle();
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


        // property injection
        private IEventAggregator _eventAggregator;
        public IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
            set { _eventAggregator = value; }
        }
    }
}
