using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listings.Domain;
using Perst;

namespace Listings.Services.Entities
{
    public class EmployerFactory : IEmployerFactory
    {
        private Storage _storage;


        public EmployerFactory(Storage storage)
        {
            _storage = storage;
        }


        public Employer Create(string name)
        {
            return new Employer(_storage, name);
        }
    }
}
