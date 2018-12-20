using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimeRegistration.Models;

namespace TimeRegistration.Controllers
{
    public class ValuesController : ApiController
    {
        private TimeRegistrationEntities db = new TimeRegistrationEntities();

        // GET api/values
        public IEnumerable<Employee> Get()
        {
            return db.Employees.ToList();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]Employee employee)
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
