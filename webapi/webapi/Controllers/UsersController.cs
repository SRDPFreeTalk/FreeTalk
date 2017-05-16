using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace webapi.Controllers
{
    public class UsersController : ApiController
    {
        public class registerdata
        {
            public string id { get; set; }
            public string password { get; set; }
            public string nikename { get; set; }
            public bool sex { get; set; }
            public string introduce { get; set; }
        }
        //注册
        [HttpPost]
        [ActionName("Register")]
        public IHttpActionResult register(registerdata rgd)
        {
            results res = new results();
            if (IfExist(rgd.id))
            {
                res.result = 2;
                return Ok(res);
            }

            try
            {
                using (var db = new oucfreetalkEntities())
                {
                    students std = new students();
                    std.id = rgd.id;
                    std.nikename = rgd.nikename;
                    std.sex = rgd.sex;
                    std.introduction = rgd.introduce;
                    std.name = " ";
                    std.birth = DateTime.Today;
                    std.year = DateTime.Today.Year.ToString();
                    std.ifsex = false;
                    std.exp = 0;
                    std.ifemail = false;
                    std.ifmobile = false;
                    std.ifname = false;
                    std.ifbirth = false;
                    std.password = PasswordHash.PasswordHash.CreateHash(rgd.password);
                    db.students.Add(std);
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 0;
                        return Ok(res);
                    }
                    else
                    {
                        res.result = 1;
                        return Ok(res);
                    }
                }
            }
            catch
            {
                //return NotFound();\
                res.result = 0;
                return Ok(res);
            }
        }


        //修改个人信息
        [HttpPost]
        [ActionName("Editor")]
        public IHttpActionResult eidtor(students stu)
        {
            try
            {
                string sid = HttpContext.Current.Session["sid"].ToString();
                if (sid == stu.id)
                {
                    using (var db = new oucfreetalkEntities())
                    {
                        var s = db.students.FirstOrDefault(a => a.id == stu.id);
                        if (s == null)
                        {
                            results res = new results();
                            res.result = 3;//没有此用户
                            return Ok(res);
                        }
                        else
                        {
                            s.nikename = stu.nikename;
                            s.sex = stu.sex;
                            s.birth = stu.birth;
                            s.year = stu.year;
                            s.family = stu.family;
                            s.pic = stu.pic;
                            s.ifname = stu.ifname;
                            s.ifsex = stu.ifsex;
                            s.ifbirth = stu.ifbirth;
                            s.ifmobile = stu.ifmobile;
                            s.ifemail = stu.ifemail;
                            if (db.SaveChanges() != 0)
                            {
                                results res = new results();
                                res.result = 1;//成功
                                return Ok(res);
                            }
                            else
                            {
                                results res = new results();
                                res.result = 4;//保存失败
                                return Ok(res);
                            }
                        }
                    }
                }
                else
                {
                    results res = new results();
                    res.result = 2; //id不合法
                    return Ok(res);
                }
            }
            catch
            {
                results res = new results();
                res.result = 0; //还未登录
                return Ok(res);
            }
        }


        //获取个人信息
        [HttpGet]
        [ActionName("Myinformation")]
        public IHttpActionResult Getmy()
        {
            results res = new results();
            try
            {
                string sid = HttpContext.Current.Session["sid"].ToString();
                using (var db = new oucfreetalkEntities())
                {
                    var stu = db.students.FirstOrDefault(a => a.id == sid);
                    if (stu != null)
                    {
                        reStudent st = new reStudent();
                        st.id = stu.id;
                        st.name = stu.name;
                        st.nikename = stu.nikename;
                        st.pic = stu.pic;
                        st.ifsex = stu.ifsex;
                        st.year = stu.year;
                        st.ifemail = stu.ifemail;
                        st.ifmobile = stu.ifmobile;
                        st.exp = stu.exp;
                        st.family = stu.family;
                        st.ifbirth = stu.ifbirth;
                        st.email = stu.email;
                        st.ifname = stu.ifname;
                        st.mobile = stu.mobile;
                        st.birth = stu.birth;
                        st.sex = stu.sex;
                        return Ok(st);
                    }
                    else
                    {
                        res.result = 4;
                        return Ok(res);
                    }
                }
            }
            catch
            {
                res.result = 0;
                return Ok(res);
            }
        }

        //获取指定id的用户信息

        [HttpGet]
        [ActionName("Othersinformation")]
        public IHttpActionResult GetOhters(string id)
        {
            results res = new results();
            using (var db = new oucfreetalkEntities())
            {
                var stu = db.students.FirstOrDefault(a => a.id == id);
                if (stu != null)
                {
                    reStudent st = new reStudent();
                    st.id = stu.id;
                    st.ifname = stu.ifname;
                    st.nikename = stu.nikename;
                    st.pic = stu.pic;
                    st.ifsex = stu.ifsex;
                    st.year = stu.year;
                    st.ifemail = stu.ifemail;
                    st.ifmobile = stu.ifmobile;
                    st.exp = stu.exp;
                    st.family = stu.family;
                    st.ifbirth = stu.ifbirth;
                    if (stu.ifemail)
                    {
                        st.email = stu.email;
                    }
                    if (stu.ifname)
                    {
                        st.name = stu.name;
                    }
                    if (stu.ifmobile)
                    {
                        st.mobile = stu.mobile;
                    }
                    if (stu.ifbirth)
                    {
                        st.birth = stu.birth;
                    }
                    if (stu.ifsex)
                    {
                        st.sex = stu.sex;
                    }
                    return Ok(st);
                }
                else
                {
                    res.result = 0;
                    return Ok(res);
                }
            }
        }

        public class results
        {
            public int result { get; set; }
        }

        public class reStudent
        {
            public string id { get; set; }
            public string name { get; set; }
            public bool ifname { get; set; }
            public string nikename { get; set; }
            public string pic { get; set; }
            public bool sex { get; set; }
            public bool ifsex { get; set; }
            public System.DateTime birth { get; set; }
            public string year { get; set; }
            public string email { get; set; }
            public bool ifemail { get; set; }
            public string mobile { get; set; }
            public bool ifmobile { get; set; }
            public int exp { get; set; }
            public string family { get; set; }
            public bool ifbirth { get; set; }
            public int accountlevel { get; set; }
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
}
