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
    public partial class RadioButtonList : System.Web.UI.UserControl
    {
        protected DataSet _data;    //The input DataSet
        protected string _idName;       //The id column name
        protected string _displayName;  //The dispaly column name
        protected string _table;    //The name of DataTable in the DataSet
        protected int _column = 1;      //The number of column in the list
        protected ArrayList _textBox;   //The items'ID that there should be a textbox
        protected int _maxLength = 0;   //The max length of the remark textbox
        protected string _groupName; //The group name of the radio group
        protected string _colWidth = "";


        #region Getter Setter
        /// <summary>
        /// Set or Get the DataSet of the CheckBoxList
        /// </summary>
        public DataSet Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// Set or Get the column name of the record key
        /// </summary>
        public string IDName
        {
            get { return _idName; }
            set { _idName = value; }
        }

        public string ColWidth
        {
            get { return _colWidth; }
            set { _colWidth = value; }
        }

        /// <summary>
        /// Set or Get the group name of the radio button
        /// </summary>
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        /// <summary>
        /// Set or Get the column name of the display value
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        /// <summary>
        /// Set or Get the name of the DataTable
        /// </summary>
        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }

        /// <summary>
        /// Set or Get the number of column in the CheckBoxList
        /// </summary>
        public int Column
        {
            get { return _column; }
            set { _column = value; }
        }

        /// <summary>
        /// Set or Get the max length of the remark textbox
        /// </summary>
        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

        /// <summary>
        /// Set or Get the items'ID that a textbox should appear
        /// </summary>
        public ArrayList TextBox
        {
            get { return _textBox; }
            set { _textBox = value; }
        }

        /// <summary>
        /// Set or Get the items' remarks
        /// </summary>
        public Hashtable CheckedRemarks
        {
            get { return this.getCheckedRemarks(); }
            set { this.setCheckRemarks(value); }
        }

        /// <summary>
        /// Get the value of the checked CheckBox record key
        /// </summary>
        public string CheckedValue
        {
            get { return getCheckedValue(); }
            set { setCheckedValue(value); }
        }

        /// <summary>
        /// Get the ArrayList of the checked items' display value
        /// </summary>
        public string CheckedDisplay
        {
            get { return this.getCheckedDisplay(); }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region JavaScript Clear Remarks
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            sb.AppendLine("function clearRemarks(sampleID){");
            sb.AppendLine("mystr=sampleID.split(\"_\");");
            sb.AppendLine("var tempID = mystr[0];");
            sb.AppendLine("for(i=1;i<mystr.length-1;i++){");
            sb.AppendLine("tempID = tempID + \"_\" + mystr[i];");
            sb.AppendLine("}");
            sb.AppendLine("tempID = tempID + \"_txt\" + mystr[mystr.length-1];");
            sb.AppendLine("if (!document.getElementById(sampleID).checked)");
            sb.AppendLine("document.getElementById(tempID).value='';");
            sb.AppendLine("}");
            sb.AppendLine("</script>");

            Type t = this.GetType();
            if (!Page.ClientScript.IsClientScriptBlockRegistered(t, "ClearRemark"))
                Page.ClientScript.RegisterClientScriptBlock(t, "ClearRemark", sb.ToString());
            #endregion
        }

        public void BindData()
        {
            if (_data != null)
            {
                String tablesName = (_table == null) ? _data.Tables[0].TableName : _table;
                String idColumn = (_idName == null) ? _data.Tables[tablesName].Columns[0].ColumnName : _idName;
                int i = 0;

                if (_data.Tables[tablesName].Rows.Count > 0)
                {
                    foreach (DataRow row in _data.Tables[tablesName].Rows)
                    {
                        if (i % _column == 0)
                        {
                            phCheckBoxList.Controls.Add(new LiteralControl("<tr valign=\"top\">"));
                        }

                        phCheckBoxList.Controls.Add(new LiteralControl("<td valign=\"middle\" class=\"text\" width=\"5\">"));
                        RadioButton chk1 = new RadioButton();
                        chk1.ID = Convert.ToString((int)row[idColumn]);
                        chk1.GroupName = _groupName;
                        phCheckBoxList.Controls.Add(chk1);
                        phCheckBoxList.Controls.Add(new LiteralControl("</td>"));

                        if (_textBox != null && !_textBox.Contains(Convert.ToString((int)row[idColumn])))
                            phCheckBoxList.Controls.Add(new LiteralControl("<td valign=\"middle\" class=\"text\" colspan='2'>"));
                        else
                        {
                            chk1.Attributes.Add("onclick", "javascript:clearRemarks(this.id);");
                            if (_colWidth.Length > 0)
                            {
                                phCheckBoxList.Controls.Add(new LiteralControl("<td valign=\"middle\" width=\"" + _colWidth + "\" class=\"text\">"));
                            }
                            else
                            {
                                phCheckBoxList.Controls.Add(new LiteralControl("<td valign=\"middle\" class=\"text\">"));
                            }
                        }

                        Label lbl1 = new Label();
                        lbl1.ID = "lbl" + Convert.ToString((int)row[idColumn]);
                        lbl1.Text = (_displayName == null) ? "" : row[_displayName].ToString();
                        lbl1.CssClass = "text";
                        phCheckBoxList.Controls.Add(lbl1);
                        phCheckBoxList.Controls.Add(new LiteralControl("</td>"));
                        if (_textBox != null && _textBox.Contains(Convert.ToString((int)row[idColumn])))
                        {
                            phCheckBoxList.Controls.Add(new LiteralControl("<td valign=\"middle\" class=\"text\">"));
                            TextBox txt1 = new TextBox();
                            txt1.ID = "txt" + Convert.ToString((int)row[idColumn]);
                            txt1.MaxLength = _maxLength;
                            txt1.CssClass = "forminput";
                            phCheckBoxList.Controls.Add(txt1);
                            phCheckBoxList.Controls.Add(new LiteralControl("</td>"));
                        }
                        if ((i + 1) % _column == 0 || i == _data.Tables[tablesName].Rows.Count)
                        {
                            phCheckBoxList.Controls.Add(new LiteralControl("</tr>\n"));
                        }
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Recursivly check if the checkbox in the input control
        /// is checked.
        /// </summary>
        /// <param></param>
        /// <returns>An value of string of checked box ID</returns>
        private string getCheckedValue()
        {
            string strReturn = "";
            foreach (Control con1 in phCheckBoxList.Controls)
            {
                if (con1.GetType() == typeof(RadioButton) && ((RadioButton)con1).Checked)
                {
                    strReturn = ((RadioButton)con1).ID;
                }
            }
            return strReturn;
        }

        /// <summary>
        /// To get the Display value of those checked items
        /// </summary>
        /// <returns>value of those checked display value</returns>
        private string getCheckedDisplay()
        {
            string strReturn = "";
            foreach (Control con1 in phCheckBoxList.Controls)
            {
                if (con1.GetType() == typeof(RadioButton) && ((RadioButton)con1).Checked)
                {
                    strReturn = ((Label)phCheckBoxList.FindControl("lbl" + ((RadioButton)con1).ID)).Text;
                }
            }
            return strReturn;
        }

        /// <summary>
        /// To set the CheckBoxs
        /// </summary>
        /// <param name="list">An arraylist of string of checked box ID</param>
        private void setCheckedValue(string list)
        {
            foreach (Control con1 in phCheckBoxList.Controls)
            {
                if (con1.GetType() == typeof(RadioButton))
                {
                    if (((RadioButton)con1).ID == list)
                    {
                        ((RadioButton)con1).Checked = true;
                    }
                    else
                    {
                        ((RadioButton)con1).Checked = false;
                    }
                }
            }
        }

        /// <summary>
        /// To get the remarks
        /// </summary>
        /// <returns>Hashtable with [key=item'sID, value=textbox.Text]</returns>
        private Hashtable getCheckedRemarks()
        {
            Hashtable list = new Hashtable();
            foreach (Control con1 in phCheckBoxList.Controls)
            {
                if (con1.GetType() == typeof(RadioButton) && ((RadioButton)con1).Checked)
                {
                    if ((TextBox)phCheckBoxList.FindControl("txt" + ((RadioButton)con1).ID) != null)
                    {
                        string remark = ((TextBox)phCheckBoxList.FindControl("txt" + ((RadioButton)con1).ID)).Text;
                        list.Add(((RadioButton)con1).ID, remark);
                    }
                    else
                    {
                        list.Add(((RadioButton)con1).ID, "");
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// To set the remark
        /// </summary>
        /// <param name="list">Hashtable with [key=item'sID, value=textbox.Text]</param>
        private void setCheckRemarks(Hashtable list)
        {
            foreach (object key in list.Keys)
            {
                if ((TextBox)phCheckBoxList.FindControl("txt" + (string)key) != null)
                {
                    ((TextBox)phCheckBoxList.FindControl("txt" + (string)key)).Text = (string)list[key];
                }
            }
        }
    }
}