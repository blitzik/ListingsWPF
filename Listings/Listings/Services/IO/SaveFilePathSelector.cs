using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Listings.Services.IO
{
    public class SaveFilePathSelector : ISavingFilePathSelector
    {
        public string GetFilePath(string defaultFilePath, Action<object> modifier)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.FileName = defaultFilePath;
            modifier.Invoke(d);

            if (d.ShowDialog() == DialogResult.OK) {
                return d.FileName;
            }

            return null;
        }
    }
}
