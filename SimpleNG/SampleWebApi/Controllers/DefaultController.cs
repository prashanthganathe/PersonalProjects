using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SampleWebApi.Controllers
{

    public class Person
    {
       public string Name {get;set;} 
      public int Age{get;set;}
       public string City{get;set;}
       public DateTime Date{get;set;}
    }

    [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DefaultController : ApiController
    {
        
        // GET: api/Default
        public IEnumerable<Person> Get()
        {
           // return new string[] { "value1", "value2" };
            //
           List<Person> lstperson = new List<Person>();
           lstperson.Add(new Person { Name= "Name10", Age= 24, City= "NY", Date= new DateTime(2010,01,02)});
           lstperson.Add(new Person { Name = "Name20", Age = 25, City = "AY", Date = new DateTime(2011, 01, 02) });
           lstperson.Add(new Person { Name = "Name30", Age = 26, City = "BY", Date = new DateTime(2012, 01, 02) });
           lstperson.Add(new Person { Name = "Name40", Age = 27, City = "CY", Date = new DateTime(2013, 01, 02) });
           return lstperson;
                    //{ Name: 'Name2', Age: '22', City: 'AY', Date: '2013-2-21' },
                    //{ Name: 'Name3', Age: '23', City: 'BY', Date: '2014-2-21' },
                    //{ Name: 'Name4', Age: '25', City: 'CY', Date: '2010-2-21' }
        }

        // GET: api/Default/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Default
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }
    }
}
