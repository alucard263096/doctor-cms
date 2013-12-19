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
using SunStar_CMS.admin.Classes.Mgr;
using SunStar_CMS.admin.Classes.Objects;
using SunStar_CMS.admin.Classes.Utils;
using SunStar_CMS.admin.Classes.ControlValues;

namespace SunStar_CMS.admin
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblSiteName.Text = ConfigurationManager.AppSettings["SiteName"];
            if (Session != null && Session["user"] != null)
            {
                txtUsername.Text = ((User)Session["user"]).LoginID;
            }
            txtUsername.Focus();
            if (!Page.IsPostBack)
            {
                lblCapsLock.Style.Add("visibility", "hidden");
                mskPassword.Attributes.Add("onkeypress", "javascript:if(capLock(event)){document.getElementById('" + lblCapsLock.ClientID + "').style.visibility = 'visible';}else{document.getElementById('" + lblCapsLock.ClientID + "').style.visibility = 'hidden';}");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Equals(""))
            {
                txtUsername.Text = "请输入用户名";
                return;
            }
            else
            {
                UserMgr userMgr = new UserMgr();

                object user = userMgr.getUser(txtUsername.Text, mskPassword.Text);
                lblError.Text ="";
                if (user.GetType() == typeof(int))
                {
                    switch ((int)user)
                    {
                        case UserMgr.USER_NOT_EXISTS:
                            lblError.Text = "对不起，用户不存在";
                            break;
                        case UserMgr.INACTIVE_USER:
                            lblError.Text = "对不起，用户无效";
                            break;
                        case UserMgr.WRONG_PASSWORD:
                            lblError.Text = "密码错误";
                            break;
                        default:
                            lblError.Text = "系统错误";
                            break;
                    }
                }
                else
                {
                    Session.Add("user", (User)user);
                    Response.Redirect("home.aspx");
                }
            }
        }
    }
}
