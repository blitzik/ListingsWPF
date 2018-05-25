﻿using System;
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
        private StoragePool _storagePool;


        public EmployerFactory(StoragePool storagePool)
        {
            _storagePool = storagePool;
        }


        public Employer Create(string name)
        {
            return new Employer(_storagePool.GetByName(PerstStorageFactory.MAIN_DATABASE_NAME), name);
        }
    }
}
