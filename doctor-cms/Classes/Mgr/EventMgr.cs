using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SunStar_CMS.admin.Classes.Objects;
using System.Collections;
using SunStar_CMS.admin.Classes.Utils;
using System.Data.Common;
using SunStar_CMS.admin.Classes.ControlValues;
using doctor_cms.Classes.Objects;
using System.Collections.Generic;

namespace SunStar_CMS.admin.Classes.Mgr
{
    public class EventMgr
    {
        internal object[] getEventList(User user, string title, string content, int status)
        {
            BidirHashtable<object, EnumValueAttribute> recordStatusMap = EnumConvertUtils.EnumToAttributeMap(typeof(RecordStatusEnum));

            string sql = @"SELECT a.eventId, a.title, a.summary,publishedDate, ";
            sql += "CASE a.status ";
            foreach (string recordStatus in Enum.GetNames(typeof(RecordStatusEnum)))
            {
                sql += "WHEN " + Convert.ToString((int)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DbValue) +
                        " THEN '" + (string)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DisplayValue + "' ";
            }
            sql += @"END AS status 
                    FROM tb_event a
                    WHERE 1=1 ";

            if (title != null)
            {
                sql += "AND a.title LIKE '%" + title.Replace('\'', '"') + "%' ";
            }
            if (content != null)
            {
                sql += "AND a.content LIKE '%" + content.Replace('\'', '"') + "%' ";
            }
            if (status != null)
            {
                sql += "AND a.status = " + status + " ";
            }

            using (DBUtil util = new DBUtil())
            {
                return new object[] { util.getDataSet(sql, "tb_event"), sql };
            }


        }

        public List<Event> getEventListByUpdateDate(string latest_update_date)
        {
            List<Event> lstEvent = new List<Event>();
            string sql = "select * from tb_event where updated_date>'" + latest_update_date.Replace('\'', '"') + "'";
            DataTable dt = null;
            using (DBUtil util = new DBUtil())
            {
                dt = util.getDataSet(sql, "tb_event").Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    Event e = new Event()
                    {
                        EventId = Convert.ToInt32(dr["eventId"]),
                        Summary = Convert.ToString(dr["summary"]),
                        Content = Convert.ToString(dr["content"]),
                        ImageUrl = Convert.ToString(dr["imageUrl"]),
                        PublishedDate = Convert.ToDateTime(dr["publishedDate"]),
                        Status = Convert.ToInt32(dr["status"]),
                        Title = Convert.ToString(dr["title"])
                    };
                    lstEvent.Add(e);
                }
            }
            return lstEvent;
        }

       

        internal object[] getEventList(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return new object[] { null, sql };
            }
            using (DBUtil util = new DBUtil())
            {
                return new object[] { util.getDataSet(sql, "tb_event"), sql };
            }
        }


        internal object getEventObject(int eventId)
        {
            string sql = "select * from tb_event where eventId=" + eventId.ToString();

            DataTable dt = null;
            using (DBUtil util = new DBUtil())
            {
                dt = util.getDataSet(sql, "tb_event").Tables[0];
            }

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                Event e = new Event()
                {
                    EventId = Convert.ToInt32(dr["eventId"]),
                    Summary = Convert.ToString(dr["summary"]),
                    Content = Convert.ToString(dr["content"]),
                    ImageUrl = Convert.ToString(dr["imageUrl"]),
                    PublishedDate = Convert.ToDateTime(dr["publishedDate"]),
                    Status = Convert.ToInt32(dr["status"]),
                    Title = Convert.ToString(dr["title"])
                };
                return e;
            }
            else
            {
                return null;
            }

        }

        internal int addEvent(User user, Event e)
        {
            using (DBUtil util = new DBUtil())
            {
                int eventId = util.getMasterId("tb_event");
                e.EventId = eventId;
                string sql = @"insert into tb_event (eventId,title,summary,imageUrl,publishedDate,[content],status,created_user,created_date,updated_user,updated_date) 
values (@eventId,@title,@summary,@imageUrl,@publishedDate,@content,@status,@user_id,getdate(),@user_id,getdate()) ";
                string[] param_name = new string[] { "@eventId", "@title", "@summary", "@imageUrl", "@publishedDate", "@content", "@status", "@user_id" };
                object[] param_value = new object[] { e.EventId, e.Title, e.Summary, e.ImageUrl, e.PublishedDate, e.Content, e.Status, user.UserID };
                util.executeNonQuery(sql, param_name, param_value);
                return eventId;
            }
        }

        internal void editEvent(User user, Event e)
        {
            using (DBUtil util = new DBUtil())
            {
                string sql = @" update tb_event set 
title=@title,
summary=@summary,
imageUrl=@imageUrl,
publishedDate=@publishedDate,
[content]=@content,
status=@status,
updated_user=@user_id,
updated_date=getdate()
where eventId=@eventId";
                string[] param_name = new string[] { "@eventId", "@title", "@summary", "@imageUrl", "@publishedDate", "@content", "@status", "@user_id" };
                object[] param_value = new object[] { e.EventId, e.Title, e.Summary, e.ImageUrl, e.PublishedDate, e.Content, e.Status, user.UserID };
                util.executeNonQuery(sql, param_name, param_value);
                
            }
        }

        
    }
}
