﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Listings.Services.IO
{
    public class OpenFilePathSelector : IOpeningFilePathSelector
    {
        public string GetFilePath(string defaultFilePath, Action<object> modifier)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.FileName = defaultFilePath;
            modifier.Invoke(d);

            if (d.ShowDialog() == DialogResult.OK) {
                return d.FileName;
            }

            return null;
        }
    }
}
