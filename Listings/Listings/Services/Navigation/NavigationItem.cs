using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Services
{
    public class NavigationItem
    {
        private string _index;
        public string Index
        {
            get { return _index; }
            private set { _index = value; }
        }


        private string _text;
        public string Text
        {
            get { return _text; }
            private set { _text = value; }
        }


        private string _imageSourcePath;
        public string ImageSourcePath
        {
            get { return _imageSourcePath; }
            private set { _imageSourcePath = value; }
        }


        public NavigationItem(string index, string text, string imageSourcePath) : this(index, text)
        {
            ImageSourcePath = imageSourcePath;
        }


        public NavigationItem(string index, string text)
        {
            Text = text;
            Index = index;
        }
    }
}
