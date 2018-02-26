using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Listings.Views
{
    public class ProgressViewModel : ScreenBaseViewModel
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


        public ProgressViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            Text = "Probíhá zpracování Vašeho požadavku";
        }
    }
}
