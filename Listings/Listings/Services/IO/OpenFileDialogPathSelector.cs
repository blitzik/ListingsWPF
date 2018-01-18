using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Listings.Services.IO
{
    public class OpenFileDialogPathSelector : IFilePathSelector
    {
        public string GetFilePath(Action<object> settingSetter = null)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (settingSetter != null) {
                settingSetter.Invoke(d);
            }

            if (d.ShowDialog() == DialogResult.OK) {
                return d.FileName;
            }

            return string.Empty;
        }
    }
}
