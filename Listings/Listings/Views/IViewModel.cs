using Listings.Domain;

namespace Listings.Views
{
    public interface IViewModel
    {
        string BaseWindowTitle { get; set; }
        PageTitle WindowTitle { get; set; }
    }
}
