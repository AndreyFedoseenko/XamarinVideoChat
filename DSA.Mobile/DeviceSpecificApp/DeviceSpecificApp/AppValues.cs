using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSpecificApp
{
    public static class AppValues
    {
        public static string BaseServerUrl { get; private set; }

        static AppValues()
        {
            BaseServerUrl = "https://devicespecificappserver.azurewebsites.net";
        }
    }
}
