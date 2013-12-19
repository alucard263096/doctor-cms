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
    public class EnumValueAttribute : Attribute
    {
        object _dbValue;

        public object DbValue
        {
            get { return _dbValue; }
        }
        object _displayValue;

        public object DisplayValue
        {
            get { return _displayValue; }
        }

        public EnumValueAttribute(object dbValue, object displayValue)
        {
            _dbValue = dbValue;
            _displayValue = displayValue;
        }
    }
}
