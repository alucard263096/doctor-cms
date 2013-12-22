using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunStar_CMS.admin.Classes.Utils;
using SunStar_CMS.admin.Classes.ControlValues;
using SunStar_CMS.admin.Classes.Objects;
using SunStar_CMS.admin.Classes.Mgr;
using doctor_cms.Classes.Objects;

namespace doctor_cms
{
    public partial class event_detail : System.Web.UI.Page
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

            EventMgr Mgr = new EventMgr();

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
                    txtTitle.Focus();

                    #region Set User Data
                    /***************Set User Data****************/
                    object oe = Mgr.getEventObject(Convert.ToInt32(Request["id"]));
                    if (oe == null)
                    {
                        Session["message"] = "<font color=\"red\">Error exists.<br />Event ID " + Request["id"] + " does not exists</font>";
                        Response.Redirect("~/event_list.aspx");
                    }
                    else
                    {
                        btnSaveNext.Visible = false;
                        lblTitle.Text += "  编辑";
                        Event eve = (Event)oe;
                        txtTitle.Text = eve.Title;
                        txtSummary.Text = eve.Summary;
                        txtContent.Text = eve.Content;
                        txtPublishedDate.Text = eve.PublishedDate.ToString("yyyy-MM-dd HH:mm:ss");
                        imgurl.ImageUrl = eve.ImageUrl;
                        ddlStatus.SelectedValue = eve.Status.ToString();
                    }
                    #endregion

                }
                else
                {
                    txtTitle.Focus();
                    txtPublishedDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    lblTitle.Text += "  新增";
                    btnNew.Visible = false;
                }
            }
        }
        protected void btnQuit_Click(object sender, EventArgs e)
        {
            Session["search_from_session"] = "event";
            Response.Redirect("event_list.aspx", true);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            object result = save();
            if (result != null)
            {
                Session["message"] = (new Message()).getInformation("added");
                Response.Redirect("event_detail.aspx?id=" + Convert.ToString((int)result));
            }
            else
            {
                // Response.Write("asdasdasd");
            }
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("event_detail.aspx", true);
        }

        protected void btnSaveNext_Click(object sender, EventArgs e)
        {
            object result = save();
            if (result != null)
            {
                Session["message"] = (new Message()).getInformation("added");
                Response.Redirect("event_detail.aspx");
            }
        }

        private object save()
        {
            Message message = new Message();
            #region Validation


            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                Master.lblWarning.Text = message.getMessage("Error", "请输入活动标题");
                return null;
            }
            if (string.IsNullOrEmpty(txtSummary.Text))
            {
                Master.lblWarning.Text = message.getMessage("Error", "请输入活动简介");
                return null;
            }
            if (string.IsNullOrEmpty(txtContent.Text))
            {
                Master.lblWarning.Text = message.getMessage("Error", "请输入活动内容");
                return null;
            }
            if (string.IsNullOrEmpty(txtPublishedDate.Text))
            {
                Master.lblWarning.Text = message.getMessage("Error", "请输入发布时间");
                return null;
            }
            try
            {
                DateTime pbt = Convert.ToDateTime(txtPublishedDate.Text);
            }
            catch
            {
                Master.lblWarning.Text = message.getMessage("Error", "输入的发布时间不是一个正确的时间格式");
                return null;
            }
            #endregion

            #region get Type object


            Event eve = new Event();
            eve.Title = txtTitle.Text;
            eve.Summary = txtSummary.Text;
            eve.Content = txtContent.Text;
            eve.PublishedDate =Convert.ToDateTime( txtPublishedDate.Text);

            eve.ImageUrl = imgurl.ImageUrl;
            if (imageupload.HasFile)
            {
                UploadFile uf = new UploadFile();
                eve.ImageUrl = "upload\\" + uf.SaveFile(Server.MapPath("./") + "upload/", imageupload, "event_" + DateTime.Now.Ticks.ToString() + "_" + (new Random()).Next(10000), true);
            }

            if (string.IsNullOrEmpty(eve.ImageUrl))
            {
                Master.lblWarning.Text = message.getMessage("Error", "请上传图片");
                return null;
            }
            eve.Status = Convert.ToInt32(ddlStatus.SelectedValue);


            #endregion

            if (string.IsNullOrEmpty(Request["id"]))
            {
                int result = (new EventMgr()).addEvent((User)Session["user"], eve);


                return result;
            }
            else
            {
                eve.EventId = Convert.ToInt32(Request["id"]);

                (new EventMgr()).editEvent((User)Session["user"], eve);
                Master.lblWarning.Text = message.getInformation("saved");
            }

            return null;
        }

    }
}