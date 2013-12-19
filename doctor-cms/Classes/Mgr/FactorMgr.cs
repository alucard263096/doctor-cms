using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using SunStar_CMS.admin.Classes.Objects;
using SunStar_CMS.admin.Classes.ControlValues;
using SunStar_CMS.admin.Classes.Utils;
using System.Collections;

namespace SunStar_CMS.admin.Classes.Mgr
{
    public class FactorMgr
    {
        internal object[] getFactorList(SunStar_CMS.admin.Classes.Objects.User user, string engName, string chnName, string type_id, string category_id, string status)
        {
            BidirHashtable<object, EnumValueAttribute> recordStatusMap = EnumConvertUtils.EnumToAttributeMap(typeof(RecordStatusEnum));

            string sql = @"SELECT a.factor_id, a.eng_name, a.chn_name,b.chn_name category_name,c.chn_name type_name, a.remarks, ";
            sql += "CASE a.status ";
            foreach (string recordStatus in Enum.GetNames(typeof(RecordStatusEnum)))
            {
                sql += "WHEN " + Convert.ToString((int)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DbValue) +
                        " THEN '" + (string)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DisplayValue + "' ";
            }
            sql += @"END AS status 
                    FROM tb_factor a
inner join tb_category b on a.category_id=b.category_id
inner join tb_type c on b.type_id=c.type_id
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
                if (Convert.ToInt32(type_id) > 0)
                {
                    sql += "AND c.type_id = " + type_id + " ";
                }
            }
            if (category_id != null)
            {
                if (Convert.ToInt32(category_id) > 0)
                {
                    sql += "AND b.category_id = " + type_id + " ";
                }
            }
            if (status != null)
            {
                sql += "AND a.status = " + status + " ";
            }

            using (DBUtil util = new DBUtil())
            {
                return new object[] { util.getDataSet(sql, "tb_factor"), sql };
            }
        }

        internal System.Collections.ArrayList deleteFactor(string factorList)
        {
            ArrayList result = new ArrayList();
            string sql = "";
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        string[] factor_id = factorList.Split(',');
                        for (int i = 0; i < factor_id.Length; i++)
                        {
                            sql = @"DELETE FROM tb_factor 
                                                WHERE factor_id = @factor_id";
                            util.executeNonQuery(sql,
                                                    new string[] { "@factor_id" },
                                                    new object[] { Convert.ToInt32(factor_id[i]) },
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

        internal object getFactorObject(int factor_id)
        {
            string sql = "select * from tb_factor where factor_id=" + factor_id.ToString();

            DataTable dt = null;
            using (DBUtil util = new DBUtil())
            {
                dt = util.getDataSet(sql, "tb_factor").Tables[0];
            }

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                Factor factor = new Factor();
                factor.FactorId = (int)dr["factor_id"];
                factor.CategoryId = (int)dr["category_id"];
                factor.ChnName = dr["chn_name"].ToString();
                factor.EngName = dr["eng_name"].ToString();
                factor.Remarks = dr["remarks"].ToString();
                factor.Status = (int)dr["status"];
                factor.Priority = (int)dr["priority"];

                return factor;
            }
            else
            {
                return null;
            }
        }

        internal int addFactor(User user, Factor objfactor)
        {
            using (DBUtil util = new DBUtil())
            {
                int factor_id = util.getMasterId("tb_factor");
                string sql = @"insert into tb_factor (factor_id,category_id,eng_name,chn_name,start_value,end_value,remarks,status,priority,created_user,created_date,updated_user,updated_date) 
values (@factor_id,@category_id,@eng_name,@chn_name,0,0,@remarks,@status,@priority,@user_id,getdate(),@user_id,getdate()) ";
                string[] param_name = new string[] { "@factor_id", "@category_id", "@eng_name", "@chn_name", "@remarks", "@status", "@priority", "@user_id" };
                object[] param_value = new object[] { factor_id, objfactor.CategoryId, objfactor.EngName, objfactor.ChnName, objfactor.Remarks, objfactor.Status, objfactor.Priority, user.UserID };
                util.executeNonQuery(sql, param_name, param_value);
                return factor_id;
            }
        }

        internal void editFactor(User user, Factor objfactor)
        {
            using (DBUtil util = new DBUtil())
            {
                string sql = @" update tb_factor set 
category_id=@category_id,
eng_name=@eng_name,
chn_name=@chn_name,
remarks=@remarks,
status=@status,
priority=@priority,
updated_user=@user_id,
updated_date=getdate()
where factor_id=@factor_id";
                string[] param_name = new string[] { "@factor_id", "@category_id", "@eng_name", "@chn_name", "@remarks", "@status", "@priority", "@user_id" };
                object[] param_value = new object[] { objfactor.FactorId, objfactor.CategoryId, objfactor.EngName, objfactor.ChnName, objfactor.Remarks, objfactor.Status, objfactor.Priority,user.UserID };
                util.executeNonQuery(sql, param_name, param_value);

            }
        }

        internal DataSet getFacotrDataSet(int category_id)
        {

            using (DBUtil util = new DBUtil())
            {
                string sql = "select * from tb_factor where category_id="+category_id+" order by chn_name";
                return util.getDataSet(sql, "tb_factor");
            }
        }

        internal object[] getFactorList(string sql)
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
