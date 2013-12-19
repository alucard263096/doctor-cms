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
    public partial class user_detail : System.Web.UI.Page
    {
        protected int? functionType;

        protected void Page_Load(object sender, EventArgs e)
        {
            functionType = Master.checkPermission((int)EnumConvertUtils.ToDbValue(FunctionEnum.FunctionGroup.UserMaintenance));
            Master.PageTitle.Text = EnumConvertUtils.ToDisplayValue(FunctionEnum.FunctionGroup.UserMaintenance) + " - Sunstar CMS";
            if (functionType == null)
            {
                Session["message"] = (new Message()).getMessage("Error", "对不起，你没有权限");
                Response.Redirect("home.aspx");
            }

            UserMgr userMgr = new UserMgr();

            ucblAccessRight.Data = (new FunctionMgr()).getAccessFunction();
            ucblAccessRight.IDName = "function_id";
            ucblAccessRight.DisplayName = "function_access";
            ucblAccessRight.BindData();

            if (!string.IsNullOrEmpty(Request["id"]))
            {
                #region Reset Button
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.CssClass = "text";
                cell.Text = "&nbsp;";
                row.Controls.Add(cell);
                cell = new TableCell();
                Button btnResetPassword = new Button();
                btnResetPassword.ID = "btnResetPassword";
                btnResetPassword.CssClass = "button1";
                btnResetPassword.Text = "Reset Password";
                btnResetPassword.Attributes.Add("onclick", "Javascript:return confirm('Are you sure to reset the password?')");
                btnResetPassword.Click += new EventHandler(btnResetPassword_Click);
                cell.Controls.Add(btnResetPassword);
                row.Controls.Add(cell);
                cell = new TableCell();
                cell.ColumnSpan = 2;
                cell.CssClass = "text";
                cell.Text = "&nbsp;";
                row.Controls.Add(cell);
                phResetPassword.Controls.Add(row);
                #endregion
            }

            if (!Page.IsPostBack)
            {
                #region DropDownList
                /****************DropDownlist of Status****************/
                BidirHashtable<object, EnumValueAttribute> statusMap = EnumConvertUtils.EnumToAttributeMap(typeof(RecordStatusEnum));
                foreach (string status in Enum.GetNames(typeof(RecordStatusEnum)))
                {
                    ddlStatus.Items.Add(new ListItem((string)statusMap[Enum.Parse(typeof(RecordStatusEnum), status)].DisplayValue, Convert.ToString((int)statusMap[Enum.Parse(typeof(RecordStatusEnum), status)].DbValue)));
                }

                /****************DropDownlist of Access Right****************/
                DataSet dsAccess = (DataSet)userMgr.getUserList((User)Session["user"], null, null, Convert.ToString((int)EnumConvertUtils.ToDbValue(RecordStatusEnum.Active)))[0];
                ddlAccessRight.DataSource = dsAccess;
                ddlAccessRight.DataTextField = "user_name";
                ddlAccessRight.DataValueField = "user_id";
                ddlAccessRight.DataBind();
                ddlAccessRight.Items.Insert(0, new ListItem("--Select--", "--Select--"));

                #endregion

                //btnSave.Attributes.Add("onclick", "javascript:with(document.getElementById('" + txtEmail.ClientID + "')){if(isEmpty(value) || !isEmailAddress(value)){alert('Please input a valid email address');return false;}}");
                //btnSaveNext.Attributes.Add("onclick", "javascript:with(document.getElementById('" + txtEmail.ClientID + "')){if(isEmpty(value) || !isEmailAddress(value)){alert('Please input a valid email address');return false;}}");
                txtEmail.Attributes.Add("onblur", "javascript:isFormValidEmail(this,true);");


                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    txtUserName.Focus();

                    #region Set User Data
                    /***************Set User Data****************/
                    object user = userMgr.getUser(Convert.ToInt32(Request["id"]));
                    if (user.GetType() == typeof(int) && (int)user == UserMgr.USER_NOT_EXISTS)
                    {
                        Session["message"] = "<font color=\"red\">Error exists.<br />User ID " + Request["id"] + " does not exists</font>";
                        Response.Redirect("~/user_list.aspx");
                    }
                    else
                    {
                        btnSaveNext.Visible = false;
                        lblTitle.Text += "  编辑";

                        txtLoginID.Text = ((User)user).LoginID;
                        txtLoginID.Enabled = false;
                        txtUserName.Text = ((User)user).UserName;
                        txtEmail.Text = ((User)user).Email;
                        txtRemarks.Text = ((User)user).Remarks;
                        ddlStatus.SelectedValue = Convert.ToString(((User)user).Status);

                        #region Access Right
                        ArrayList access = new ArrayList();
                        foreach (object item in ((User)user).UserFunction)
                        {
                            if (item.GetType() == typeof(int))
                            {
                                access.Add(Convert.ToString((int)item));
                            }
                        }
                        ucblAccessRight.CheckedList = access;
                        #endregion
                    }
                    #endregion

                }
                else
                {
                    txtLoginID.Focus();
                    lblTitle.Text += "  新增";
                    btnNew.Visible = false;
                }
            }
        }

        void btnResetPassword_Click(object sender, EventArgs e)
        {
            object user = (new UserMgr()).getUser(Convert.ToInt32(Request["id"]));
            if (user.GetType() == typeof(int) && (int)user == UserMgr.USER_NOT_EXISTS)
            {
                Master.lblWarning.Text = "<font color=\"red\">Error exists.<br />User " + Request["id"] + " does not exists</font>";
            }
            else
            {
                (new UserMgr()).resetPasswd((User)Session["user"], Convert.ToInt32(Request["id"]), ((User)user).Type);
                Master.lblWarning.Text = "<font color=\"blue\">" + ((User)user).UserName + "'s password have been reset</font>";
            }
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            if (ddlAccessRight.SelectedValue != "--Select--")
            {
                UserMgr userMgr = new UserMgr();

                DataSet set = userMgr.getAssignedRight((User)Session["user"], ddlAccessRight.SelectedValue);
                foreach (DataRow row in set.Tables["tb_user_function"].Rows)
                {
                    list.Add(Convert.ToString((int)row["function_id"]));
                }
            }
            ucblAccessRight.CheckedList = list;
        }

        protected void btnQuit_Click(object sender, EventArgs e)
        {
            Session["search_from_session"] = "user";
            Response.Redirect("user_list.aspx", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            object result = save();
            if (result != null)
            {
                Session["message"] = (new Message()).getInformation("added");
                Response.Redirect("user_detail.aspx?id=" + Convert.ToString((int)result));
            }
            else
            {
               // Response.Write("asdasdasd");
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("user_detail.aspx", true);
        }

        protected void btnSaveNext_Click(object sender, EventArgs e)
        {
            object result = save();
            if (result != null)
            {
                Session["message"] = (new Message()).getInformation("added");
                Response.Redirect("user_detail.aspx");
            }
        }

        private object save()
        {
            Message message = new Message();
            #region Validation
            if (string.IsNullOrEmpty(txtLoginID.Text))
            {
                Master.lblWarning.Text = message.getMessage("Error", "请输入登录名");
                return null;
            }

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                Master.lblWarning.Text = message.getMessage("Error", "请输入用户名");
                return null;
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                Master.lblWarning.Text = message.getMessage("Error", "请输入邮箱地址");
                return null;
            }

            #endregion

            if (string.IsNullOrEmpty(Request["id"]))
            {
                int result = (new UserMgr()).addUser((User)Session["user"], txtLoginID.Text, txtUserName.Text,
                                                    txtEmail.Text, txtRemarks.Text, Convert.ToInt32(ddlStatus.SelectedValue), ucblAccessRight.CheckedList);

                if (result == UserMgr.LOGIN_ID_DUPLICATED)
                {
                    Master.lblWarning.Text = message.getMessage("Error", "此登录名已经被使用");
                }
                else
                {
                    return result;
                }
            }
            else
            {
                (new UserMgr()).editUser((User)Session["user"], Convert.ToInt32(Request["id"]), txtLoginID.Text, txtUserName.Text,
                                                    txtEmail.Text, txtRemarks.Text, Convert.ToInt32(ddlStatus.SelectedValue), ucblAccessRight.CheckedList);
                Master.lblWarning.Text = message.getInformation("saved");
            }

            return null;
        }

    }
}
