using DeviceSpecificApp.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSpecificApp.Providers
{
    public class AuthentificationProvider
    {
        public async Task<bool> Authentificate(string email,string password, string registrationId)
        {
            try
            {
                var clientUser = new User()
                {
                    Email = email,
                    Password = password,
                    Installation = registrationId
                };
                await App.MobileClient.LoginAsync("custom", JObject.FromObject(clientUser));
                return App.MobileClient.CurrentUser != null
                    && !string.IsNullOrEmpty(App.MobileClient.CurrentUser.MobileServiceAuthenticationToken);
            }
            catch(Exception ex)
            {
                Debugger.Break();
            }
            return false;
        }
    }
}
