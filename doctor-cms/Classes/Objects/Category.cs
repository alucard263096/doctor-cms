using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SunStar_CMS.admin.Classes.Objects
{
    public class Category
    {
        int _CategoryId;

        public int CategoryId
        {
            get { return _CategoryId; }
            set { _CategoryId = value; }
        }
        int _TypeId;

        public int TypeId
        {
            get { return _TypeId; }
            set { _TypeId = value; }
        }
        string _EngName;

        public string EngName
        {
            get { return _EngName; }
            set { _EngName = value; }
        }
        string _ChnName;

        public string ChnName
        {
            get { return _ChnName; }
            set { _ChnName = value; }
        }
        int _FormatType;

        public int FormatType
        {
            get { return _FormatType; }
            set { _FormatType = value; }
        }
        string _Remarks;

        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        int _Status;

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        
    }
}
