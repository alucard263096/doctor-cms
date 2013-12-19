using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace SunStar_CMS.admin.Classes.Objects
{
    public class ScreenNo
    {
        private Hashtable _screenNo;

        public ScreenNo()
        {
            _screenNo = new Hashtable();
            _screenNo.Add("admin/home.aspx", "0");
            _screenNo.Add("admin/password.aspx", "1");
            _screenNo.Add("admin/user_list.aspx", "12");
            _screenNo.Add("admin/user_detail.aspx", "12.1");
        }

        public object getSceenNo(object key)
        {
            return _screenNo[key];
        }
    }
}
