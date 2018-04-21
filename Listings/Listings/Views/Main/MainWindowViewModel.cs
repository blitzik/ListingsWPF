using Caliburn.Micro;
using Listings.Domain;
using Listings.Messages;
using System.Reflection;

namespace Listings.Views
{
    public class MainWindowViewModel : BaseConductorOneActive, IHandle<ChangeViewMessage>
    {
        private PageTitle _title = new PageTitle();
        public PageTitle Title
        {
            get { return _title; }
            private set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }


        private string _version;
        public string AppVersion
        {
            get { return _version; }
        }


        public MainWindowViewModel()
        {
            _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            DisplayListingsOverview();
        }


        public void DisplayListingsOverview()
        {
            DisplayView(nameof(ListingsOverviewViewModel));
        }


        public void DisplayListingCreation()
        {
            DisplayView(nameof(ListingViewModel));
        }


        public void DisplayEmployersList()
        {
            DisplayView(nameof(EmployersViewModel));
        }


        public void DisplaySettings()
        {
            DisplayView(nameof(SettingsViewModel));
        }


        public void DisplayEmptyListingsGeneration()
        {
            DisplayView(nameof(EmptyListingsGenerationViewModel));
        }        


        // -----


        public void Handle(ChangeViewMessage message)
        {
            ActivateItem(GetViewModel(message.ViewModelName));
        }


        // -----


        public override void ActivateItem(IViewModel item)
        {
            Title = item.WindowTitle;

            base.ActivateItem(item);
        }


    }
}