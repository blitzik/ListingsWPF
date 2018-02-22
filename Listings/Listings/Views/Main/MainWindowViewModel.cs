﻿using Caliburn.Micro;
using Listings.Domain;
using Listings.Messages;
using Listings.Services.ViewModelResolver;
using System;
using System.Collections.Generic;

namespace Listings.Views
{
    public class MainWindowViewModel : Conductor<IViewModel>.Collection.OneActive, IHandle<ChangeViewMessage>
    {
        private PageTitle _title;
        public PageTitle Title
        {
            get { return _title; }
            private set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }


        private IViewModelResolver<IViewModel> _viewModelResolver;

        private Dictionary<string, IViewModel> _viewModels;


        public MainWindowViewModel(
            IEventAggregator eventAggregator,
            IViewModelResolver<IViewModel> viewModelResolver
        ) {
            _viewModels = new Dictionary<string, IViewModel>();
            _viewModelResolver = viewModelResolver;

            eventAggregator.Subscribe(this);

            DisplayListingsOverview();
        }


        public void DisplayListingsOverview()
        {
            ActivateItem(GetViewModel(nameof(ListingsOverviewViewModel)));
        }


        public void DisplayListingCreation()
        {
            ActivateItem(GetViewModel(nameof(ListingViewModel)));
        }


        public void DisplayEmployersList()
        {
            ActivateItem(GetViewModel(nameof(EmployersViewModel)));
        }


        public void DisplaySettings()
        {
            ActivateItem(GetViewModel(nameof(SettingsViewModel)));
        }


        public override void ActivateItem(IViewModel item)
        {
            string typeName = item.GetType().Name;
            if (!_viewModels.ContainsKey(typeName)) {
                _viewModels.Add(typeName, item);
            }

            Title = item.WindowTitle;
            base.ActivateItem(item);
        }


        // -----
       
        
        public void Handle(ChangeViewMessage message)
        {
            ActivateItem(GetViewModel(message.ViewModelName));
        }


        private IViewModel GetViewModel(string viewModelName)
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