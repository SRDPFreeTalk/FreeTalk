using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapi
{
    public class MessageHelper
    {

        public  bool addcommentmessage(int commentid)
        {
            using(var db = new oucfreetalkEntities())
            {
                var searchcomment = db.postc.FirstOrDefault(a => a.id == commentid);
                if (searchcomment == null) return false;
                var thispost = db.posts.FirstOrDefault(a => a.id == searchcomment.ownpost);
                try
                {
                    notices ntc = new notices();
                    ntc.noticeclass = 1;//帖子被人回复
                    ntc.state = true;
                    ntc.stuid = thispost.owner;
                    ntc.replystuid = searchcomment.owner;
                    ntc.createtime = searchcomment.createtime;
                    ntc.postid = thispost.id;
                    ntc.commentsid = searchcomment.id;
                    db.notices.Add(ntc);
                    if(db.SaveChanges() == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }catch
                {
                    return false;
                }
            }
        }

        public bool addreplymessage(int replyid)
        {
            using (var db = new oucfreetalkEntities())
            {
                var searchreply = db.postreply.FirstOrDefault(a => a.id == replyid);
                if (searchreply == null) return false;
                var thiscomment = db.postc.FirstOrDefault(a => a.id == searchreply.ownlocation);
                try
                {
                    notices ntc = new notices();
                    ntc.noticeclass = 2;//楼层被人回复
                    ntc.state = true;
                    ntc.stuid = thiscomment.owner;
                    ntc.replystuid = searchreply.owner;
                    ntc.createtime = searchreply.createtime;
                    ntc.commentsid = thiscomment.id;
                    ntc.postid = thiscomment.ownpost;
                    ntc.replyid = searchreply.id;
                    db.notices.Add(ntc);
                    if (searchreply.replyto != thiscomment.owner)//如果不是回复评论主
                    {
                        notices ntc2 = new notices();
                        ntc.noticeclass = 3;//楼层的回复被人回复了
                        ntc.state = true;
                        ntc.stuid = searchreply.replyto;
                        ntc.replystuid = searchreply.owner;
                        ntc.createtime = searchreply.createtime;
                        ntc.commentsid = thiscomment.id;
                        ntc.postid = thiscomment.ownpost;
                        ntc.replyid = searchreply.id;
                    }
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

    }
}