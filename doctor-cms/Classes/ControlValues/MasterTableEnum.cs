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
    public class MasterTableEnum
    {

        public enum Gender
        {
            [EnumValue("M", "M - Male")]
            Male,
            [EnumValue("F", "F - Female")]
            FeMale
        }

    }
}
