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
    public partial class type_detail : System.Web.UI.Page
    {
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

            TypeMgr Mgr = new TypeMgr();

            if (!Page.IsPostBack)
            {
                #region DropDownList
                /****************DropDownlist of Status****************/
                BidirHashtable<object, EnumValueAttribute> statusMap = EnumConvertUtils.EnumToAttributeMap(typeof(RecordStatusEnum));
                foreach (string status in Enum.GetNames(typeof(RecordStatusEnum)))
                {
                    ddlStatus.Items.Add(new ListItem((string)statusMap[Enum.Parse(typeof(RecordStatusEnum), status)].DisplayValue, Convert.ToString((int)statusMap[Enum.Parse(typeof(RecordStatusEnum), status)].DbValue)));
                }
                #endregion



                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    txtChnName.Focus();

                    #region Set User Data
                    /***************Set User Data****************/
                    object type = Mgr.getTypeObject(Convert.ToInt32(Request["id"]));
                    if (type==null)
                    {
                        Session["message"] = "<font color=\"red\">Error exists.<br />Type ID " + Request["id"] + " does not exists</font>";
                        Response.Redirect("~/admin/type_list.aspx");
                    }
                    else
                    {
                        btnSaveNext.Visible = false;
                        lblTitle.Text += "  编辑";
                        ObjType objtype = (ObjType)type;

                        txtChnName.Text = objtype.ChnName;
                        txtEngName.Text = objtype.EngName;
                        txtRemarks.Text = objtype.Remarks;
                        ddlStatus.SelectedValue = objtype.Status.ToString();
                    }
                    #endregion

                }
                else
                {
                    txtChnName.Focus();
                    lblTitle.Text += "  新增";
                    btnNew.Visible = false;
                }
            }
        }


        protected void btnQuit_Click(object sender, EventArgs e)
        {
            Session["search_from_session"] = "type";
            Response.Redirect("type_list.aspx", true);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            object result = save();
            if (result != null)
            {
                Session["message"] = (new Message()).getInformation("added");
                Response.Redirect("type_detail.aspx?id=" + Convert.ToString((int)result));
            }
            else
            {
                // Response.Write("asdasdasd");
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("type_detail.aspx", true);
        }

        protected void btnSaveNext_Click(object sender, EventArgs e)
        {
            object result = save();
            if (result != null)
            {
                Session["message"] = (new Message()).getInformation("added");
                Response.Redirect("type_detail.aspx");
            }
        }

        private object save()
        {
            Message message = new Message();
            #region Validation


            if (string.IsNullOrEmpty(txtChnName.Text))
            {
                Master.lblWarning.Text = message.getMessage("Error", "请输入类型中文名");
                return null;
            }

            #endregion

            #region get Type object

            ObjType objtype = new ObjType();
            objtype.ChnName = txtChnName.Text;
            objtype.EngName = txtEngName.Text;
            objtype.Remarks = txtRemarks.Text;
            objtype.Status = Convert.ToInt32(ddlStatus.SelectedValue);


            #endregion

            if (string.IsNullOrEmpty(Request["id"]))
            {
                int result = (new TypeMgr()).addType((User)Session["user"], objtype);


                return result;
            }
            else
            {
                objtype.TypeId = Convert.ToInt32(Request["id"]);

                (new TypeMgr()).editType((User)Session["user"], objtype);
                Master.lblWarning.Text = message.getInformation("saved");
            }

            return null;
        }
    }
}
