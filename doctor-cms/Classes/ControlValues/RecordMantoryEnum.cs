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
    public enum RecordMantoryEnum
    {
        
        [EnumValue(0, "False")]
        False=0,
        [EnumValue(1, "True")]
        True=1
    }
}
