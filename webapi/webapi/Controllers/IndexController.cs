using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace webapi.Controllers
{
    public class IndexController : ApiController
    {
        [HttpPost]
        public IHttpActionResult login(loginformation log)
        {
            using(var db = new oucfreetalkEntities())
            {
                var stu = db.students.FirstOrDefault(a=> a.id == log.account);
                return Ok(stu);
            }
        }

        //[HttpPost]
        //public IHttpActionResult register(students reg)
        //{
        //    using (var db = new oucfreetalkEntities())
        //    {

        //    }
        //    return Ok();
        //}
    }
    //public class loginformation
    //{
    //    public string account { get; set; }
    //    public string password { get; set; }
    //    public string fuck { get; set; }
    //}
}
