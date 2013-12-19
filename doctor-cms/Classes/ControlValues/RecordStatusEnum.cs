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
    public enum RecordStatusEnum
    {
        [EnumValue(1, "不启用")]
        Inactive=1,
        [EnumValue(0, "启用")]
        Active=0
        
    }


    public enum QuestionStatusEnum
    {
        [EnumValue(0, "未处理")]
        Untreated = 0,
        [EnumValue(1, "已回答")]
        Treated = 1,
        [EnumValue(2, "忽略")]
        Ignore = 2

    }



}
