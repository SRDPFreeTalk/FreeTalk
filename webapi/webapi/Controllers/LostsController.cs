using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace webapi.Controllers
{
    public class LostsController : ApiController
    {

        public class addlistdata
        {
            public string sid { get; set; }
            public string area { get; set; }
            public string SecArea { get; set; }
            public string name { get; set; }
        }
        [HttpPost]
        [ActionName("addLost")]
        public IHttpActionResult addlost(addlistdata ald)
        {
            results res = new results();

            using (var db = new oucfreetalkEntities())
            {
                var laf = new lostafound();
                laf.stuid = ald.sid;
                laf.secarea = ald.SecArea;
                laf.state = false;
                laf.name = ald.name;
                laf.area = ald.area;
                laf.createtime = DateTime.Now;
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


        [HttpGet]
        [ActionName("getMyLost")]
        public IHttpActionResult getmylost(int index)
        {
            results res = new results();
            if (index < 1) index = 1;
            int perpage = 20;
            string stuid = new MyApi.SqlHelper().IfLogin();
            if (stuid == null)
            {
                res.result = 0;
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                var mylost = (from it in db.lostafound
                              where it.stuid == stuid && it.state == true
                              orderby it.createtime descending
                              select it).ToList();
                int allcount = mylost.Count;
                int allpage = allcount / perpage;
                if (allcount % perpage != 0)
                {
                    allpage++;
                }
                var search = mylost.Skip((index - 1) * perpage).Take(perpage);

                return Ok(new { search, allpage });
            }
        }

        [HttpPost]
        [ActionName("IGotIt")]
        public IHttpActionResult gotit(int lostid)
        {
            results res = new results();
            string stuid = new MyApi.SqlHelper().IfLogin();
            if (stuid == null)
            {
                res.result = 0;
                return Ok(res);
            }
            using (var db = new oucfreetalkEntities())
            {
                var thislot = db.lostafound.FirstOrDefault(a => a.id == lostid && a.state == false);
                if (thislot == null)
                {
                    res.result = 2;
                    return Ok(res);

                }
                if (thislot.stuid != stuid)
                {
                    res.result = 4;
                    return Ok(res);
                }
                thislot.state = false;

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
    }
}
