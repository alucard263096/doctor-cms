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
    public enum ComputeActionEnum
    {
        [EnumValue("1", "Sum")]
        Sum = 1,
        [EnumValue("2", "Maximum")]
        Maximum = 2,
        [EnumValue("3", "Minimum")]
        Minimum = 3,
        [EnumValue("4", "Count")]
        Count = 4

    }
}
