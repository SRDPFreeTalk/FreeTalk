using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace webapi.Controllers
{
    public class FriendsController : ApiController
    {
        [HttpGet]
        [ActionName("MyFocus")]
        public IHttpActionResult GetMyFocus()
        {
            results res = new results();
            try
            {
                string sid = HttpContext.Current.Session["sid"].ToString();
                using (var db = new oucfreetalkEntities())
                {
                    var stu = db.students.FirstOrDefault(a => a.id == sid);
                    if(stu == null)
                    {
                        res.result = 2;
                        return Ok(res);
                    }
                    var information = (from it in db.friendfocus
                                      where it.focus == stu.id
                                      select it).ToList();
                    if (information.Count == 0)
                    {
                        res.result = 3;
                        return Ok(res); //返回正确但是没有数据
                    }
                    for(int i = 0; i < information.Count; i++)
                    {
                        if (!information[i].ifname)
                        {
                            information[i].name = null;
                        }
                    }
                    return Ok(information);
                }
            }
            catch
            {
                res.result = 0;
                return Ok(res);
            }
        }

        [HttpGet]
        [ActionName("FocusMe")]
        public IHttpActionResult GetFocusMe()
        {
            results res = new results();
            try
            {
                string sid = HttpContext.Current.Session["sid"].ToString();
                using (var db = new oucfreetalkEntities())
                {
                    var stu = db.students.FirstOrDefault(a => a.id == sid);
                    if (stu == null)
                    {
                        res.result = 2;
                        return Ok(res);
                    }
                    var information = (from it in db.friendfocus
                                       where it.befocus == stu.id
                                       select it).ToList();
                    if (information.Count == 0)
                    {
                        res.result = 3;
                        return Ok(res); //返回正确但是没有数据
                    }
                    for (int i = 0; i < information.Count; i++)
                    {
                        if (!information[i].ifname)
                        {
                            information[i].name = null;
                        }
                    }
                    return Ok(information);
                }
            }
            catch
            {
                res.result = 0;
                return Ok(res);
            }
        }

        [HttpPost]
        [ActionName("YouMyFriend")]
        public IHttpActionResult AddFriend(mytarget ta)
        {
            string target = ta.target;
            results res = new results() ;
            string userid = "";
            try
            {
                userid = HttpContext.Current.Session["sid"].ToString();
                if (userid == "")
                {
                    res.result = 0;
                    return Ok(res);
                }
            }
            catch
            {
                res.result = 0;
                return Ok(res);
            }//未登录

            if (userid == target)
            {
                res.result = 5;//同名错误
                return Ok(res);
            }

            try
            {
                using(var db = new oucfreetalkEntities())
                {
                    var search = (from it in db.friends
                                  where it.focus == userid && it.befocus == target
                                  select it).ToList();
                    if (search.Count != 0)
                    {
                        res.result = 2;//已存在
                        return Ok(res);
                    }
                    else
                    {
                        friends fd = new friends();
                        fd.focus = userid;
                        fd.befocus = target;
                        fd.createtime = DateTime.Now;
                        db.friends.Add(fd);
                        if (db.SaveChanges()==0)
                        {
                            res.result = 3;//失败
                            return Ok(res);
                        }else
                        {
                            res.result = 1;//成功
                            return Ok(res);
                        }
                    }

                }
            }
            catch
            {
                res.result = 3;//服务器错误
                return Ok(res);

            }
            
            
        }

        [HttpPost]
        [ActionName("YouNotFriend")]
        public IHttpActionResult DeleteFriend(mytarget ta)
        {
            string target = ta.target;
            results res = new results();
            string userid = "";
            try
            {
                userid = HttpContext.Current.Session["sid"].ToString();
                if (userid == "")
                {
                    res.result = 0;
                    return Ok(res);
                }
            }
            catch
            {
                res.result = 0;
                return Ok(res);
            }//未登录

            if (userid == target)
            {
                res.result = 5;//同名错误
                return Ok(res);
            }

            try
            {
                using (var db = new oucfreetalkEntities())
                {
                    var search = (from it in db.friends
                                  where it.focus == userid && it.befocus == target
                                  select it).ToList();
                    if (search.Count == 0)
                    {
                        res.result = 2;//不存在该好友
                        return Ok(res);
                    }
                    else
                    {
                        db.friends.Remove(search[0]);
                        if (db.SaveChanges() == 0)
                        {
                            res.result = 3;//失败
                            return Ok(res);
                        }
                        else
                        {
                            res.result = 1;//成功
                            return Ok(res);
                        }
                    }

                }
            }
            catch
            {
                res.result = 3;//服务器错误
                return Ok(res);

            }


        }


    }

    public class mytarget
    {
        public string target { get; set; }
    }

    

}

