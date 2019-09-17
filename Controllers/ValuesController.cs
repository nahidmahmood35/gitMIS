using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HospitalManagementApp_Api.Controllers
{
    public class ValuesController : ApiController
    {
       

        public IEnumerable<string> GetNameOfRakib()
        {
            return new string[] { "Rakib", "Hasan" };
        }

        public IEnumerable<string> GetName()
        {
            return new string[] { "Get", "Name" };
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
