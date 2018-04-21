using Listings.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Messages
{
    /// <summary>
    /// Message that navigates to an existing ViewModel
    /// </summary>
    public class ChangeViewMessage : IEventAggregatorMessage
    {
        private string _viewModelName;
        public string ViewModelName
        {
            get { return _viewModelName; }
        }


        public ChangeViewMessage(string viewName)
        {
            _viewModelName = viewName;
        }
    }
}
