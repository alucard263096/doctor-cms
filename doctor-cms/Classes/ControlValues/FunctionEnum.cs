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
    public class FunctionEnum
    {
        public enum FunctionType
        {
            [EnumValue(1, "Read Only")]
            ReadOnly = 1,
            [EnumValue(3, "Assign right")]
            AssignRight = 3,
            [EnumValue(6, "Edit right")]
            EditRight = 6,
            [EnumValue(9, "Full Control")]
            FullControl = 9
        }

        public enum FunctionGroup
        {
            [EnumValue(1, "活动管理")]
            EventMaintenance = 1,
            [EnumValue(2, "医生管理")]
            DoctorMaintenance = 2,
            [EnumValue(3, "会员管理")]
            MemberMaintenance = 3,
            [EnumValue(6, "用户管理")]
            UserMaintenance = 6
        }


    }
}
