using Caliburn.Micro;
using Listings.Commands;
using Listings.Domain;
using Listings.Facades;
using Listings.Messages;
using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Listings.Views
{
    public class ListingsOverviewViewModel : BaseScreen
    {
        private DelegateCommand<Listing> _openListingCommand;
        public DelegateCommand<Listing> OpenListingCommand
        {
            get
            {
                if (_openListingCommand == null) {
                    _openListingCommand = new DelegateCommand<Listing>(p => OpenListing(p));
                }

                return _openListingCommand;
            }
        }


        private ICollectionView _listings;
        public ICollectionView Listings
        {
            get { return _listings; }
            set {
                _listings = value;
                value.GroupDescriptions.Add(new PropertyGroupDescription("Month"));

                NotifyOfPropertyChange(() => Listings);
            }
        }


        private List<int> _years = Date.GetYears(2010, "DESC");
        public List<int> Years
        {
            get { return _years; }
        }


        private List<string> _months = new List<string>(Date.Months);
        public List<string> Months
        {
            get { return _months; }
        }


        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                NotifyOfPropertyChange(() => SelectedYear);

                LoadListings(SelectedYear);
            }
        }


        private readonly ListingFacade _listingFacade;


        public ListingsOverviewViewModel(
            ListingFacade listingFacade
        ) {
            BaseWindowTitle = "Přehled výčetek";
            _listingFacade = listingFacade;
            Listings = CollectionViewSource.GetDefaultView(listingFacade.FindListings(DateTime.Now.Year));

            _selectedYear = DateTime.Now.Year;
        }


        private void LoadListings(int year)
        {
            Listings = CollectionViewSource.GetDefaultView(_listingFacade.FindListings(year));
        }


        private void OpenListing(Listing listing)
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage(nameof(ListingDetailViewModel)));
            EventAggregator.PublishOnUIThread(new ListingMessage(listing));
        }


        // -----


        protected override void OnActivate()
        {
            base.OnActivate();

            LoadListings(SelectedYear);
        }
    }
}
