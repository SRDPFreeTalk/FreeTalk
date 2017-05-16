using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webapi;

namespace MyApi
{
    public class SqlHelper
    {
         /*
         //是否登录
         //
         */
        public string IfLogin()
        {
            try
            {
                string stuid = HttpContext.Current.Session["sid"].ToString();
                using (var db = new oucfreetalkEntities())
                {
                    var re = db.students.FirstOrDefault(a => a.id == stuid);
                    if (re == null)
                    {
                        return "";
                    }
                    else
                    {
                        return stuid;
                    }
                }
            }
            catch
            {
                return "";
            }
        }


        /*
         //检查是板块是否存在
         //
         */
        public bool IfClassExist(string classname)
        {
            using(var db = new oucfreetalkEntities())
            {
                var search = (from it in db.postclass
                              where it.name == classname && it.state == true
                              select it).ToList();
                return (search.Count == 0) ? false : true;
            }
        }

        /*
         //检查是否拥有权限
         //
         */
         public bool IfYouHaveAcess(int access,string StuId)
        {
            using(var db = new oucfreetalkEntities())
            {
                var search = (from it in db.accountaccess
                              where it.studentid == StuId && it.classid == access
                              select it).ToList();
                return (search.Count == 0) ? false : true;
            }
        }

    }
}