using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public class ProgressBarWindowViewModel : Screen
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


        public ProgressBarWindowViewModel()
        {
            Text = "Probíhá zpracování Vašeho požadavku";
        }
    }
}
