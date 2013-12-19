using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SunStar_CMS.admin.Classes.Objects
{
    public class User
    {
        private int _userId;
        private string _loginId;
        private string _userName;
        private int _type;
        private int _masterDeptId;
        private string _email;
        private int _rcyDistrictId;
        private int _rcyUnitId;
        private string _remarks;
        private int _status;
        private ArrayList _userFunction;

        public int UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string LoginID
        {
            get { return _loginId; }
            set { _loginId = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int MasterDeptID
        {
            get { return _masterDeptId; }
            set { _masterDeptId = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public int RCYDistrictID
        {
            get { return _rcyDistrictId; }
            set { _rcyDistrictId = value; }
        }

        public int RCYUnitID
        {
            get { return _rcyUnitId; }
            set { _rcyUnitId = value; }
        }

        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public ArrayList UserFunction
        {
            get { return _userFunction; }
            set { _userFunction = value; }
        }
    }
}
