using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapi
{
    public class PostAcess
    {

        public bool GetPostAccess(string userid,int postid)
        {
            using(var db = new oucfreetalkEntities())
            {
                //权限
                var searchaccess = (from it in db.accountaccess
                                    where it.studentid == userid && it.classid == -1
                                    select it).ToList();
                if (searchaccess.Count != 0) return true;//权限狗
                //post部分
                var searchpost = (from it in db.posts
                                  where it.id == postid
                                  select it).ToList();
                if (searchpost.Count == 0) return false;
                if (searchpost[0].owner == userid) return true;//本人帖子
                var getac = db.accountaccess.FirstOrDefault(a => a.classid == searchpost[0].ownclass && a.studentid == userid);
                if (getac == null) return false;
                else return true;//版主

            }
        }

        public bool GetCommentAccess(string userid, int commentid)
        {
            using (var db = new oucfreetalkEntities())
            {
                //权限
                var searchaccess = (from it in db.accountaccess
                                    where it.studentid == userid && it.classid == -1
                                    select it).ToList();
                if (searchaccess.Count != 0) return true;//权限狗
                //comment部分
                var searchpost = (from it in db.postc
                                  join it2 in db.posts on it.ownpost equals it2.id
                                  where it.id == commentid
                                  select new
                                  {
                                      it.ownpost,
                                      it.owner,
                                      it2.ownclass,
                                      postowner = it2.owner
                                  }).ToList();
                if (searchpost.Count == 0) return false;
                if (searchpost[0].owner == userid) return true;//本人楼层
                if (searchpost[0].postowner == userid) return true;//本人帖子
                var getac = db.accountaccess.FirstOrDefault(a => a.classid == searchpost[0].ownclass && a.studentid == userid);
                if (getac == null) return false;
                else return true;//版主
            }
        }

        public bool GetReplyAccess(string userid, int replyid)
        {
            using (var db = new oucfreetalkEntities())
            {
                //权限
                var searchaccess = (from it in db.accountaccess
                                    where it.studentid == userid && it.classid == -1
                                    select it).ToList();
                if (searchaccess.Count != 0) return true;//权限狗
                //reply部分
                var searchpost = (from it in db.postreply
                                  join it2 in db.postc on it.ownlocation equals it2.id
                                  join it3 in db.posts on it2.ownpost equals it3.id
                                  where it.id == replyid
                                  select new
                                  {
                                      it.owner,
                                      commentowner=it2.owner,
                                      it3.ownclass
                                      
                                  }).ToList();
                if (searchpost.Count == 0) return false;
                if (searchpost[0].owner == userid) return true;//本人楼层
                if (searchpost[0].commentowner == userid) return true;//本人回复
                var getac = db.accountaccess.FirstOrDefault(a => a.classid == searchpost[0].ownclass && a.studentid == userid);
                if (getac == null) return false;
                else return true;//版主
            }
        }

    }
}