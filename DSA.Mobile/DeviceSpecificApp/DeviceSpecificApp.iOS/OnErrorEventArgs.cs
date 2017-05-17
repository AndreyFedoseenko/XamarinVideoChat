using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceSpecificApp.iOS
{
    public class OnErrorEventArgs : EventArgs
    {
        public OnErrorEventArgs(string s)
        {
            message = s;
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
