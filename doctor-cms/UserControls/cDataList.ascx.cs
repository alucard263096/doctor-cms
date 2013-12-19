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

namespace SunStar_CMS.admin.UserControls
{
    public partial class cDataList : System.Web.UI.UserControl
    {
        private String[] head; 
        private String[] body;
        private String values;
        private DataSet dataSet;
        private String headStyle;
        private int index;
        private int column;
        private String columnName;
        private String planId;
        private String url;
        public String Url
        {
            get { return url; }
            set { url = value; }
        }

        public String PlanId
        {
            get { return planId; }
            set { planId = value; }
        }
        public String HeadStyle
        {
            get { return headStyle; }
            set { headStyle = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Session.Add("ds", dataSet);
            else
                dataSet = (DataSet)Session["ds"];
        }
        public void setColumnName(String name)
        {
            this.columnName = name;
        }
        public void setColumn(int c)
        {
            this.column = c;
        }
        public void setTitle(String titl)
        {
            title.Text=titl;
        }
        public String[] Body
        {
            get { return body; }
            set { body = value; }
        }
        public String[] Head
        {
            get { return head; }
            set { head = value; }
        }
        public String Values
        {
            get { return values; }
            set { values = value; }
        }
        public DataSet DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }
        public void setIndex(int i)
        {
            this.index = i;
        }
        public void addHead(String[] h)
        {
            if (h.Length > 0)
            {
                phHeader.Controls.Add(new LiteralControl("<tr class=\""+headStyle+"\">"));
                for(int i=0;i<=h.Length+1;i++)
                {
                    if(i<h.Length)
                     phHeader.Controls.Add(new LiteralControl("<td style='width:50px'>"+h[i]+"</td>"));
                  //  Response.Write(h[i]);
                }
                phHeader.Controls.Add(new LiteralControl("</tr>"));

            }
        }
        public void listData(DataSet ds, String[] b,int t)
        {
            if (head.Length != b.Length)
                throw new Exception("error");
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    String bg = (i % 2 == 0) ? "#E6F4CE" : "#FFFFFF";
                    phBody.Controls.Add(new LiteralControl("<tr onmouseover=\" mouseOver(this)\" onmouseout=\"mouserOut(this)\"  style=\"background-color:" + bg + "\">"));
                    for (int j = 0; j <=b.Length+1; j++)
                    {  
                        phBody.Controls.Add(new LiteralControl("<td>"));
                        if (j ==column)
                        {
                            TextBox textbox = new TextBox();
                            textbox.ID = ds.Tables[0].Rows[i][index].ToString();
                            textbox.Text = ds.Tables[0].Rows[i][columnName].ToString();
                            textbox.Attributes.Add("onchange", "show(this)");
                            phBody.Controls.Add(textbox);
                        }
                        else if (j == b.Length)
                        {

                        }
                        else if (j == b.Length + 1)
                        {
                            String image = "";
                            if (ds.Tables[0].Rows[i][columnName].ToString() == "" || ds.Tables[0].Rows[i][columnName].ToString().Length < 1 || ds.Tables[0].Rows[i][columnName].ToString() == null)
                                image += "<a href=\""+url+"?factorId="+ds.Tables[0].Rows[i][index].ToString()+"&planId="+planId+"\"><img src=\"image/adddoc.ico\"/></a>";
                            else
                                image += "<a href=\"" + url +"?factorId="+ds.Tables[0].Rows[i][index].ToString()+"&planId="+planId+ "\"><img src=\"image/editdoc.ico\"/></a>";
                            phBody.Controls.Add( new LiteralControl(image));
                        }
                        else
                        {
                            Label label = new Label();
                            label.Text = ds.Tables[0].Rows[i][b[j]].ToString() + ":    ";
                            phBody.Controls.Add(label);

                        }
                        phBody.Controls.Add(new LiteralControl("</td>"));
                    }
                    phBody.Controls.Add(new LiteralControl("</tr>"));
                }
            }                 
        }
        public String getValues()
        {
            int count = dataSet.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                values += ((TextBox)phBody.FindControl(dataSet.Tables[0].Rows[i][index].ToString())).ID + ":" + ((TextBox)phBody.FindControl(dataSet.Tables[0].Rows[i][index].ToString())).Text + ",";
            }
            return this.values;
        }
        public void databind()
        {
            addHead(head);
            listData(dataSet, body, index);
        }
    }
}