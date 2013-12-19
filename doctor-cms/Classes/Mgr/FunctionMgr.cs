using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SunStar_CMS.admin.Classes.ControlValues;
using SunStar_CMS.admin.Classes.Utils;
using SunStar_CMS.admin.Classes.Objects;

namespace SunStar_CMS.admin.Classes.Mgr
{
    public class FunctionMgr
    {
        public DataSet getFunction()
        {
            using (DBUtil util = new DBUtil())
            {
                return util.getDataSet(@"SELECT * 
                                        FROM tb_function 
                                        WHERE status = @status 
                                        ORDER BY seq",
                                        "tb_function",
                                        new string[] { "@status" },
                                        new object[] { (int)RecordStatusEnum.Active });
            }
        }

        public DataSet getAccessFunction()
        {
            using (DBUtil util = new DBUtil())
            {
                string strSQL = "";
                BidirHashtable<object, EnumValueAttribute> functionAccessMap = EnumConvertUtils.EnumToAttributeMap(typeof(FunctionEnum.FunctionType));

                strSQL = "SELECT *, CASE function_type ";
                foreach (string functionAccess in Enum.GetNames(typeof(FunctionEnum.FunctionType)))
                {
                    strSQL += "WHEN " + Convert.ToString((int)functionAccessMap[Enum.Parse(typeof(FunctionEnum.FunctionType), functionAccess)].DbValue) +
                            " THEN function_name + ' (" + (string)functionAccessMap[Enum.Parse(typeof(FunctionEnum.FunctionType), functionAccess)].DisplayValue + ")' ";
                }
                strSQL += "END AS function_access FROM tb_function WHERE status = @status ORDER BY seq";

                return util.getDataSet(@strSQL,
                                        "tb_function",
                                        new string[] { "@status" },
                                        new object[] { (int)RecordStatusEnum.Active });
            }
        }
    }
}
