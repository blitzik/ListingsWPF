using Listings.Domain;
using Listings.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Messages
{
    /// <summary>
    /// Message that delivers new ViewModel instance
    /// </summary>
    public class DisplayViewMessage
    {
        private IViewModel _viewModel;
        public IViewModel ViewModel
        {
            get { return _viewModel; }
        }


        public DisplayViewMessage(IViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
