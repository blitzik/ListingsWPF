using Caliburn.Micro;
using Listings.Domain;
using Listings.Services.ViewModelResolver;
using System;
using System.Collections.Generic;

namespace Listings.Views
{
    public abstract class BaseConductorOneActive : Conductor<IViewModel>.Collection.OneActive, IViewModel
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


        // property injection
        private IViewModelResolver<IViewModel> _viewModelResolver;
        public IViewModelResolver<IViewModel> ViewModelResolver
        {
            get { return _viewModelResolver; }
            set { _viewModelResolver = value; }
        }


        protected Dictionary<string, IViewModel> _viewModels = new Dictionary<string, IViewModel>();


        public override void ActivateItem(IViewModel item)
        {
            if (ActiveItem == item) {
                return;
            }

            string typeName = item.GetType().Name;
            if (!_viewModels.ContainsKey(typeName)) {
                _viewModels.Add(typeName, item);
            }

            base.ActivateItem(item);
        }


        protected void DisplayView(string viewModelName)
        {
            ActivateItem(GetViewModel(viewModelName));
        }


        protected IViewModel GetViewModel(string viewModelName)
        {
            IViewModel viewModel;
            if (!_viewModels.ContainsKey(viewModelName)) {
                viewModel = _viewModelResolver.Resolve(viewModelName);
                if (viewModel == null) {
                    throw new Exception("Requested ViewModel does not Exist!");
                }
                _viewModels.Add(viewModelName, viewModel);

            } else {
                viewModel = _viewModels[viewModelName];
            }

            return viewModel;
        }

    }
}
