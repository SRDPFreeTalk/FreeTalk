using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace webapi.Controllers
{
    public class LogController : ApiController
    {
        [HttpPost]
        [ActionName("Login")]
        public IHttpActionResult login(loginformation log)
        {
            using (var db = new oucfreetalkEntities())
            {
                var stu = db.students.FirstOrDefault(a => a.id == log.account);
                results res = new results();
                if(stu == null)
                {
                    res.result = 0;//登录失败，没有用户名
                    return Ok(res);
                }
                else
                {
                    if (PasswordHash.PasswordHash.ValidatePassword(log.password, stu.password))
                    {
                        HttpContext.Current.Session["sid"] = stu.id;
                        res.result = 1;//登录成功
                        return Ok(res);
                    }
                    else
                    {
                        res.result = 2;//密码错误
                        return Ok(res);
                    }
                }
            }
        }

        [HttpGet]
        [ActionName("Logout")]
        public IHttpActionResult logout(loginformation log)
        {
            HttpContext.Current.Session.Clear();
            results res = new results();
            res.result = 1;
            return Ok(res);
        }

        private bool IfExist(string id)
        {
            using (var db = new oucfreetalkEntities())
            {
                var se = db.students.FirstOrDefault(a => a.id == id);
                if (se == null)
                    return false;
                else return true;
            }
        }
    }

    public class loginformation
    {
        public string account { get; set; }
        public string password { get; set; }
    }

}
