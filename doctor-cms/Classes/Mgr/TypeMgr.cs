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

namespace SunStar_CMS.admin.Classes.Mgr
{
    public class TypeMgr
    {
        internal object[] getTypeList(User user, string engName, string chnName, string status)
        {
            BidirHashtable<object, EnumValueAttribute> recordStatusMap = EnumConvertUtils.EnumToAttributeMap(typeof(RecordStatusEnum));

            string sql = @"SELECT a.type_id, a.eng_name, a.chn_name, a.remarks, ";
            sql += "CASE a.status ";
            foreach (string recordStatus in Enum.GetNames(typeof(RecordStatusEnum)))
            {
                sql += "WHEN " + Convert.ToString((int)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DbValue) +
                        " THEN '" + (string)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DisplayValue + "' ";
            }
            sql += @"END AS status 
                    FROM tb_type a
                    WHERE 1=1 ";

            if (engName != null)
            {
                sql += "AND a.eng_name LIKE '%" + engName.Replace('\'', '"') + "%' ";
            }
            if (chnName != null)
            {
                sql += "AND a.chn_name LIKE '%" + chnName.Replace('\'', '"') + "%' ";
            }
            if (status != null)
            {
                sql += "AND a.status = " + status + " ";
            }

            using (DBUtil util = new DBUtil())
            {
                return new object[] { util.getDataSet(sql, "tb_type"), sql };
            }


        }

        internal System.Collections.ArrayList deleteType(string typeList)
        {
            ArrayList result = new ArrayList();
            string sql = "";
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        string[] type_id = typeList.Split(',');
                        for (int i = 0; i < type_id.Length; i++)
                        {
                            sql = @"DELETE FROM tb_type 
                                                WHERE type_id = @type_id";
                            util.executeNonQuery(sql,
                                                    new string[] { "@type_id" },
                                                    new object[] { Convert.ToInt32(type_id[i]) },
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

        internal object[] getTypeList(string sql)
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


        internal object getTypeObject(int type_id)
        {
            string sql = "select * from tb_type where type_id="+type_id.ToString();

            DataTable dt = null;
            using (DBUtil util = new DBUtil())
            {
                dt = util.getDataSet(sql, "tb_type").Tables[0];
            }

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ObjType type = new ObjType();
                type.TypeId = (int)dr["type_id"];
                type.ChnName = dr["chn_name"].ToString();
                type.EngName = dr["eng_name"].ToString();
                type.Remarks = dr["remarks"].ToString();
                type.Status = (int)dr["status"];

                return type;
            }
            else
            {
                return null;
            }

        }

        internal int addType(User user, ObjType objtype)
        {
            using (DBUtil util = new DBUtil())
            {
                int type_id = util.getMasterId("tb_type");
                string sql = @"insert into tb_type (type_id,eng_name,chn_name,remarks,status,created_user,created_date,updated_user,updated_date) 
values (@type_id,@eng_name,@chn_name,@remarks,@status,@user_id,getdate(),@user_id,getdate()) ";
                string[] param_name = new string[] { "@type_id", "@eng_name", "@chn_name", "@remarks", "@status", "@user_id" };
                object[] param_value = new object[] { type_id, objtype.EngName, objtype.ChnName, objtype.Remarks, objtype.Status, user.UserID };
                util.executeNonQuery(sql, param_name, param_value);
                return type_id;
            }
        }

        internal void editType(User user, ObjType objtype)
        {
            using (DBUtil util = new DBUtil())
            {
                string sql=@" update tb_type set 
eng_name=@eng_name,
chn_name=@chn_name,
remarks=@remarks,
status=@status,
updated_user=@user_id,
updated_date=getdate()
where type_id=@type_id";
                string[] param_name = new string[] { "@type_id", "@eng_name", "@chn_name", "@remarks", "@status", "@user_id" };
                object[] param_value = new object[] { objtype.TypeId, objtype.EngName, objtype.ChnName, objtype.Remarks, objtype.Status, user.UserID };
                util.executeNonQuery(sql, param_name, param_value);
                
            }
        }

        internal DataTable getTypeDataTable()
        {
            using (DBUtil util = new DBUtil())
            {
                string sql = "select * from tb_type order by chn_name";
                return util.getDataSet(sql, "tb_type").Tables[0];
            }
        }
    }
}
