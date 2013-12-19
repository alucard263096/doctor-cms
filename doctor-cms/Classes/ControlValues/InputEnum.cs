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
using SunStar_CMS.admin.Classes.Mgr;
using SunStar_CMS.admin.Classes.Utils;
using SunStar_CMS.admin.Classes.ControlValues;

namespace SunStar_CMS.admin.Classes.ControlValues
{
    public enum InputEnum
    {
       
        Button = 0,
        
        Checkbox = 1,
        
        File = 2,
        
        Hidden = 3,
        
        Image = 4,
        
        Password = 5,
        
        Radio = 6,
        
        Reset = 7,
        
        Submit = 8,
        
        Text = 9
    }
}
