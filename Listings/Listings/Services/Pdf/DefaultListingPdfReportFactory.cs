using Listings.Domain;
using Listings.Utils;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Services.Pdf
{
    public class DefaultListingPdfReportFactory : IListingPdfDocumentFactory
    {
        private readonly IListingSectionFactory _listingSectionFactory;


        public DefaultListingPdfReportFactory(IListingSectionFactory listingSectionFactory)
        {
            _listingSectionFactory = listingSectionFactory;
        }


        public Document Create(Listing listing, DefaultListingPdfReportSetting settings)
        {
            Document doc = new Document();

            _listingSectionFactory.Create(doc, listing, settings);

            return doc;
        }
    }
}
