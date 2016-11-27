<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Linq;
using System.Text;

public class Handler : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        HttpResponse res = context.Response;
        HttpRequest req = context.Request;
        res.ContentType = "text/plain";
        string password = req.Form["password"].Trim();
        string account = req.Form["account"].Trim();
        try
        {
            using(var db = new oucfreetalkEntities())
            {
                var se = from it in db.students
                         where it.id == password
                         select it;
                if (se.Count() == 1)
                {
                        StringBuilder sb = new StringBuilder("{");
                sb.Append("\"id\":\"" + se.First().id + "\"");
                /*
                sb.Append(",\"lesnum\":\"" + se.lesnum + "\"");
                sb.Append(",\"lesgoal\":\"" + se.lesgoal + "\"");
                if (se.lesbook != null)
                {
                    sb.Append(",\"lesbook\":\"" + se.lesbook + "\"");
                }
                else
                {
                    sb.Append(",\"lesbook\":\"" + "无" + "\"");
                }
                sb.Append(",\"lestest\":\"" + se.lestest + "\"");*/
                sb.Append("}");
                res.Write(sb.ToString());
                //res.Write(sb);
                res.End();
                }else
                {

                }
            }
        }
        catch { }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}