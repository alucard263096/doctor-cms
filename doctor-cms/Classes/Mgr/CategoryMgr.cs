using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Data.Common;
using SunStar_CMS.admin.Classes.Utils;
using SunStar_CMS.admin.Classes.ControlValues;
using SunStar_CMS.admin.Classes.Objects;

namespace SunStar_CMS.admin.Classes.Mgr
{
    public class CategoryMgr
    {
        internal object[] getCategoryList(SunStar_CMS.admin.Classes.Objects.User user, string engName, string chnName, string type_id, string status)
        {
            BidirHashtable<object, EnumValueAttribute> recordStatusMap = EnumConvertUtils.EnumToAttributeMap(typeof(RecordStatusEnum));

            string sql = @"SELECT a.category_id, a.eng_name, a.chn_name,b.chn_name type_name, a.remarks, ";
            sql += "CASE a.status ";
            foreach (string recordStatus in Enum.GetNames(typeof(RecordStatusEnum)))
            {
                sql += "WHEN " + Convert.ToString((int)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DbValue) +
                        " THEN '" + (string)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DisplayValue + "' ";
            }
            sql += @"END AS status 
                    FROM tb_category a
inner join tb_type b on a.type_id=b.type_id
                    WHERE 1=1 ";

            if (engName != null)
            {
                sql += "AND a.eng_name LIKE '%" + engName.Replace('\'', '"') + "%' ";
            }
            if (chnName != null)
            {
                sql += "AND a.chn_name LIKE '%" + chnName.Replace('\'', '"') + "%' ";
            }
            if (type_id != null)
            {
                if (Convert.ToInt32(type_id)>0)
                {
                    sql += "AND a.type_id = " + type_id + " ";
                }
            }
            if (status != null)
            {
                sql += "AND a.status = " + status + " ";
            }

            using (DBUtil util = new DBUtil())
            {
                return new object[] { util.getDataSet(sql, "tb_category"), sql };
            }

        }

        internal System.Collections.ArrayList deleteCategory(string categoryList)
        {
            ArrayList result = new ArrayList();
            string sql = "";
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        string[] category_id = categoryList.Split(',');
                        for (int i = 0; i < category_id.Length; i++)
                        {
                            sql = @"DELETE FROM tb_category 
                                                WHERE category_id = @category_id";
                            util.executeNonQuery(sql,
                                                    new string[] { "@category_id" },
                                                    new object[] { Convert.ToInt32(category_id[i]) },
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

        internal object getCategoryObject(int category_id)
        {
            string sql = "select * from tb_category where category_id=" + category_id.ToString();

            DataTable dt = null;
            using (DBUtil util = new DBUtil())
            {
                dt = util.getDataSet(sql, "tb_category").Tables[0];
            }

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                Category category = new Category();
                category.CategoryId = (int)dr["category_id"];
                category.TypeId = (int)dr["type_id"];
                category.ChnName = dr["chn_name"].ToString();
                category.EngName = dr["eng_name"].ToString();
                category.Remarks = dr["remarks"].ToString();
                category.Status = (int)dr["status"];

                return category;
            }
            else
            {
                return null;
            }
        }

        internal int addCategory(User user, Category objcategory)
        {
            using (DBUtil util = new DBUtil())
            {
                int category_id = util.getMasterId("tb_category");
                string sql = @"insert into tb_category (category_id,type_id,eng_name,chn_name,format_type,remarks,status,created_user,created_date,updated_user,updated_date) 
values (@category,@type_id,@eng_name,@chn_name,0,@remarks,@status,@user_id,getdate(),@user_id,getdate()) ";
                string[] param_name = new string[] { "@category", "@type_id", "@eng_name", "@chn_name", "@remarks", "@status", "@user_id" };
                object[] param_value = new object[] { category_id, objcategory.TypeId, objcategory.EngName, objcategory.ChnName, objcategory.Remarks, objcategory.Status, user.UserID };
                util.executeNonQuery(sql, param_name, param_value);
                return category_id;
            }
        }

        internal void editCategory(User user, Category objcategory)
        {
            using (DBUtil util = new DBUtil())
            {
                string sql = @" update tb_category set 
type_id=@type_id,
eng_name=@eng_name,
chn_name=@chn_name,
remarks=@remarks,
status=@status,
updated_user=@user_id,
updated_date=getdate()
where category_id=@category_id";
                string[] param_name = new string[] { "@category_id", "@type_id", "@eng_name", "@chn_name", "@remarks", "@status", "@user_id" };
                object[] param_value = new object[] { objcategory.CategoryId, objcategory.TypeId, objcategory.EngName, objcategory.ChnName, objcategory.Remarks, objcategory.Status, user.UserID };
                util.executeNonQuery(sql, param_name, param_value);

            }
        }

        internal DataTable getCategoryDataTable()
        {

            using (DBUtil util = new DBUtil())
            {
                string sql = "select b.* from tb_type a inner join tb_category b on a.type_id=b.type_id order by b.chn_name";
                return util.getDataSet(sql, "tb_category").Tables[0];
            }
        }

        internal object[] getCategoryList(string sql)
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
    }
}
