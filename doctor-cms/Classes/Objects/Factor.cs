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
    public class Factor
    {
        int _FactorId;

        public int FactorId
        {
            get { return _FactorId; }
            set { _FactorId = value; }
        }
        int _CategoryId;

        public int CategoryId
        {
            get { return _CategoryId; }
            set { _CategoryId = value; }
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
        decimal _StartValue;

        public decimal StartValue
        {
            get { return _StartValue; }
            set { _StartValue = value; }
        }
        decimal _EndValue;

        public decimal EndValue
        {
            get { return _EndValue; }
            set { _EndValue = value; }
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
        int _Priority;

        public int Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

    }
}
