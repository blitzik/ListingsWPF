using Caliburn.Micro;
using Listings.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Services.ViewModelResolver
{
    public class ViewModelResolver : IViewModelResolver<IViewModel>
    {
        private readonly SimpleContainer _container;


        public ViewModelResolver(SimpleContainer container)
        {
            _container = container;
        }


        public IViewModel Resolve(string viewModel)
        {
           return (IViewModel)_container.GetInstance(Type.GetType(viewModel), viewModel);
        }
    }
}
