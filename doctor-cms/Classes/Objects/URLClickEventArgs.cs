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
    public class URLClickEventArgs : EventArgs
    {
        private readonly int _DataID = 0;
        private readonly string _DataIDList = "";

        public URLClickEventArgs()
        {
        }

        public URLClickEventArgs(int DataID, string DataIDList)
        {
            this._DataID = DataID;
            this._DataIDList = DataIDList;
        }

        public int DataID
        {
            get { return _DataID; }
        }

        public string DataIDList
        {
            get { return _DataIDList; }
        }
    }
}
