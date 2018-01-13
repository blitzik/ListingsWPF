using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class ResultObject
    {
        private readonly bool _success;
        public bool Success
        {
            get { return _success; }
        }


        private List<string> _messages;


        public ResultObject(bool success)
        {
            _messages = new List<string>();
            _success = success;
        }


        public void AddMessage(string message)
        {
            _messages.Add(message);
        }


        public List<string> GetMessages()
        {
            return new List<string>(_messages);
        }


        public string GetLastMessage()
        {
            return _messages.Last();
        }
    }
}
