using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IMSServer.Controllers
{
    [AllowAnonymous]
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new [] { "value 1", "value 2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return $"value {id}";
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
