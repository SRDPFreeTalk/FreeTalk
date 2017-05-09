using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace webapi.Controllers
{
    public class PostsController : ApiController
    {


        public class PostMainInfo
        {
            public string title { get; set; }
            public string context { get; set; }
            public int pclass { get; set; }

        }
        [HttpPost]
        [ActionName("addPost")]
        public IHttpActionResult addPost(PostMainInfo pmi)
        {
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
            }
            try
            {
                using (var db = new oucfreetalkEntities())
                {
                    posts pts = new posts();
                    DateTime nowtime = DateTime.Now;
                    pts.ownclass = pmi.pclass;
                    pts.title = pmi.title;
                    pts.contenttext = pmi.context;
                    pts.realbody = 1;
                    pts.body = 1;
                    pts.owner = userid;
                    pts.createtime = nowtime;
                    pts.updatetime = nowtime;
                    db.posts.Add(pts);
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 2;
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
                res.result = 3;
                return Ok(res);
            }
        }

        public class CommentMainInfo
        {
            public string context { get; set; }
            public int postid { get; set; }
        }
        [HttpPost]
        [ActionName("addcomments")]
        public IHttpActionResult addcomment(CommentMainInfo cmi)
        {
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
            }

            try
            {
                using (var db = new oucfreetalkEntities())
                {
                    var seachpost = (from it in db.posts
                                     where it.id == cmi.postid
                                     select it).ToList();

                    if (seachpost.Count == 0)
                    {
                        res.result = 4;
                        return Ok(res);
                    }

                    var thisPost = db.posts.FirstOrDefault(a => a.id == cmi.postid);
                    postc pc = new postc();
                    pc.owner = userid;
                    pc.ownpost = thisPost.id;
                    pc.body = cmi.context;
                    pc.createtime = DateTime.Now;
                    pc.postlocation = thisPost.body + 1;
                    db.postc.Add(pc);
                    thisPost.realbody += 1;
                    thisPost.body += 1;
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 2;
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
                res.result = 3;
                return Ok(res);
            }

        }

        public class ReqPost
        {
            public int pclass { get; set; }
            public int perpage { get; set; }
            public int index { get; set; }
        }

        [HttpGet]
        [ActionName("getPosts")]
        public IHttpActionResult getPosts(ReqPost rp)
        {
            results res = new results();
            if (rp.perpage <= 1) rp.perpage = 5;
            if (rp.index <= 0) rp.index = 1;
            using (var db = new oucfreetalkEntities())
            {
                if (rp.pclass != -1 && db.postc.FirstOrDefault(a => a.id == rp.pclass) == null)
                {
                    res.result = 0;
                    return Ok(res);
                }

                if (rp.pclass == -1)
                {
                    var search = (from it in db.posts
                                  join itb in db.students on it.owner equals itb.id
                                  orderby it.updatetime descending
                                  select new
                                  {
                                      it.id,
                                      it.ownclass,
                                      it.owner,
                                      it.realbody,
                                      it.title,
                                      it.contenttext,
                                      it.body,
                                      it.createtime,
                                      ownername = itb.nikename,
                                      ownerpic = itb.pic
                                  }).Skip((rp.index - 1) * rp.perpage).Take(rp.perpage);

                    return Ok(search);


                }
                else
                {
                    var search = (from it in db.posts
                                  join itb in db.students on it.owner equals itb.id
                                  where it.ownclass == rp.pclass
                                  orderby it.updatetime descending
                                  select new
                                  {
                                      it.id,
                                      it.ownclass,
                                      it.owner,
                                      it.realbody,
                                      it.title,
                                      it.contenttext,
                                      it.body,
                                      it.createtime,
                                      ownername = itb.nikename,
                                      ownerpic = itb.pic
                                  }).Skip((rp.index - 1) * rp.perpage).Take(rp.perpage);

                    return Ok(search);
                }
            }
        }


        public class ReqPostPage
        {
            public int pclass { get; set; }
            public int perpage { get; set; }
        }
        [HttpGet]
        [ActionName("getPostsPage")]
        public IHttpActionResult getPostsPage(ReqPostPage rp)
        {
            results res = new results();
            if (rp.perpage <= 1) rp.perpage = 5;
            using (var db = new oucfreetalkEntities())
            {
                if (rp.pclass != -1 && db.postc.FirstOrDefault(a => a.id == rp.pclass) == null)
                {
                    res.result = 0;
                    return Ok(res);
                }
                int allcount = 0;
                if (rp.pclass == -1)
                {
                    var search = (from it in db.posts
                                  select it).ToList();
                    allcount = search.Count;



                }
                else
                {
                    var search = (from it in db.posts
                                  select it).ToList();

                    allcount = search.Count;
                }
                int allpage = allcount / rp.perpage;
                if (allcount % rp.perpage != 0)
                {
                    allpage++;
                }
                return Ok(new { allpage });
            }
        }


        public class AddClassData
        {
            public string classname { get; set; }
        }
        [HttpPost]
        [ActionName("addClass")]
        public IHttpActionResult addClass(AddClassData acd)
        {
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
                res.result = 0;//未登录
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                var searchaccess = (from it in db.accountaccess
                                    where it.classid == -1
                                    select it).ToList();
                if (searchaccess.Count == 0)
                {
                    res.result = 2;//权限不够
                    return Ok(res);
                }
                var searchName = (from it in db.postclass
                                  where it.name == acd.classname
                                  select it).ToList();
                if (searchName.Count != 0)
                {
                    res.result = 4;//该板块已存在
                    return Ok(res);
                }
                try
                {
                    postclass pcs = new postclass();
                    pcs.name = acd.classname;
                    db.postclass.Add(pcs);
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 5;//服务器错误
                        return Ok(res);
                    }
                    else
                    {
                        res.result = 1;//添加成功
                        return Ok(res);
                    }
                }
                catch
                {
                    res.result = 3;//服务器错误
                    return Ok(res);
                }
            }
        }


        public class editorClassData
        {
            public int classid { get; set; }
            public string newname { get; set; }
        }
        [HttpPost]
        [ActionName("editorClass")]
        public IHttpActionResult editorClass(editorClassData ecd)
        {
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
                res.result = 0;//未登录
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                var searchaccess = (from it in db.accountaccess
                                    where it.classid == -1
                                    select it).ToList();
                if (searchaccess.Count == 0)
                {
                    res.result = 2;//权限不够
                    return Ok(res);
                }
                var searchName = (from it in db.postclass
                                  where it.id == ecd.classid
                                  select it).ToList();
                if (searchName.Count == 0)
                {
                    res.result = 4;//该板块不存在
                    return Ok(res);
                }
                try
                {
                    postclass ptc = db.postclass.FirstOrDefault(a => a.id == ecd.classid);
                    ptc.name = ecd.newname;
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 5;//服务器错误
                        return Ok(res);
                    }
                    else
                    {
                        res.result = 1;//修改成功
                        return Ok(res);
                    }
                }
                catch
                {
                    res.result = 3;//服务器错误
                    return Ok(res);
                }
            }
        }


        public class removeClassData
        {
            public int classid { get; set; }
        }
        [HttpPost]
        [ActionName("deleteClass")]
        public IHttpActionResult deleteClass(removeClassData rcd)
        {
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
                res.result = 0;//未登录
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                var searchaccess = (from it in db.accountaccess
                                    where it.classid == -1
                                    select it).ToList();
                if (searchaccess.Count == 0)
                {
                    res.result = 2;//权限不够
                    return Ok(res);
                }
                var searchName = (from it in db.postclass
                                  where it.id == rcd.classid
                                  select it).ToList();
                if (searchName.Count == 0)
                {
                    res.result = 4;//该板块不存在
                    return Ok(res);
                }
                try
                {
                    postclass ptc = db.postclass.FirstOrDefault(a => a.id == rcd.classid);//删除
                    var search_post = (from it in db.posts
                                       where it.postclass == ptc
                                       select it).ToList();//查找该板块的帖子
                    for(int i = 0; i < search_post.Count; i++)
                    {
                        var search_comm = (from it in db.postc
                                           where it.ownpost == search_post[i].id
                                           select it).ToList();//查找帖子的楼层
                        for(int j = 0; j < search_comm.Count; j++)
                        {
                            var search_reply = (from it in db.postreply
                                                where it.ownlocation == search_comm[i].id
                                                select it).ToList();
                            for(int ij = 0; ij < search_reply.Count; ij++)
                            {
                                db.postreply.Remove(search_reply[ij]);//循环删除回复
                            }
                            db.postc.Remove(search_comm[j]);//循环删除楼层
                        }
                        db.posts.Remove(search_post[i]);//循环删除帖子
                    }
                    var search_access = (from it in db.accountaccess
                                         where it.classid == rcd.classid
                                         select it).ToList();
                    for(int i = 0; i < search_access.Count; i++)
                    {
                        db.accountaccess.Remove(search_access[i]);//循环删除版主
                    }
                    db.postclass.Remove(ptc);//删除板块
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 5;//服务器错误
                        return Ok(res);
                    }
                    else
                    {
                        res.result = 1;//成功
                        return Ok(res);
                    }
                }
                catch
                {
                    res.result = 3;//服务器错误
                    return Ok(res);
                }
            }
        }

        [HttpGet]
        [ActionName("getClasses")]
        public IHttpActionResult getClasses()
        {
            results res = new results();
            try
            {
                using(var db = new oucfreetalkEntities())
                {
                    var classes = (from it in db.postclass
                                   select it).ToList();
                    if (classes.Count == 0)
                    {
                        res.result = 0;
                        return Ok(res);
                    }
                    return Ok(classes);
                }
            }
            catch
            {
                res.result = 3;
                return Ok(res);
            }
        }






    }


}
