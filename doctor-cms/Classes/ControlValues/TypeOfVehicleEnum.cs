using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SunStar_CMS.admin.Classes.ControlValues
{

    public class TypeOfVehicleEnum
    {
        public enum TypeOfVehicleEnum_eng
        {
            [EnumValue(1, "Comprehensive")]
            Comprehensive = 1,
            [EnumValue(7, "3rd Party Only")]
            Party = 7
        }

        public enum TypeOfVehicleEnum_chn
        {
            [EnumValue(1, "全保")]
            Comprehensive = 1,
            [EnumValue(7, "第三者保险")]
            Party = 7
        }

    }
}
