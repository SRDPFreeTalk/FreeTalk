using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapi
{
    public class buridata
    {
        public static void addbridata(string stuid,int actionid ,int resultid)
        {
            using(var db = new oucfreetalkEntities())
            {
                var bdata = new burieddata();
                
                bdata.stuid = stuid;
                bdata.actionid = actionid;
                bdata.resultid = resultid;
                bdata.createtime = DateTime.Now;
                db.burieddata.Add(bdata);
                db.SaveChanges();
            }
        }
    }
}