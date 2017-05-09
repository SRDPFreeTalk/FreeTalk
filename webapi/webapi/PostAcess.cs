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
                
            }
            return true;
        }

        public bool GetCommentAccess(string userid, int commentid)
        {
            using (var db = new oucfreetalkEntities())
            {

            }
            return true;
        }

        public bool GetReplyAccess(string userid, int postid)
        {
            using (var db = new oucfreetalkEntities())
            {

            }
            return true;
        }

    }
}