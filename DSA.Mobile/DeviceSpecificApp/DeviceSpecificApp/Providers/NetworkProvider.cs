using DeviceSpecificApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSpecificApp.Providers
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

        public async Task<List<string>> GetChats()
        {
            try
            {
                var builder = new StringBuilder(AppValues.BaseServerUrl + "/api/chat/chats");
                builder.Append("?");
                builder.Append(string.Format("{0}={1}", "userName", App.MobileClient.CurrentUser.UserId));

                var response = await this.client.GetAsync(builder.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<string>>(data);
                    return result;
                }
            }
            catch (Exception ex)
            {

            }
            return new List<string>();
        }

        public async Task<List<MessageInfo>> GetMessages(string chatName)
        {
            try
            {
                if (App.MobileClient.CurrentUser != null
                    && !string.IsNullOrEmpty(App.MobileClient.CurrentUser.MobileServiceAuthenticationToken))
                {
                    var builder = new StringBuilder(AppValues.BaseServerUrl + "/api/chat/messages");
                    builder.Append("?");
                    builder.Append(string.Format("{0}={1}&", "Email", App.MobileClient.CurrentUser.UserId));
                    builder.Append(string.Format("{0}={1}", "ChatName", chatName));

                    var response = await this.client.GetAsync(builder.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<List<MessageInfo>>(data);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new List<MessageInfo>();
        }

        public async Task<bool> CreateChat(string chatName)
        {
            try
            {
                if(App.MobileClient.CurrentUser != null 
                    && !string.IsNullOrEmpty(App.MobileClient.CurrentUser.MobileServiceAuthenticationToken))
                {
                    var model = new UserChatModel()
                    {
                        ChatName = chatName,
                        Email = App.MobileClient.CurrentUser.UserId
                    };

                    var response = await this.client.PostAsync(AppValues.BaseServerUrl + "/api/chat/create",
                        new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<bool> SendMessage(string chatName, string message)
        {
            try
            {
                if (App.MobileClient.CurrentUser != null
                    && !string.IsNullOrEmpty(App.MobileClient.CurrentUser.MobileServiceAuthenticationToken))
                {
                    var info = new MessageInfo()
                    {
                        SenderEmail = App.MobileClient.CurrentUser.UserId,
                        ChatName = chatName,
                        Text = message
                    };
                    var response = await this.client.PostAsync(AppValues.BaseServerUrl + "/api/chat/send",
                        new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        public async Task<bool> InvitePerson(string chatName, string personName)
        {
            try
            {
                if (App.MobileClient.CurrentUser != null
                    && !string.IsNullOrEmpty(App.MobileClient.CurrentUser.MobileServiceAuthenticationToken))
                {
                    var invitation = new Invitation()
                    {
                        Sender = App.MobileClient.CurrentUser.UserId,
                        ChatName = chatName,
                        Receiver = personName
                    };
                    var response = await this.client.PostAsync(AppValues.BaseServerUrl + "/api/chat/invite",
                        new StringContent(JsonConvert.SerializeObject(invitation), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<bool> AcceptInvitation(string chatName, string sender)
        {
            try
            {
                if (App.MobileClient.CurrentUser != null
                    && !string.IsNullOrEmpty(App.MobileClient.CurrentUser.MobileServiceAuthenticationToken))
                {
                    var invitation = new Invitation()
                    {
                        Sender = sender,
                        ChatName = chatName,
                        Receiver = App.MobileClient.CurrentUser.UserId
                    };
                    var response = await this.client.PostAsync(AppValues.BaseServerUrl + "/api/chat/accept",
                        new StringContent(JsonConvert.SerializeObject(invitation), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
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
