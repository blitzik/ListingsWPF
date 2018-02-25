﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listings.Domain;
using MigraDoc.DocumentObjectModel;

namespace Listings.Services.Pdf
{
    public class MultipleListingReportFactory : IMultipleListingReportFactory
    {
        private readonly IListingSectionFactory _listingSectionFactory;


        public MultipleListingReportFactory(IListingSectionFactory listingSectionFactory)
        {
            _listingSectionFactory = listingSectionFactory;
        }


        public Document Create(List<Listing> listings, DefaultListingPdfReportSetting settings)
        {
            Document doc = new Document();

            foreach (Listing l in listings) {
                _listingSectionFactory.Create(doc, l, settings);
            }

            return doc;
        }
    }
}
