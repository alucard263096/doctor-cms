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
using SunStar_CMS.admin.Classes.ControlValues;
using SunStar_CMS.admin.Classes.Mgr;
using SunStar_CMS.admin.Classes.Objects;
using SunStar_CMS.admin.Classes.Utils;

namespace SunStar_CMS.admin
{
    public partial class type_list : System.Web.UI.Page
    {
        public const int GET_BY_CRITERIA = 1;
        protected int? functionType;



        protected void Page_Load(object sender, EventArgs e)
        {
            functionType = Master.checkPermission((int)EnumConvertUtils.ToDbValue(FunctionEnum.FunctionGroup.TypeMaintenance));
            Master.PageTitle.Text = EnumConvertUtils.ToDisplayValue(FunctionEnum.FunctionGroup.TypeMaintenance) + " - Sunstar CMS";

            if (functionType == null)
            {
                Session["message"] = (new Message()).getMessage("Error", "对不起，你没有权限");
                Response.Redirect("home.aspx");
            }
            if (!Page.IsPostBack)
            {
                /****************DropDownlist of Status****************/
                BidirHashtable<object, EnumValueAttribute> statusMap = EnumConvertUtils.EnumToAttributeMap(typeof(RecordStatusEnum));
                foreach (string status in Enum.GetNames(typeof(RecordStatusEnum)))
                {
                    ddlStatus.Items.Add(new ListItem((string)statusMap[Enum.Parse(typeof(RecordStatusEnum), status)].DisplayValue, Convert.ToString((int)statusMap[Enum.Parse(typeof(RecordStatusEnum), status)].DbValue)));
                }

                btnDelete.Attributes.Add("onclick", "Javascript:return confirm('是否确定删除?')");
                setResult(GET_BY_CRITERIA);
            }
            else
            {
                resetResult();
            }
        }

        void SetUcResultHeader()
        {
            ucResult.pHeader = "ID,中文名称,英文名称,状态";
            ucResult.pDBField = "type_id,chn_name,eng_name,status";
            ucResult.pDisplayType = Convert.ToString((int)DisplayFormatEnum.CenterAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString);
            ucResult.pDetailURL = "type_detail.aspx?id=";
            ucResult.pHyperLinkType = Convert.ToString((int)ListingHyperlinkTypeEnum.URL);
            ucResult.pHyperLinkCol = "1";
            ucResult.pSortingField = "type_id,chn_name,eng_name,status";
        }

        protected void resetResult()
        {
            SetUcResultHeader();

            object[] array = (new TypeMgr()).getTypeList((string)ViewState["sql"]);
            ucResult.pDataSet = (DataSet)array[0];
            ucResult.BindData();
        }

        private void setResult(int value)
        {

            DataSet set = new DataSet();
            object[] array = new object[2];
            TypeMgr Mgr = new TypeMgr();

            if (Session["search_from_session"] != null && Session["search_to_session"] != null)
            {
                if (Session["search_from_session"].ToString() == "type" && Session["search_from_session"].ToString() == Session["search_to_session"].ToString())
                {
                    if (Session["search_hashtable"] != null)
                    {
                        Session["search_from_session"] = "";
                        Hashtable htSessionCriteria = (Hashtable)Session["search_hashtable"];
                        
                        txtEngName.Text = htSessionCriteria["SearchEngName"].ToString();
                        txtChnName.Text = htSessionCriteria["SearchChnName"].ToString();
                        ddlStatus.SelectedValue = htSessionCriteria["SearchStatus"].ToString();

                    }
                }
            }

            switch (value)
            {
                /*case GET_ALL:
                    set = userMgr.getUserList((User)Session["user"], null, null, null, null);
                    break;*/
                case GET_BY_CRITERIA:
                    array = Mgr.getTypeList((User)Session["user"],
                        (txtEngName.Text == "") ? null : txtEngName.Text,
                        (txtChnName.Text == "") ? null : txtChnName.Text,
                        (string)ddlStatus.SelectedItem.Value);

                    Hashtable htCriteria = new Hashtable();
                    htCriteria.Add("SearchEngName", txtEngName.Text);
                    htCriteria.Add("SearchChnName", txtChnName.Text);
                    htCriteria.Add("SearchStatus", ddlStatus.SelectedValue);
                    Session["search_hashtable"] = htCriteria;
                    Session["search_from_session"] = "";
                    Session["search_to_session"] = "type";

                    break;
                default:
                    ((Label)Master.FindControl("lblWarning")).Text = "出现错误,请重新尝试";
                    break;
            }


            SetUcResultHeader();

            ucResult.pDataSet = (DataSet)array[0];
            ucResult.BindData();
            ViewState["sql"] = (string)array[1];

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Session["search_from_session"] = "";
            this.setResult(GET_BY_CRITERIA);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("type_detail.aspx", true);

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (ucResult.checkedList.ToString().Trim().Length > 0)
            {
                TypeMgr Mgr = new TypeMgr();
                Message message = new Message();
                DataSet set = (DataSet)Mgr.getTypeList((User)Session["user"], null, null, null)[0];
                ArrayList result = Mgr.deleteType(ucResult.checkedList);

                if (result.Count == 0)
                {
                    Master.lblWarning.Text = message.getMessage("Information", "Type are deleted");
                }

                setResult(GET_BY_CRITERIA);
            }
        }
    }
}
