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
using SunStar_CMS.admin.Classes.ControlValues;
using SunStar_CMS.admin.Classes.Utils;

namespace SunStar_CMS.admin
{
    public partial class Base1 : System.Web.UI.MasterPage
    {
        DataSet functionSet = (new FunctionMgr()).getFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblSiteName.Text = ConfigurationManager.AppSettings["SiteName"];
            Page.MaintainScrollPositionOnPostBack = true;
            Message message = new Message();

            if (Session != null && Session["user"] != null)
            {
                lblUser.Text = ((User)Session["user"]).UserName;
                lblWarning.Text = message.getMessage("default");
                if (Session["message"] != null)
                {
                    lblWarning.Text = (string)Session["message"];
                    Session.Remove("message");
                }
                setMenu();
            }
            else
            {
                lblWarning.Text = message.getError("sessionExpired");
                lblUser.Visible = false;
                lblScreenNo.Visible = false;
                phMainContent.Visible = false;
                phMenu.Visible = false;
            }

            ScreenNo screenNo = new ScreenNo();
            lblScreenNo.Text = Convert.ToString(screenNo.getSceenNo(Request.Url.LocalPath.Substring(1)));
            #region HistoryForward
            if (ConfigurationManager.AppSettings["Mode"] != "debug")
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
                sb.AppendLine("window.history.forward(1);");
                sb.AppendLine("</script>");

                Type t = this.GetType();
                if (!Page.ClientScript.IsClientScriptBlockRegistered(t, "HistoryForward"))
                    Page.ClientScript.RegisterClientScriptBlock(t, "HistoryForward", sb.ToString());
            }
            #endregion

        }

        /// <summary>
        /// To check the permission of a function
        /// </summary>
        /// <param name="functionGroup">The function group of the function</param>
        /// <returns>return null if there is no access right for the function 
        /// else function_type will be returned</returns>
        public int? checkPermission(int functionGroup)
        {
            ArrayList functionType = new ArrayList();
            if (Session["user"] == null)
            {
                string str = "what the fuck";
            }
            if (Session != null && Session["user"] != null)
                foreach (DataRow row in functionSet.Tables["tb_function"].Rows)
                {
                    if ((int)row["function_group"] == functionGroup)
                    {
                        if (((User)Session["user"]).UserFunction.Contains(row["function_id"]))
                        {
                            functionType.Add((Int16)row["function_type"]);
                        }
                    }
                }

            if (functionType.Count > 0)
            {
                functionType.Sort();
                return (Int16?)functionType[functionType.Count - 1];
            }
            else
                return null;
        }

        /// <summary>
        /// To set the left hand side menu
        /// </summary>
        protected void setMenu()
        {
            int intPreviousGroup = -1;
            foreach (DataRow row in functionSet.Tables["tb_function"].Rows)
            {
                if (((User)Session["user"]).UserFunction.Contains(row["function_id"]) && (int)row["parent_id"] == 0 && intPreviousGroup != (int)row["function_group"])
                {
                    intPreviousGroup = (int)row["function_group"];
                    TableRow trow = new TableRow();
                    TableCell tcell = new TableCell();
                    tcell.Style.Add("width", "120");
                    tcell.Text = "<a href='" + (string)row["function_link"] + "' class='menulink'>" + (string)row["function_name"] + "</a>";
                    trow.Cells.Add(tcell);
                    phMenu.Controls.Add(trow);
                }
            }
            TableRow trow1 = new TableRow();
            phMenu.Controls.Add(trow1);
            trow1 = new TableRow();
            TableCell tcell1 = new TableCell();
            tcell1.Style.Add("width", "120");
            tcell1.Text = "<a href='password.aspx' class='menulink'>¸ü¸ÄÃÜÂë</a>";
            trow1.Cells.Add(tcell1);
            phMenu.Controls.Add(trow1);
        }

        /// <summary>
        /// To logout the session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogout_Click(object sender, EventArgs e)
        {

            string langue = (string)Session["langue"];
            Session.Clear();
            Session["langue"] = langue;
            Session.Abandon();
            Response.Redirect("login.aspx");
        }


        protected void Button1_Click1(object sender, EventArgs e)
        {
         //   Response.Redirect("family_parameter.aspx?plan_id=6&cat_id=3&operate=new&option_id=1");
        }
    }
}
