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
    public class News
    {
        int _NewsId;

        public int NewsId
        {
            get { return _NewsId; }
            set { _NewsId = value; }
        }

        string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        string _HtmlContent;

        public string HtmlContent
        {
            get { return _HtmlContent; }
            set { _HtmlContent = value; }
        }
        string _Author;

        public string Author
        {
            get { return _Author; }
            set { _Author = value; }
        }
        string _WriteDate;

        public string WriteDate
        {
            get { return _WriteDate; }
            set { _WriteDate = value; }
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
