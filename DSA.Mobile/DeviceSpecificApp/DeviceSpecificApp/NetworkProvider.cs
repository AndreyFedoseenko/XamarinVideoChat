using Newtonsoft.Json;
using OpenToxServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSpecificApp
{
    public class NetworkProvider
    {
        protected readonly HttpClient client;

        public NetworkProvider()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(AppValues.BaseServerUrl);
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<SessionInfo> GetSessionInfo()
        {
            try
            {
                var response = await this.client.GetAsync(AppValues.BaseServerUrl + "/api/session");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result =  JsonConvert.DeserializeObject<SessionInfo>(data);
                    return result;
                }
            }
            catch(Exception ex)
            {

            }
            return new SessionInfo();
        }
    }
}
