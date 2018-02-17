using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Views
{
    public interface IViewModel
    {
        string ViewModelName { get; }

        string BaseWindowTitle { get; set; }
        string WindowTitle { get; set; }
    }
}
