using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ProgressBarWindowViewModel : ConductorOneActiveBaseViewModel
    {
        public string Text
        {
            get { return ProgressViewModel.Text; }
            set
            {
                ProgressViewModel.Text = value;
            }
        }


        private ProgressViewModel _progressViewModel;
        public ProgressViewModel ProgressViewModel
        {
            get
            {
                if (_progressViewModel == null) {
                    _progressViewModel = new ProgressViewModel(EventAggregator);
                }
                return _progressViewModel;
            }
        }


        private SuccessViewModel _successViewModel;
        public SuccessViewModel SuccessViewModel
        {
            get
            {
                if (_successViewModel == null) {
                    _successViewModel = new SuccessViewModel(EventAggregator);
                }
                return _successViewModel;
            }
        }


        private bool? _success;
        public bool? Success
        {
            get { return _success; }
            set
            {
                _success = value;
                if (value == null) {
                    ActivateItem(ProgressViewModel);

                } else if (value == true) {
                    ActivateItem(SuccessViewModel);

                } else {
                    // todo
                }
            }
        }


        private int _resultIconDelay;
        public int ResultIconDelay
        {
            get { return _resultIconDelay; }
            set { _resultIconDelay = value; }
        }


        public ProgressBarWindowViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            Success = null;
            ResultIconDelay = 500;
        }


        protected override void OnActivate()
        {
            Success = null;
        }
    }
}
