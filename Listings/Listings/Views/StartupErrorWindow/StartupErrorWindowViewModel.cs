using Listings.Commands;

namespace Listings.Views
{
    public class StartupErrorWindowViewModel : BaseScreen
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
        

        public StartupErrorWindowViewModel()
        {
        }
    }
}
