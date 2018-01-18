using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Listings.Services.IO
{
    public class SaveFileDialogPathSelector : IFilePathSelector
    {
        public string GetFilePath(Action<object> settingSetter = null)
        {
            SaveFileDialog sd = new SaveFileDialog();
            if (settingSetter != null) {
                settingSetter.Invoke(sd);
            }

            if (sd.ShowDialog() == DialogResult.OK) {
                return sd.FileName;
            }

            return string.Empty;
        }
    }
}
