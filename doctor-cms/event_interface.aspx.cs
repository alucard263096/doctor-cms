using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SunStar_CMS.admin.Classes.Mgr;
using doctor_cms.Classes.Objects;

namespace doctor_cms
{
    public partial class event_interface : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request["type"];
            if (type == "geteventlist")
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<?xml version='1.0' encoding='utf-8' ?>");
                sb.Append("<root>");
                sb.Append("<request_date>");
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("</request_date>");
                sb.Append("<events>");
                EventMgr mgr = new EventMgr();
                List<Event> lst = mgr.getEventListByUpdateDate(Request["request_date"]);
                foreach (Event eve in lst)
                {
                    sb.Append("<event>");
                    sb.AppendFormat("<eventid>{0}</eventid>", eve.EventId);
                    sb.AppendFormat("<title>{0}</title>", eve.Title.Replace("<", "&lt;").Replace(">", "&rt;"));
                    sb.AppendFormat("<summary>{0}</summary>", eve.Summary.Replace("<", "&lt;").Replace(">", "&rt;"));
                    sb.AppendFormat("<imageurl>{0}</imageurl>", eve.ImageUrl.Replace("<", "&lt;").Replace(">", "&rt;"));
                    sb.AppendFormat("<content>{0}</content>", eve.Content.Replace("<", "&lt;").Replace(">", "&rt;"));
                    sb.AppendFormat("<publisheddate>{0}</publisheddate>", eve.PublishedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    sb.AppendFormat("<status>{0}</status>", eve.Status);
                    sb.Append("</event>");
                }
                sb.Append("</events>");
                sb.Append("</root>");
                this.Response.Clear();
                Response.ContentType = "text/xml";
                this.Response.Write(sb.ToString());
            }
        }
    }
}