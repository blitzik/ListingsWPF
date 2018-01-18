using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Services.IO
{
    public class NullFilePathSelector : IFilePathSelector
    {
        public string GetFilePath(Action<object> settingSetter = null)
        {
            return string.Empty;
        }
    }
}
