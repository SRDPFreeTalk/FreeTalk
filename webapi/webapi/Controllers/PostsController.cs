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
            string userid = new MyApi.SqlHelper().IfLogin();

            if (userid == "")
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
                    pts.state = true;
                    db.posts.Add(pts);
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 2;
                        return Ok(res);
                    }
                    else
                    {
                        buridata.addbridata(userid,1,0);
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

            string userid = new MyApi.SqlHelper().IfLogin();

            if (userid == "")
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
                    pc.state = true;
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
                        //添加提醒
                        var rst = db.comment.FirstOrDefault(a => a.owner == pc.owner && a.createtime == pc.createtime && a.body == pc.body);
                        if(!new MessageHelper().addcommentmessage(rst.id))
                        {
                            res.result = 6;//消息未创建成功
                            return Ok(res);
                        }
                        buridata.addbridata(userid, 2, 0);
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

        public class ReplyMainInfo
        {
            public string context { get; set; }
            public int commentid { get; set; }
            public string replyid { get; set; }
        }
        [HttpPost]
        [ActionName("addreply")]
        public IHttpActionResult addreply(ReplyMainInfo rmi)
        {
            results res = new results();

            string userid = new MyApi.SqlHelper().IfLogin();

            if (userid == "")
            {
                res.result = 0;
                return Ok(res);
            }

            try
            {
                using (var db = new oucfreetalkEntities())
                {
                    var thisPost = db.postc.FirstOrDefault(a => a.id == rmi.commentid);
                    if (thisPost == null)
                    {
                        res.result = 4;
                        return Ok(res);
                    }
                    var rootpost = db.posts.FirstOrDefault(a => a.id == thisPost.ownpost);
                    postreply pc = new postreply();
                    pc.owner = userid;
                    pc.ownlocation = thisPost.id;
                    pc.createtime = DateTime.Now;
                    pc.replyto = rmi.replyid;
                    pc.state = true;
                    pc.contenttext = rmi.context;
                    db.postreply.Add(pc);
                    rootpost.realbody += 1;
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 2;
                        return Ok(res);
                    }
                    else
                    {
                        var rst = db.postreply.FirstOrDefault(a=>a.owner==pc.owner&&a.createtime==pc.createtime&&a.ownlocation==pc.ownlocation);
                        if(!new MessageHelper().addreplymessage(rst.id))
                        {
                            res.result = 6;//消息未创建成功
                            return Ok(res);
                        }
                        buridata.addbridata(userid, 3, 0);
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
            public int index { get; set; }
        }

        [HttpGet]
        [ActionName("getPosts")]
        public IHttpActionResult getPosts(ReqPost rp)
        {
            results res = new results();
            int perpage = 20;
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
                    var search_post = (from it in db.posts
                                       join itb in db.students on it.owner equals itb.id
                                       where it.state == true
                                       orderby it.updatetime descending
                                       select new
                                       {
                                           it.id,
                                           it.ownclass,
                                           it.owner,
                                           it.realbody,
                                           it.title,
                                           it.contenttext,
                                           it.createtime,
                                           ownername = itb.nikename,
                                           ownerpic = itb.pic
                                       }).ToList();
                    int allcount = search_post.Count;
                    int allpage = allcount / perpage;
                    if (allcount % perpage != 0)
                    {
                        allpage++;
                    }
                    var search=search_post.Skip((rp.index - 1) * perpage).Take(perpage);

                    return Ok(new { search ,allpage});


                }
                else
                {
                    var search_post = (from it in db.posts
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
                                       }).ToList();
                    int allcount = search_post.Count;
                    int allpage = allcount / perpage;
                    if (allcount % perpage != 0)
                    {
                        allpage++;
                    }
                    var search = search_post.Skip((rp.index - 1) * perpage).Take(perpage);

                    return Ok(new { search, allpage });
                }
            }
        }


        public class ReqPostData
        {
            public int postid { get; set; }
            public int index { get; set; }
        }
        [HttpGet]
        [ActionName("getPost")]
        public IHttpActionResult getPostindex(ReqPostData rpd)
        {
            results res = new results();
            int perpage = 20;
            if (rpd.index <= 0) rpd.index = 1;
            using (var db = new oucfreetalkEntities())
            {
                var thispost = db.posts.FirstOrDefault(a=>a.id==rpd.postid);      
                if (thispost == null)
                {
                    res.result = 2;//post不存在
                    return Ok(res);
                }
                var thisstu = db.students.FirstOrDefault(a => a.id == thispost.owner);
                var searchcomment = (from it in db.postc
                                     join it2 in db.students on it.owner equals it2.id
                                     where it.ownpost == rpd.postid && it.state == true
                                     orderby it.createtime
                                     select new
                                     {
                                         commentid=it.id,
                                         commentcontext = it.body,
                                         it.createtime,
                                         it.postlocation,
                                         stuid=it2.id,
                                         ico=it2.pic,
                                         nikename=it2.nikename
                                     }).ToList();

                searchcomment.Add(new { commentid = 0, commentcontext = thispost.contenttext, thispost.createtime, postlocation = 1, stuid = thisstu.id, ico = thisstu.pic, thisstu.nikename });
                searchcomment.OrderBy(a => a.postlocation);
                int allcount = searchcomment.Count;
                int allpage = allcount / perpage;
                if (allcount % perpage != 0)
                {
                    allpage++;
                }
                var search = searchcomment.Skip((rpd.index - 1) * perpage).Take(perpage);

                return Ok(new { search, allpage });
            }
        }

        public class ReqCommentData
        {
            public int commentid { get; set; }
            public int index { get; set; }
        }
        [HttpGet]
        [ActionName("getPost")]
        public IHttpActionResult getCommentindex(ReqCommentData rcd)
        {
            results res = new results();
            int perpage = 20;
            if (rcd.index <= 0) rcd.index = 1;
            using (var db = new oucfreetalkEntities())
            {
                var thiscomment = db.posts.FirstOrDefault(a => a.id == rcd.commentid);

                if (thiscomment == null)
                {
                    res.result = 2;//comment不存在
                    return Ok(res);
                }
                var searchreply = (from it in db.postreply
                                   where it.ownlocation == thiscomment.id && it.state == true
                                   orderby it.createtime descending
                                   select it).ToList();
                int allcount = searchreply.Count;
                int allpage = allcount / perpage;
                if (allcount % perpage != 0)
                {
                    allpage++;
                }
                var search = searchreply.Skip((rcd.index - 1) * perpage).Take(perpage);

                return Ok(new { search, allpage });
            }
        }

        public class delPostData
        {
            public int postid { get; set; }
        }

        [HttpPost]
        [ActionName("delPost")]
        public IHttpActionResult delPostsd(delPostData dpd)
        {
            results res = new results();
            string userid = new MyApi.SqlHelper().IfLogin();//获取id
            if (userid == "")
            {
                res.result = 0;//未登录
                return Ok(res);
            }
            if(!new PostAcess().GetPostAccess(userid, dpd.postid))
            {
                res.result = 2;//权限不够
                return Ok(res);
            }
            using(var db = new oucfreetalkEntities())
            {
                var searchpost = db.posts.FirstOrDefault(a=>a.id==dpd.postid);
                if (searchpost == null)
                {
                    res.result = 4;//帖子不存在
                    return Ok(res);
                }
                searchpost.state = false;
                var search_comment = (from it in db.postc
                                      where it.ownpost == searchpost.id&&it.state==true
                                      select it).ToList();
                for(int i = 0; i < search_comment.Count; i++)
                {
                    var search_reply = (from it in db.postreply
                                        where it.ownlocation == search_comment[i].id && it.state == true
                                        select it).ToList();
                    for(int j = 0; j < search_reply.Count; j++)
                    {
                        search_reply[j].state = false;
                    }
                    search_comment[i].state = false;
                }
                try
                {
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 5;
                        return Ok(res);
                    }
                    else
                    {
                        res.result = 1;
                        return Ok(res);
                    }
                }
                catch
                {
                    res.result = 3;
                    return Ok(res);
                }
            }
        }

        public class delCommentData
        {
            public int commentid { get; set; }

        }
        [HttpPost]
        [ActionName("delComment")]
        public IHttpActionResult delComment(delCommentData dcd)
        {
            results res = new results();
            string userid = new MyApi.SqlHelper().IfLogin();//获取id
            if (userid == "")
            {
                res.result = 0;//未登录
                return Ok(res);
            }
            if (!new PostAcess().GetCommentAccess(userid, dcd.commentid))
            {
                res.result = 2;//权限不够
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                var searchcomment = db.postc.FirstOrDefault(a => a.ownpost == dcd.commentid);
                if (searchcomment == null)
                {
                    res.result = 4;
                    return Ok(res);
                }
                var searchpost = db.posts.FirstOrDefault(a => a.id == searchcomment.ownpost);

                searchcomment.state = false;
                searchpost.realbody -= 1;
                var search_reply = (from it in db.postreply
                                    where it.ownlocation == searchcomment.id && it.state == true
                                    select it).ToList();
                for(int i = 0; i < search_reply.Count; i++)
                {
                    search_reply[i].state = false;
                    searchpost.realbody -= 1;
                }

                try
                {
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 5;
                        return Ok(res);
                    }else
                    {
                        res.result = 1;
                        return Ok(res);
                    }
                }
                catch
                {
                    res.result = 3;
                    return Ok(res);
                }
            }
        }

        public class delReplyData
        {
            public int replyid { get; set; }

        }
        [HttpPost]
        [ActionName("delReply")]
        public IHttpActionResult delReply(delReplyData drd)
        {
            results res = new results();
            string userid = new MyApi.SqlHelper().IfLogin();//获取id
            if (userid == "")
            {
                res.result = 0;//未登录
                return Ok(res);
            }
            if (!new PostAcess().GetReplyAccess(userid, drd.replyid))
            {
                res.result = 2;//权限不够
                return Ok(res);
            }
            using(var db = new oucfreetalkEntities())
            {
                var searchreply = db.postreply.FirstOrDefault(a => a.id == drd.replyid);
                if (searchreply == null)
                {
                    res.result = 4;
                    return Ok(res);
                }
                var thiscomment = db.postc.FirstOrDefault(a => a.id == searchreply.ownlocation);
                var thispost = db.posts.FirstOrDefault(a => a.id == thiscomment.ownpost);
                thispost.realbody -= 1;
                searchreply.state = false;

                try
                {
                    if (db.SaveChanges() == 0)
                    {
                        res.result = 5;
                        return Ok(res);
                    }
                    else
                    {
                        res.result = 1;
                        return Ok(res);
                    }
                }
                catch
                {
                    res.result = 3;
                    return Ok(res);
                }
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
            string userid = new MyApi.SqlHelper().IfLogin();

            if (userid == "")
            {
                res.result = 0;//未登录
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                if (!new MyApi.SqlHelper().IfYouHaveAcess(-1,userid))
                {
                    res.result = 2;//权限不够
                    return Ok(res);
                }
                if (new MyApi.SqlHelper().IfClassExist(acd.classname))
                {
                    res.result = 4;//该板块已存在
                    return Ok(res);
                }
                try
                {
                    postclass pcs = new postclass();
                    pcs.name = acd.classname;
                    pcs.state = true;//未删除
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
            string userid = new MyApi.SqlHelper().IfLogin();

            if (userid == "")
            {
                res.result = 0;
                return Ok(res);
            }
            if (!new MyApi.SqlHelper().IfYouHaveAcess(-1, userid))
            {
                res.result = 2;//权限不够
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {

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
            string userid = new MyApi.SqlHelper().IfLogin();

            if (userid == "")
            {
                res.result = 0;
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
                    for (int i = 0; i < search_post.Count; i++)
                    {
                        var search_comm = (from it in db.postc
                                           where it.ownpost == search_post[i].id
                                           select it).ToList();//查找帖子的楼层
                        for (int j = 0; j < search_comm.Count; j++)
                        {
                            var search_reply = (from it in db.postreply
                                                where it.ownlocation == search_comm[i].id
                                                select it).ToList();
                            for (int ij = 0; ij < search_reply.Count; ij++)
                            {
                                search_reply[ij].state=false;//循环删除回复
                            }
                            search_comm[j].state=false;//循环删除楼层
                        }
                        search_post[i].state = false;//循环删除帖子
                    }
                    var search_access = (from it in db.accountaccess
                                         where it.classid == rcd.classid
                                         select it).ToList();
                    for (int i = 0; i < search_access.Count; i++)
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
                using (var db = new oucfreetalkEntities())
                {
                    var classes = (from it in db.postclass
                                   where it.state==true
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


        public class SearchData
        {
            public string searchtext { get; set; }
            public int index { get; set; }
        }

        public class SearchReturn
        {
            public int srtype { get; set; }

            //帖子部分
            public string postid;
            public string postname;
            public string commentsid;
            public string replytext;
            public DateTime createtime;
            //人物部分
            public string nikename;
            public string stuid;
            public string ico;
        }

        [HttpGet]
        [ActionName("SearchAll")]
        public IHttpActionResult SearchPost(SearchData sd)
        {
            int perpage = 20;
            List<SearchReturn> srd = new List<SearchReturn>();
            using (var db = new oucfreetalkEntities())
            {
                //查询主题
                var search_post = (from it in db.posts
                                   join it2 in db.students on it.owner equals it2.id
                                   where it.state == true && (it.title.Contains(sd.searchtext) || it.contenttext.Contains(sd.searchtext))
                                   select new
                                   {
                                       it.title,
                                       it.contenttext,
                                       it.id,
                                       it.updatetime,
                                       it2.nikename,
                                       stuid = it2.id
                                   }).ToList();
                for (int i = 0; i < search_post.Count; i++)
                {
                    SearchReturn srtemp = new SearchReturn();
                    srtemp.srtype = 2;//类型2帖子
                    srtemp.postid = search_post[i].id.ToString();//帖子id
                    srtemp.postname = search_post[i].title.ToString();//贴子标题
                    srtemp.createtime = search_post[i].updatetime;//帖子最后更新时间
                    srtemp.stuid = search_post[i].stuid;
                    srtemp.nikename = search_post[i].nikename;
                    srtemp.replytext = search_post[i].contenttext;
                    srd.Add(srtemp);
                }

                //搜索楼层
                var search_post_s = (from it in db.postc
                                     join it2 in db.posts on it.ownpost equals it2.id
                                     join it3 in db.students on it.owner equals it3.id
                                     where it.state == true && it.body.Contains(sd.searchtext)
                                     select new
                                     {
                                         it.body,
                                         it.id,
                                         it.createtime,
                                         postname=it2.title,
                                         postid=it2.id,
                                         nikename=it3.nikename,
                                         stuid=it3.id
                                         
                                     }).ToList();
                for(int i = 0; i < search_post_s.Count; i++)
                {
                    SearchReturn srtemp = new SearchReturn();
                    srtemp.srtype = 3;
                    srtemp.postid = search_post_s[i].postid.ToString();
                    srtemp.stuid = search_post_s[i].stuid;
                    srtemp.nikename = search_post_s[i].nikename;
                    srtemp.replytext = search_post_s[i].body;
                    srtemp.commentsid = search_post_s[i].id.ToString();
                    srtemp.postname = search_post_s[i].postname;
                    srtemp.createtime = search_post_s[i].createtime;
                    srd.Add(srtemp);
                }

                var dd = (from it in db.students
                          where it.nikename == sd.searchtext
                          select it).ToList();
                srd.OrderByDescending(a => a.createtime);
                for(int i = 0; i < dd.Count; i++)
                {
                    SearchReturn srtemp = new SearchReturn();
                    srtemp.srtype = 1;
                    srtemp.ico = dd[i].pic;
                    srtemp.nikename = dd[i].nikename;
                    srtemp.stuid = dd[i].id;
                    srd.Insert(0,srtemp);
                }
                //计算页数
                int allcount = srd.Count;
                int allpage = allcount / perpage;
                if (allcount % perpage != 0)
                {
                    allpage++;
                }
                var search = srd.Skip((sd.index - 1) * perpage).Take(perpage);

                return Ok(new { search, allpage });
            }
        }






    }


}
