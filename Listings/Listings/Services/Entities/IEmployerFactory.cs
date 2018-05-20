using Listings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Services.Entities
{
    public interface IEmployerFactory
    {
        Employer Create(string name);
    }
}
