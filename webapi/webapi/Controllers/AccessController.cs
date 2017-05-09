using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace webapi.Controllers
{
    public class AccessController : ApiController
    {
        [HttpGet]
        [ActionName("IAmMaster")]
        public IHttpActionResult getGod()
        {
            results res = new results();
            string userid = "";
            try
            {
                userid = HttpContext.Current.Session["sid"].ToString();
                if (userid == "")
                {
                    res.result = 0;//未登录
                    return Ok(res);
                }
            }
            catch
            {
                res.result = 0;//未登录
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                var search = (from it in db.accountaccess
                              where it.studentid == userid && it.classid == -2
                              select it).ToList();//确认是否为狗管理
                if (search.Count != 0)
                {
                    res.result = 2;//你已经是狗管理了
                    return Ok(res);
                }
                else
                {
                    accountaccess ata = new accountaccess();
                    ata.studentid = userid;
                    ata.createtime = DateTime.Now;
                    ata.classid = -2;
                    try
                    {
                        db.accountaccess.Add(ata);
                        if (db.SaveChanges() == 0)
                        {
                            res.result = 4;//服务器错误
                            return Ok(res);
                        }
                        res.result = 1;//授予成功
                        return Ok(res);
                    }
                    catch
                    {
                        res.result = 3;//服务器错误
                        return Ok(res);
                    }
                }
            }
        }

        public class setaccessdata
        {
            public string stuid { get; set; }
            public int accessclass { get; set; }
        }
        [HttpPost]
        [ActionName("GiveYou")]
        public IHttpActionResult setAccess(setaccessdata sad)
        {
            results res = new results();
            string userid = "";
            try
            {
                userid = HttpContext.Current.Session["sid"].ToString();
                if (userid == "")
                {
                    res.result = 0;//未登录
                    return Ok(res);
                }
            }
            catch
            {
                res.result = 0;//未登录
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                var search = (from it in db.accountaccess
                              where it.studentid == userid && it.classid == -2
                              select it).ToList();
                if (search.Count == 0)
                {
                    res.result = 2;//不是管理员的管理员
                    return Ok(res);
                }
                if (sad.accessclass != -1)
                {
                    var search_access = (from it in db.postclass
                                         where it.id == sad.accessclass
                                         select it).ToList();
                    if (search_access.Count == 0)
                    {
                        res.result = 5;//权限不存在
                        return Ok(res);
                    }
                }
                var search_stu_access = (from it in db.accountaccess
                                         where it.studentid == sad.stuid && (it.classid == sad.accessclass || it.classid == -1)
                                         select it).ToList();
                if (search_stu_access.Count != 0)
                {
                    res.result = 6;//已有权限或者更高权限
                    return Ok(res);
                }

                try
                {
                    accountaccess ata = new accountaccess();
                    ata.studentid = userid;
                    ata.createtime = DateTime.Now;
                    ata.classid = sad.accessclass;
                    db.accountaccess.Add(ata);
                    if (sad.accessclass == -1)//如果添加的是板块总管理员,删除其他版主身份
                    {
                        for(int i = 0; i < search_stu_access.Count; i++)
                        {
                            if (search_stu_access[i].classid != -2)
                            {
                                db.accountaccess.Remove(search_stu_access[i]);
                            }
                        }
                    }
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 4;//服务器错误
                        return Ok(res);
                    }
                    res.result = 1;
                    return Ok(res);
                }
                catch
                {
                    res.result = 3;
                    return Ok(res);
                }

            }
        }


        public class delaccessdata
        {
            public string stuid { get; set; }
            public int accessclass { get; set; }
        }
        [HttpPost]
        [ActionName("ReturnToMe")]
        public IHttpActionResult delAccess(delaccessdata sad)
        {
            results res = new results();
            string userid = "";
            try
            {
                userid = HttpContext.Current.Session["sid"].ToString();
                if (userid == "")
                {
                    res.result = 0;//未登录
                    return Ok(res);
                }
            }
            catch
            {
                res.result = 0;//未登录
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                var search = (from it in db.accountaccess
                              where it.studentid == userid && it.classid == -2
                              select it).ToList();
                if (search.Count == 0)
                {
                    res.result = 2;//不是管理员的管理员
                    return Ok(res);
                }
                if (sad.accessclass != -1)
                {
                    var search_access = (from it in db.postclass
                                         where it.id == sad.accessclass
                                         select it).ToList();
                    if (search_access.Count == 0)
                    {
                        res.result = 5;//权限不存在
                        return Ok(res);
                    }
                }
                var search_stu_access = (from it in db.accountaccess
                                         where it.studentid == sad.stuid && it.classid == sad.accessclass
                                         select it).ToList();
                if (search_stu_access.Count == 0)
                {
                    res.result = 6;//根本没有该权限
                    return Ok(res);
                }
                try
                {
                    db.accountaccess.Remove(search_stu_access[0]);
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 4;//服务器错误
                        return Ok(res);
                    }
                    res.result = 1;
                    return Ok(res);
                }
                catch
                {
                    res.result = 3;
                    return Ok(res);
                }

            }
        }

        [HttpGet]
        [ActionName("SeeMyAccess")]
        public IHttpActionResult getMyAccess()
        {
            results res = new results();
            string userid = "";
            try
            {
                userid = HttpContext.Current.Session["sid"].ToString();
                if (userid == "")
                {
                    res.result = 0;//未登录
                    return Ok(res);
                }
            }
            catch
            {
                res.result = 0;//未登录
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                var search = (from it in db.accountaccess
                              where it.studentid == userid
                              select new { it.id,it.studentid,it.classid,it.createtime}).ToList();//查看我的管理列表
                if (search.Count == 0)
                {
                    res.result = 1;//没有任何权限
                    return Ok(res);
                }
                else return Ok(search);
            }
        }


        public class getAccessData
        {
            public int access { get; set; }
        } 
        [HttpGet]
        [ActionName("SeeAccess")]
        public IHttpActionResult getAccess(getAccessData gad)
        {
            results res = new results();
            string userid = "";
            try
            {
                userid = HttpContext.Current.Session["sid"].ToString();
                if (userid == "")
                {
                    res.result = 0;//未登录
                    return Ok(res);
                }
            }
            catch
            {
                res.result = 0;//未登录
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                var search = (from it in db.accountaccess
                              where it.studentid == userid && it.classid==-2
                              select it).ToList();//查看我的是否是管理员
                if (search.Count == 0)
                {
                    res.result = 2;//你不是管理员再见
                    return Ok(res);
                }

                if (gad.access == -3)
                {
                    var s_data = (from it in db.accountaccess
                                  select it).ToList();
                    if (s_data.Count == 0)
                    {
                        res.result = 1;
                        return Ok(res);
                    }
                    return Ok(s_data);//返回数据
                }
                else
                {
                    var s_data = (from it in db.accountaccess
                                  where it.classid==gad.access
                                  select it).ToList();
                    if (s_data.Count == 0)
                    {
                        res.result = 1;
                        return Ok(res);
                    }
                    return Ok(s_data);//返回数据
                }
            }
        }
    }
}
