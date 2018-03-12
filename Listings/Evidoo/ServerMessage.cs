using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evidoo
{
    public class ServerMessage
    {
        private string _code;
        public string Code
        {
            get { return _code; }
        }


        private string _message;
        public string Message
        {
            get { return _message; }
        }


        public ServerMessage(string code, string message)
        {
            _code = code;
            _message = message;
        }
    }
}
