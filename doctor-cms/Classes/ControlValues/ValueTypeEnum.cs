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
    public enum ValueTypeEnum
    {
        [EnumValue(0, "Nil")]
        Nil = 0,
        [EnumValue(1, "String")]
        String = 1,
        [EnumValue(2, "Integer")]
        Integer = 2,
        [EnumValue(3, "Decimal")]
        Decimal = 3,
        [EnumValue(4, "Datetime")]
        Datetime = 4

    }
}
