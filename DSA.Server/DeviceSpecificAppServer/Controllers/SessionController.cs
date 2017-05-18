using DeviceSpecificAppServerService.Interfaces;
using DeviceSpecificAppServerService.Providers;
using Microsoft.Azure.Mobile.Server.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OpenToxWebServerApi.Controllers
{
    [MobileAppController]
    [Route("/api/session")]
    public class SessionController : ApiController
    {
        private readonly IOpenToxProvider provider;

        public SessionController()
        {
            provider = new OpenToxProvider();
        }

        [HttpGet]
        // GET api/values
        public IHttpActionResult Get()
        {
            var session = provider.GetSession();
            return this.Json(session);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
