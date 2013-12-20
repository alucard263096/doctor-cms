using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunStar_CMS.admin.Classes.Utils;
using SunStar_CMS.admin.Classes.ControlValues;
using SunStar_CMS.admin.Classes.Objects;
using System.Data;
using SunStar_CMS.admin.Classes.Mgr;
using System.Collections;

namespace doctor_cms
{
    public partial class event_list : System.Web.UI.Page
    {
        public const int GET_BY_CRITERIA = 1;
        protected int? functionType;

        protected void Page_Load(object sender, EventArgs e)
        {
            functionType = Master.checkPermission((int)EnumConvertUtils.ToDbValue(FunctionEnum.FunctionGroup.EventMaintenance));
            Master.PageTitle.Text = EnumConvertUtils.ToDisplayValue(FunctionEnum.FunctionGroup.EventMaintenance) + " - Sunstar CMS";

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

                //btnDelete.Attributes.Add("onclick", "Javascript:return confirm('是否确定删除?')");
                setResult(GET_BY_CRITERIA);
            }
            else
            {
                resetResult();
            }
        }

        protected void resetResult()
        {
            SetUcResultHeader();

            object[] array = (new EventMgr()).getEventList((string)ViewState["sql"]);
            ucResult.pDataSet = (DataSet)array[0];
            ucResult.BindData();
        }

        void SetUcResultHeader()
        {
            ucResult.pHeader = "ID,标题,简介,发布日期,状态";
            ucResult.pDBField = "eventId,title,summary,publishedDate,status";
            ucResult.pDisplayType = Convert.ToString((int)DisplayFormatEnum.CenterAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString);
            ucResult.pDetailURL = "event_detail.aspx?id=";
            ucResult.pHyperLinkType = Convert.ToString((int)ListingHyperlinkTypeEnum.URL);
            ucResult.pHyperLinkCol = "1";
            ucResult.pSortingField = "eventId,title,summary,publishedDate,status";
        }
        private void setResult(int value)
        {

            DataSet set = new DataSet();
            object[] array = new object[2];
            EventMgr Mgr = new EventMgr();

            if (Session["search_from_session"] != null && Session["search_to_session"] != null)
            {
                if (Session["search_from_session"].ToString() == "event" && Session["search_from_session"].ToString() == Session["search_to_session"].ToString())
                {
                    if (Session["search_hashtable"] != null)
                    {
                        Session["search_from_session"] = "";
                        Hashtable htSessionCriteria = (Hashtable)Session["search_hashtable"];
                        
                        txtTitle.Text = htSessionCriteria["SearchTitle"].ToString();
                        txtContent.Text = htSessionCriteria["SearchContent"].ToString();
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
                    array = Mgr.getEventList((User)Session["user"],
                        (txtTitle.Text == "") ? null : txtTitle.Text,
                        (txtContent.Text == "") ? null : txtContent.Text,
                        Convert.ToInt32(ddlStatus.SelectedItem.Value));

                    Hashtable htCriteria = new Hashtable();
                    htCriteria.Add("SearchTitle", txtTitle.Text);
                    htCriteria.Add("SearchContent", txtContent.Text);
                    htCriteria.Add("SearchStatus", ddlStatus.SelectedValue);
                    Session["search_hashtable"] = htCriteria;
                    Session["search_from_session"] = "";
                    Session["search_to_session"] = "event";

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
            Response.Redirect("event_detail.aspx", true);
        }


    }
}