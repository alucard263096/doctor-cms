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
using SunStar_CMS.admin.UserControls;

namespace SunStar_CMS.admin
{
    public partial class user_list : System.Web.UI.Page
    {
        public const int GET_BY_CRITERIA = 1;
        protected int? functionType;

        protected void Page_Load(object sender, EventArgs e)
        {
            functionType = Master.checkPermission((int)EnumConvertUtils.ToDbValue(FunctionEnum.FunctionGroup.UserMaintenance));
            Master.PageTitle.Text =EnumConvertUtils.ToDisplayValue( FunctionEnum.FunctionGroup.UserMaintenance)+" - Sunstar CMS";


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

        private void setResult(int value)
        {
            DataSet set = new DataSet();
            object[] array = new object[2];
            UserMgr userMgr = new UserMgr();

            if (Session["search_from_session"] != null && Session["search_to_session"] != null)
            {
                if (Session["search_from_session"].ToString() == "user" && Session["search_from_session"].ToString() == Session["search_to_session"].ToString())
                {
                    if (Session["search_hashtable"] != null)
                    {
                        Session["search_from_session"] = "";
                        Hashtable htSessionCriteria = (Hashtable)Session["search_hashtable"];
                        txtLoginID.Text = htSessionCriteria["SearchLoginID"].ToString();
                        txtUserName.Text = htSessionCriteria["SearchUserName"].ToString();
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
                    array = userMgr.getUserList((User)Session["user"],
                        (txtLoginID.Text == "") ? null : txtLoginID.Text,
                        (txtUserName.Text == "") ? null : txtUserName.Text,
                        (string)ddlStatus.SelectedItem.Value);

                    Hashtable htCriteria = new Hashtable();
                    htCriteria.Add("SearchLoginID", txtLoginID.Text);
                    htCriteria.Add("SearchUserName", txtUserName.Text);
                    htCriteria.Add("SearchStatus", ddlStatus.SelectedValue);
                    Session["search_hashtable"] = htCriteria;
                    Session["search_from_session"] = "";
                    Session["search_to_session"] = "user";

                    break;
                default:
                    ((Label)Master.FindControl("lblWarning")).Text = "出现错误,请重新尝试";
                    break;
            }
            ucResult.pHeader = "UserId,Login ID,User Name,Email,Status";
            ucResult.pDBField = "user_id,login_id,user_name,email,status";
            ucResult.pDisplayType = Convert.ToString((int)DisplayFormatEnum.CenterAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString);
            ucResult.pDetailURL = "user_detail.aspx?id=";
            ucResult.pHyperLinkType = Convert.ToString((int)ListingHyperlinkTypeEnum.URL);
            ucResult.pHyperLinkCol = "1";
            ucResult.pSortingField = "user_id,login_id,user_name,email,status";

            ucResult.pDataSet = (DataSet)array[0];
            ucResult.BindData();
            ViewState["sql"] = (string)array[1];
        }

        protected void resetResult()
        {
            ucResult.pHeader = "UserId,Login ID,User Name,Email,Status";
            ucResult.pDBField = "user_id,login_id,user_name,email,status";
            ucResult.pDisplayType = Convert.ToString((int)DisplayFormatEnum.CenterAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString) + "," +
                                    Convert.ToString((int)DisplayFormatEnum.LeftAlignedString);
            ucResult.pDetailURL = "user_detail.aspx?id=";
            ucResult.pHyperLinkType = Convert.ToString((int)ListingHyperlinkTypeEnum.URL);
            ucResult.pHyperLinkCol = "1";
            ucResult.pSortingField = "user_id,login_id,user_name,email,status";

            object[] array = (new UserMgr()).getUserList((string)ViewState["sql"]);
            ucResult.pDataSet = (DataSet)array[0];
            ucResult.BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Session["search_from_session"] = "";
            this.setResult(GET_BY_CRITERIA);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("user_detail.aspx", true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
             if (ucResult.checkedList.ToString().Trim().Length > 0)
            {
                UserMgr userMgr = new UserMgr();
                Message message = new Message();
                DataSet set = (DataSet)userMgr.getUserList((User)Session["user"], null, null, null)[0];
                ArrayList result = userMgr.deleteUser(ucResult.checkedList);
                if (result.Count == 0)
                {
                    Master.lblWarning.Text = message.getMessage("Information", "Users are deleted");
                }
                else
                {
                    string haveTrans = "";
                    foreach (DataRow row in set.Tables["tb_user"].Rows)
                    {
                        if (result.Contains(Convert.ToString((int)row["user_id"])))
                        {
                            haveTrans += (string)row["user_name"] + ", ";
                        }
                    }

                    Master.lblWarning.Text = message.getMessage("Error", "Users : " + haveTrans.Substring(0, haveTrans.Length - 2) + "<br /> Have transaction cannot be deleted");
                }
                setResult(GET_BY_CRITERIA);
            }

        }
    }
}
