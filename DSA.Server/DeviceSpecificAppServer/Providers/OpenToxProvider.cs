using DeviceSpecificAppServerService.Interfaces;
using DeviceSpecificAppServerService.Models;
using OpenTokSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceSpecificAppServerService.Providers
{
    public class OpenToxProvider : IOpenToxProvider
    {
        public OpenTok OpenTok { get; protected set; }

        public Session Session { get; protected set; }

        private int apiKey = 45842692;

        private string apiSecret = "f57c518980dc775f1aeff77f9fa94d96b9208423";

        public OpenToxProvider()
        {
            OpenTok = new OpenTok(apiKey, apiSecret);
            Session = OpenTok.CreateSession();
        }

        public SessionInfo GetSession()
        {
            double inOneWeek = (DateTime.UtcNow.Add(TimeSpan.FromDays(7)).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return new SessionInfo()
            {
                ApiKey = OpenTok.ApiKey,
                SessionId = Session.Id,
                Token = Session.GenerateToken(Role.MODERATOR, inOneWeek)
            };
        }
    }
}
