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
            [EnumValue(1, "类型管理")]
            TypeMaintenance = 1,
            [EnumValue(2, "类别管理")]
            CategoryMaintenance = 2,
            [EnumValue(3, "参数管理")]
            FactorMaintenance = 3,
            [EnumValue(4, "产品管理")]
            ProductMaintenance = 4,
            [EnumValue(5, "会员管理")]
            CustomerMaintenance = 5,
            [EnumValue(6, "用户管理")]
            UserMaintenance = 6,
            [EnumValue(7, "新闻管理")]
            NewsMaintenance = 7,
            [EnumValue(8, "留言问题管理")]
            QuestionMaintenance = 8
        }


    }
}
