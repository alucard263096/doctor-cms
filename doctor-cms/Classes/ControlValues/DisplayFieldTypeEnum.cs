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
    public enum DisplayFieldTypeEnum
    {
        [EnumValue(0, "String")]
        StringType = 0,
        [EnumValue(1, "Decimal")]
        DecimalType = 1,
        [EnumValue(2, "Rate")]
        RateType = 2,
        [EnumValue(3, "Date Value")]
        DateValueType = 3,
        [EnumValue(4, "Date")]
        DateType = 4,
        [EnumValue(5, "Integer")]
        IntegerType = 5

    }
}
