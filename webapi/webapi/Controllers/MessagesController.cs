using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace webapi.Controllers
{
    public class MessagesController : ApiController
    {

        public class noticedata
        {
            public int index { get; set; }
            public int nclass { get; set; }
        }
        [HttpGet]
        [ActionName("getMyNotice")]
        public IHttpActionResult getmynotice(noticedata ntd)
        {
            int perpage = 20;
            results res = new results();
            string stuid = new MyApi.SqlHelper().IfLogin();
            if (stuid == null)
            {
                res.result = 0;
            }
            
            using(var db = new oucfreetalkEntities())
            {
                if (ntd.nclass == -1)
                {
                    var searchnotice = (from it in db.notices
                                        join it3 in db.students on it.replystuid equals it3.id
                                        join it4 in db.posts on it.postid equals it4.id
                                        join it5 in db.postc on it.commentsid equals it5.id
                                        join it6 in db.postreply on it.replyid equals it6.id
                                        where it.state == true && it.stuid == stuid
                                        orderby it.createtime descending
                                        select new
                                        {
                                            it.id,
                                            it.noticeclass,
                                            it.postid,
                                            it.createtime,
                                            posttitle = it4.title,
                                            it.commentsid,
                                            commenttext = it5.body,
                                            it.replyid,
                                            replytext = it6.contenttext,
                                            it.replystuid,
                                            it3.nikename,
                                            it3.pic
                                        }).ToList();
                    int allcount = searchnotice.Count;
                    int allpage = allcount / perpage;
                    if (allcount % perpage != 0)
                    {
                        allpage++;
                    }
                    var search = searchnotice.Skip((ntd.index - 1) * perpage).Take(perpage);

                    return Ok(new { search, allpage });

                }
                else
                {
                    var searchnotice = (from it in db.notices
                                        join it3 in db.students on it.replystuid equals it3.id
                                        join it4 in db.posts on it.postid equals it4.id
                                        join it5 in db.postc on it.commentsid equals it5.id
                                        join it6 in db.postreply on it.replyid equals it6.id
                                        where it.state == true && it.stuid == stuid&&it.noticeclass== ntd.nclass
                                        orderby it.createtime descending
                                        select new
                                        {
                                            it.id,
                                            it.noticeclass,
                                            it.postid,
                                            it.createtime,
                                            posttitle = it4.title,
                                            it.commentsid,
                                            commenttext = it5.body,
                                            it.replyid,
                                            replytext = it6.contenttext,
                                            it.replystuid,
                                            it3.nikename,
                                            it3.pic
                                        }).ToList();
                    int allcount = searchnotice.Count;
                    int allpage = allcount / perpage;
                    if (allcount % perpage != 0)
                    {
                        allpage++;
                    }
                    var search = searchnotice.Skip((ntd.index - 1) * perpage).Take(perpage);

                    return Ok(new { search, allpage });
                }
            }
        }


    }
}
