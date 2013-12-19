using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SunStar_CMS.admin.Classes.Utils;
using SunStar_CMS.admin.Classes.Objects;
using SunStar_CMS.admin.Classes.ControlValues;
using System.Collections;
using System.Data.Common;

namespace SunStar_CMS.admin.Classes.Mgr
{
    public class NewsMgr
    {

        internal object[] getNewsList(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return new object[] { null, sql };
            }
            using (DBUtil util = new DBUtil())
            {
                return new object[] { util.getDataSet(sql, "tb_type"), sql };
            }
        }

        internal object[] getNewsList(SunStar_CMS.admin.Classes.Objects.User user, string title, string author, string status)
        {
            BidirHashtable<object, EnumValueAttribute> recordStatusMap = EnumConvertUtils.EnumToAttributeMap(typeof(RecordStatusEnum));

            string sql = @"SELECT a.news_id,a.title, a.author, a.write_date,  ";
            sql += "CASE a.status ";
            foreach (string recordStatus in Enum.GetNames(typeof(RecordStatusEnum)))
            {
                sql += "WHEN " + Convert.ToString((int)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DbValue) +
                        " THEN '" + (string)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DisplayValue + "' ";
            }
            sql += @"END AS status 
                    FROM tb_news a
                    WHERE 1=1 ";

            if (title != null)
            {
                sql += "AND a.title LIKE '%" + title.Replace('\'', '"') + "%' ";
            }
            if (author != null)
            {
                sql += "AND a.author LIKE '%" + author.Replace('\'', '"') + "%' ";
            }
            if (status != null)
            {
                sql += "AND a.status = " + status + " ";
            }

            using (DBUtil util = new DBUtil())
            {
                return new object[] { util.getDataSet(sql, "tb_news"), sql };
            }

        }

        internal System.Collections.ArrayList deleteNews(string newsList)
        {
            ArrayList result = new ArrayList();
            string sql = "";
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        string[] news_id = newsList.Split(',');
                        for (int i = 0; i < news_id.Length; i++)
                        {
                            sql = @"DELETE FROM tb_news 
                                                WHERE news_id = @news_id";
                            util.executeNonQuery(sql,
                                                    new string[] { "@news_id" },
                                                    new object[] { Convert.ToInt32(news_id[i]) },
                                                    tx);
                        }
                        tx.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        internal object getNewsObject(int news_id)
        {
            string sql = "select * from tb_news where news_id=" + news_id.ToString();

            DataTable dt = null;
            using (DBUtil util = new DBUtil())
            {
                dt = util.getDataSet(sql, "tb_news").Tables[0];
            }

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                News n = new News();
                n.NewsId = (int)dr["news_id"];
                n.Title = dr["title"].ToString();
                n.HtmlContent = dr["html_content"].ToString();
                n.Author = dr["author"].ToString();
                n.WriteDate = dr["write_date"].ToString();
                n.Remarks = dr["remarks"].ToString();
                n.Status = (int)dr["status"];

                return n;
            }
            else
            {
                return null;
            }
        }

        internal int addNews(User user, News objnews)
        {
            using (DBUtil util = new DBUtil())
            {
                int news_id = util.getMasterId("tb_news");
                string sql = @"insert into tb_news (news_id,title,html_content,author,write_date,remarks,status,created_user,created_date,updated_user,updated_date) 
values (@news_id,@title,@html_content,@author,@write_date,@remarks,@status,@user_id,getdate(),@user_id,getdate()) ";
                string[] param_name = new string[] { "@news_id", "@title", "@html_content", "@author", "@write_date", "@remarks", "@status", "@user_id" };
                object[] param_value = new object[] { news_id, objnews.Title, objnews.HtmlContent, objnews.Author, objnews.WriteDate, objnews.Remarks, objnews.Status, user.UserID };
                util.executeNonQuery(sql, param_name, param_value);
                return news_id;
            }
        }

        internal void editNews(User user, News objnews)
        {
            using (DBUtil util = new DBUtil())
            {

                string sql = @"update tb_news set title=@title,html_content=@html_content,author=@author,write_date=@write_date,remarks=@remarks,status=@status,updated_user=@user_id,updated_date=getdate() 
where news_id=@news_id ";
                string[] param_name = new string[] { "@news_id", "@title", "@html_content", "@author", "@write_date", "@remarks", "@status", "@user_id" };
                object[] param_value = new object[] { objnews.NewsId, objnews.Title, objnews.HtmlContent, objnews.Author, objnews.WriteDate, objnews.Remarks, objnews.Status, user.UserID };
                util.executeNonQuery(sql, param_name, param_value);
            }
        }
    }
}
