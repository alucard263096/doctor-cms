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
    public enum DisplayFormatEnum
    {
        [EnumValue(0, "Left Aligned String")]
        LeftAlignedString=0,
        [EnumValue(1, "Left Aligned Decimal")]
        LeftAlignedDecimal = 1,
        [EnumValue(2, "Left Aligned Rate")]
        LeftAlignedRate = 2,
        [EnumValue(3, "Left Aligned Date Value")]
        LeftAlignedDateValue = 3,
        [EnumValue(4, "Left Aligned Date")]
        LeftAlignedDate = 4,
        [EnumValue(5, "Left Aligned Integer")]
        LeftAlignedInteger = 5,
        [EnumValue(10, "Center Aligned String")]
        CenterAlignedString = 10,
        [EnumValue(11, "Center Aligned Decimal")]
        CenterAlignedDecimal = 11,
        [EnumValue(12, "Center Aligned Rate")]
        CenterAlignedRate = 12,
        [EnumValue(13, "Center Aligned Date Value")]
        CenterAlignedDateValue = 13,
        [EnumValue(14, "Center Aligned Date")]
        CenterAlignedDate = 14,
        [EnumValue(15, "Center Aligned Integer")]
        CenterAlignedInteger = 15,
        [EnumValue(20, "Right Aligned String")]
        RightAlignedString = 20,
        [EnumValue(21, "Right Aligned Decimal")]
        RightAlignedDecimal = 21,
        [EnumValue(22, "Right Aligned Rate")]
        RightAlignedRate = 22,
        [EnumValue(23, "Right Aligned Date Value")]
        RightAlignedDateValue = 23,
        [EnumValue(24, "Right Aligned Date")]
        RightAlignedDate = 24,
        [EnumValue(25, "Right Aligned Integer")]
        RightAlignedInteger = 25,

    }
}
