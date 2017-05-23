using DeviceSpecificAppServerService.Context;
using DeviceSpecificAppServerService.DataObjects;
using DeviceSpecificAppServerService.Models;
using DeviseSpecificAppServer.Interfaces;
using DeviseSpecificAppServer.Models;
using Microsoft.Azure.Mobile.Server.Login;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DeviceSpecificAppServerService.Controllers
{
    [Route(".auth/login/custom")]
    public class AuthController : ApiController
    {
        private DeviceSpecificAppServerContext context;
        private INotificationsProvider notificationsProvider;
        private string signingKey, audience, issuer;

        public AuthController(DeviceSpecificAppServerContext context,
                                INotificationsProvider notificationsProvider)
        {
            this.context = context;
            this.notificationsProvider = notificationsProvider;
            signingKey = Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");
            var website = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
            audience = $"https://{website}/";
            issuer = $"https://{website}/";
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] UserModel body)
        {
            if (body == null || body.Email == null || body.Password == null ||
                body.Email.Length == 0 || body.Password.Length == 0)
            {
                return BadRequest();
            }

            if (!IsValidUser(body))
            {
                return Unauthorized();
            }

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, body.Email)
            };

            JwtSecurityToken token = AppServiceLoginHandler.CreateToken(
                claims, signingKey, audience, issuer, TimeSpan.FromDays(30));

            this.notificationsProvider.AddTagToRegistration(body.Installation, body.Email);

            return Ok(new LoginResult()
            {
                AuthenticationToken = token.RawData,
                User = new LoginResultUser { UserId = body.Email }
            });
        }

        private bool IsValidUser(UserModel user)
        {
            return context.Users.Count(u => u.Email.Equals(user.Email) && u.Password.Equals(user.Password)) > 0;
            //return db.Users.Count() > 0;
        }
    }
}