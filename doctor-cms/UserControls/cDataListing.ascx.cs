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

namespace SunStar_CMS.admin.UserControls
{
    public partial class cDataListing : System.Web.UI.UserControl
    {

        // list of header name separated by ,
        public string pHeader = "";
        // list of db field separated by , (must match with pHeader)
        public string pDBField = "";
        // list of display type separated by , (must match with pDBField)
        // 1-left currency; 2-right rate; 3-center date value; 4-center dd/MM/yyyy; 5-center string; 6-rigth number;
        public string pDisplayType = "";
        // widht of each column
        public string pColumnWidth = "";
        // cssclass for heading
        public string pHeaderCssClass = ConfigurationManager.AppSettings["DefaultHeaderCssClass"];
        // cssclass for row
        public string pRowCssClass = ConfigurationManager.AppSettings["DefaultRowCssClass"];
        // highlight row when selected
        public bool pShowCheckedRow = Convert.ToBoolean(ConfigurationManager.AppSettings["DefaultShowCheckedRow"]);
        // checked row color
        public string pCheckedRowColor = ConfigurationManager.AppSettings["DefaultCheckedRowColor"];
        // highlight row when mouseon
        public bool pShowHighlightRow = Convert.ToBoolean(ConfigurationManager.AppSettings["DefaultShowHighlightRow"]);
        // highlighted row color
        public string pHighlightRowColor = ConfigurationManager.AppSettings["DefaultHighlightRowColor"];
        // non-highlighted row color
        public string pRowColor = ConfigurationManager.AppSettings["DefaultRowColor"];
        // cssclass for hyperlink column
        public string pRowHyperCssClass = ConfigurationManager.AppSettings["DefaultRowHyperCssClass"];
        // cssclass for hyperlink column in header
        public string pHeaderHyperCssClass = ConfigurationManager.AppSettings["DefaultHeaderHyperCssClass"];
        // URL for hyperlink when pHyperLinkType = 0
        public string pDetailURL = "";
        // dataset for listing
        public DataSet pDataSet;
        // list of column index which show hyperlink separated by ,
        public string pHyperLinkCol = "0";
        // list of hyperlink type separated by ,
        // 0-URL; 1-RaiseEvent
        public string pHyperLinkType = "0";
        // show checkbox for selection
        public bool pAllowSelection = Convert.ToBoolean(ConfigurationManager.AppSettings["DefaultAllowSelection"]);
        // show sorting in header
        public bool pShowSorting = Convert.ToBoolean(ConfigurationManager.AppSettings["DefaultShowSorting"]);
        // useless
        public string pSortingURL = "";
        // list of db field for sorting separated by , (must match with pDBField)
        public string pSortingField = "";
        //useless
        public string pCheckField = "";
        //useless
        public string pCheckValue = "";
        //current db field for sorting
        public string pCurrentSortField="";

        //identifier for conexist listing
        public string pCheckBoxAttribute = "defaultList";

        //list title
        public string pListTitle = "Search result:";

        //Show total count
        public bool pShowTotalCount = false;

        //Indicate the export format
        public bool pExcelString = false;


        //Show grand total
        public bool pShowPageTotal = false;
        //list of db field to show the total separated by , (must match with pDBField)
        public string pPageTotalField = "";
        //Grand Total Label
        public string pPageTotalLabel = "Page Total";
        // list of display type separated by , (must match with pPageTotalField)
        // 1-left currency; 2-right rate; 6-rigth number;
        public string pPageDisplayType = "";
        // cssclass for page total column
        public string pPageTotalCssClass = ConfigurationManager.AppSettings["DefaultPageCssClass"];
        //list of compute action separated by , (must match with pGrandTotalField)
        public string pPageComputeAction = "";
        
        private string[] arrPageTotalField;
        private string[] arrPageDisplayType;
        private string[] arrPageComputeAction;
        
        //Show grand total
        public bool pShowGrandTotal = false;
        //list of db field to show the total separated by , (must match with pDBField)
        public string pGrandTotalField = "";
        // cssclass for grand total column
        public string pGrandTotalCssClass = ConfigurationManager.AppSettings["DefaultGrandCssClass"];
        //Grand Total Label
        public string pGrandTotalLabel = "Total";
        // list of display type separated by , (must match with pGrandTotalField)
        // 1-left currency; 2-right rate; 6-right number;
        public string pGrandDisplayType = "";
        //list of compute action separated by , (must match with pGrandTotalField)
        public string pGrandComputeAction = "";

        private string[] arrGrandTotalField;
        private string[] arrGrandDisplayType;
        private string[] arrGrandComputeAction;


        private string[] arrDisplayType;
        private string[] arrColumnWidth;
        private string[] arrHyperLinkType;
        private string[] arrHyperLinkCol;

        //cannot include '_' character
        private string strHyperLinkPrefix = "Link-";
        private string strHyperLinkIdx = "_";
        private int _intSelectID = 0;

        private string strSortLinkPrefix = "sort*";
        private string strSortLinkIdx = "*";

        public int pPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
        public bool pShowPaging = Convert.ToBoolean(ConfigurationManager.AppSettings["DefaultShowPaging"]);
        private int intStarRow = 0;
        private int intPage = 0;
        private int intTotalPage = 0;
//        private bool doSorting = false;

        private DataRow[] drRecords;

        private DataSet dsPageData;

        public int intSelectID
        {
            get { return _intSelectID; }
            set { _intSelectID = value; }
        }

        public delegate void URLClickEventHandler(object sender, URLClickEventArgs e);
        public event URLClickEventHandler URLClickEvent;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (pAllowSelection)
            {
                txtSelectedCount.Visible = true;
                lblTotalCount.Visible = false;
            }
            else
            {
                txtSelectedCount.Visible = false;
                lblTotalCount.Visible = pShowTotalCount;
                if (pDataSet != null)
                {
                    lblTotalCount.Text = pDataSet.Tables[0].Rows.Count.ToString();
                }
                else
                {
                    lblTotalCount.Text = "0";
                }
            }
            if (!Page.IsPostBack)
            {
                ResetPageNumber();
            }else{
                intPage = Convert.ToInt32(txtCurrentPage.Value);
            }
            lblListTitle.Text = pListTitle;
        }

        protected void AddHeader()
        {
            string[] arrHeader = pHeader.ToString().Split(',');
            string[] arrField;

            if (pShowSorting)
            {
                arrField = pSortingField.ToString().Split(',');
            }
            else
            {
                pSortingField = " , ";
                arrField = pSortingField.ToString().Split(',');
            }

            phHeader.Controls.Add(new LiteralControl("<tr class=\"" + pHeaderCssClass + "\">"));
            for (int i = 0; i < arrHeader.Length; i++)
            {
                string strAlign = "";
                if (Convert.ToInt32(arrDisplayType[i]) < 10)
                {
                    strAlign = "Left";
                }
                else if (Convert.ToInt32(arrDisplayType[i]) < 20)
                {
                    strAlign = "Center";
                }
                else
                {
                    strAlign = "Right";
                }
                if (i == 0)
                {
                    if (pAllowSelection)
                    {
                        phHeader.Controls.Add(new LiteralControl("<th class=\"" + strAlign + "_" + pHeaderCssClass + "\" align='left'>"));

                        CheckBox cb = new CheckBox();
                        cb.ID = pCheckBoxAttribute + "*" + "all_id";
                        cb.Attributes.Add(pCheckBoxAttribute, "");
                        if (pShowCheckedRow)
                        {
                            cb.Attributes.Add("onclick", "onCheckAllWithHightLight(this,'" + pCheckedRowColor + "','" + pHighlightRowColor + "','" + pRowColor + "','" + pCheckBoxAttribute + "');");
                        }
                        else
                        {
                            cb.Attributes.Add("onClick", "onCheckAllWithCount(this,'" + pCheckBoxAttribute + "');");
                        }
                        phHeader.Controls.Add(cb);
                        phHeader.Controls.Add(new LiteralControl("</th>"));
                    }
                
                }
                else
                {
                    if (!pShowSorting)
                    {
                        phHeader.Controls.Add(new LiteralControl("<th class=\"" + strAlign + "_" + pHeaderCssClass + "\" align='left'>" + arrHeader[i] + "</th>"));
                    }
                    else
                    {
                        //HyperLink hlHref = new HyperLink();
                        //hlHref.ID = "sort_" + i;
                        //hlHref.Text = arrHeader[i].ToString();
                        //if (arrField[i].ToString() == pPreviousSortField)
                        //{
                        //    if (pPreviousOrder == "ASC")
                        //    {
                        //        hlHref.NavigateUrl = "../" + pSortingURL + arrField[i].ToString() + "&order=DESC";
                        //    }
                        //    else
                        //    {
                        //        hlHref.NavigateUrl = "../" + pSortingURL + arrField[i].ToString() + "&order=ASC";
                        //    }
                        //}
                        //else
                        //{
                        //    hlHref.NavigateUrl = "../" + pSortingURL + arrField[i].ToString() + "&order=ASC";
                        //}
                        //hlHref.CssClass = pHeaderHyperCssClass;
                        //phHeader.Controls.Add(new LiteralControl("<th>"));
                        //phHeader.Controls.Add(hlHref);
                        //phHeader.Controls.Add(new LiteralControl("</th>"));

                        LinkButton lbHref = new LinkButton();
                        lbHref.ID = pCheckBoxAttribute + "_" + i + "_" + strSortLinkPrefix + arrField[i].ToString();
                        lbHref.Text = arrHeader[i].ToString();
                        lbHref.CssClass = pHeaderHyperCssClass;
                        lbHref.Click += new EventHandler(OnSort);
                        phHeader.Controls.Add(new LiteralControl("<th class=\"" + strAlign + "_" + pHeaderCssClass + "\" align='left'>"));
                        phHeader.Controls.Add(lbHref);
                        phHeader.Controls.Add(new LiteralControl("</th>"));
                    }
                }
            }
            phHeader.Controls.Add(new LiteralControl("</tr>"));
        }

        protected bool IsHyperLinkCol(int idx)
        {
            string strLink = "," + pHyperLinkCol.ToString().Trim() + ",";
            if (strLink.IndexOf(","+idx+",") >=0) {
                return true;
            }else{
                return false;
            }
        }

        protected string GetHyperLinkType(int idx)
        {
            string strLink = "," + pHyperLinkCol.ToString().Trim() + ",";
            if (strLink.IndexOf("," + idx + ",") >= 0)
            {
                for (int i = 0; i < arrHyperLinkCol.Length; i++)
                {
                    if (arrHyperLinkCol[i].ToString() == idx.ToString())
                    {
                        return arrHyperLinkType[i].ToString();
//                        break;
                    }
                }
                return "0";
            }
            else
            {
                return "0";
            }
        }

        protected void AddPageTotal()
        {
            string strStyle = pPageTotalCssClass;
            int iType = -1;
            int iLabelCols = 0;
            string[] arrDBField = pDBField.ToString().Split(',');
            phListing.Controls.Add(new LiteralControl("<tr class=\"" + strStyle + "\">"));
            for (int i = 0; i < arrDBField.Length; i++)
            {
                if (i == 0)
                {
                    iType = 0;
                    iLabelCols++;
                }
                else if (i == iLabelCols)
                {
                    if (iType == 0)
                    {
                        iType = 0;
                        iLabelCols++;
                    }
                }
                else if (iType == 0)
                {
                    iLabelCols++;
                    iType = 2;
                }
                else
                {
                    iType = -99;
                }

                string strAlign = "";
                bool blnFound = false;

                for (int j = 0; j < arrPageTotalField.Length; j++)
                {
                    if (Convert.ToInt32(arrPageDisplayType[j]) < 10)
                    {
                        strAlign = "Left";
                    }
                    else if (Convert.ToInt32(arrPageDisplayType[j]) < 20)
                    {
                        strAlign = "Center";
                    }
                    else
                    {
                        strAlign = "Right";
                    }

                    if (arrDBField[i].ToString() == arrPageTotalField[j].ToString())
                    {
                        if (iType == 2 || iType == 0)
                        {
                            phListing.Controls.Add(new LiteralControl("<td align=\"left\" colspan=\"" + (iLabelCols - 1) + "\">" + pPageTotalLabel + "</td>"));
                            iType = -99;
                        }

                        blnFound = true;
                        Label lbl = new Label();
                        lbl.ID = pCheckBoxAttribute + "*" + "paget" + i;

                        switch (Convert.ToInt32(arrPageComputeAction[j]))
                        {
                            case (int)ComputeActionEnum.Sum:
                                object objTotal = dsPageData.Tables[0].Compute("SUM(" + arrPageTotalField[j].ToString() + ")", "");
                                decimal decTotal = Convert.ToDecimal((objTotal is DBNull) ? 0 : objTotal);
                                switch (Convert.ToInt32(arrPageDisplayType[j].ToString().Substring(arrPageDisplayType[j].ToString().Length - 1)))
                                {
                                    case (int)DisplayFieldTypeEnum.DecimalType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["CurrencyDisplayFormat"] + "}", Convert.ToDecimal(decTotal));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.RateType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["RateFormat"] + "}", Convert.ToDecimal(decTotal));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.IntegerType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["NumberDisplayFormat"] + "}", Convert.ToInt32(decTotal));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    default:
                                        if (pExcelString)
                                        {
                                            phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\" x:str=\"'" + decTotal.ToString() + "\">"));
                                        }
                                        else
                                        {
                                            phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        }
                                        lbl.Text = decTotal.ToString();
                                        phListing.Controls.Add(lbl);
                                        break;
                                }
                                phListing.Controls.Add(new LiteralControl("</td>"));

                                break;
                            case (int)ComputeActionEnum.Count:
                                int intTotal = Convert.ToInt32(dsPageData.Tables[0].Compute("COUNT(" + arrPageTotalField[j].ToString() + ")", ""));
                                phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["NumberDisplayFormat"] + "}", Convert.ToInt32(intTotal));
                                phListing.Controls.Add(lbl);
                                phListing.Controls.Add(new LiteralControl("</td>"));
                                break;
                            case (int)ComputeActionEnum.Maximum:
                                object objMax = dsPageData.Tables[0].Compute("MAX(" + arrPageTotalField[j].ToString() + ")", "");
                                switch (Convert.ToInt32(arrPageDisplayType[j].ToString().Substring(arrPageDisplayType[j].ToString().Length - 1)))
                                {
                                    case (int)DisplayFieldTypeEnum.DecimalType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["CurrencyDisplayFormat"] + "}", Convert.ToDecimal((objMax is DBNull) ? 0 : objMax));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.RateType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["RateFormat"] + "}", Convert.ToDecimal((objMax is DBNull) ? 0 : objMax));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.DateValueType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = (string)Tools.toDateString(objMax, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateValueFormat"], true);
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.DateType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        if (pExcelString)
                                        {
                                            lbl.Text = (string)Tools.toDateString(objMax, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateValueFormat"], true);
                                        }
                                        else
                                        {
                                            lbl.Text = (string)Tools.toDateString(objMax, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateFormat"], true);
                                        }
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.IntegerType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["NumberDisplayFormat"] + "}", Convert.ToInt32((objMax is DBNull) ? 0 : objMax));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    default:
                                        if (pExcelString)
                                        {
                                            phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\" x:str=\"'" + Convert.ToString((objMax is DBNull) ? "" : objMax) + "\">"));
                                        }
                                        else
                                        {
                                            phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        }
                                        lbl.Text = Convert.ToString((objMax is DBNull) ? "" : objMax);
                                        phListing.Controls.Add(lbl);
                                        break;
                                }
                                phListing.Controls.Add(new LiteralControl("</td>"));

                                break;
                            case (int)ComputeActionEnum.Minimum:
                                object objMin = dsPageData.Tables[0].Compute("MIN(" + arrPageTotalField[j].ToString() + ")", "");
                                switch (Convert.ToInt32(arrPageDisplayType[j].ToString().Substring(arrPageDisplayType[j].ToString().Length - 1)))
                                {
                                    case (int)DisplayFieldTypeEnum.DecimalType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["CurrencyDisplayFormat"] + "}", Convert.ToDecimal((objMin is DBNull) ? 0 : objMin));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.RateType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["RateFormat"] + "}", Convert.ToDecimal((objMin is DBNull) ? 0 : objMin));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.DateValueType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = (string)Tools.toDateString(objMin, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateValueFormat"], true);
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.DateType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        if (pExcelString)
                                        {
                                            lbl.Text = (string)Tools.toDateString(objMin, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateValueFormat"], true);
                                        }
                                        else
                                        {
                                            lbl.Text = (string)Tools.toDateString(objMin, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateFormat"], true);
                                        }
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.IntegerType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["NumberDisplayFormat"] + "}", Convert.ToInt32((objMin is DBNull) ? 0 : objMin));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    default:
                                        if (pExcelString)
                                        {
                                            phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\" x:str=\"'" + Convert.ToString((objMin is DBNull) ? "" : objMin) + "\">"));
                                        }
                                        else
                                        {
                                            phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        }
                                        lbl.Text = Convert.ToString((objMin is DBNull) ? "" : objMin);
                                        phListing.Controls.Add(lbl);
                                        break;
                                }
                                phListing.Controls.Add(new LiteralControl("</td>"));

                                break;
                            default:
                                phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                lbl.Text = "";
                                phListing.Controls.Add(lbl);
                                phListing.Controls.Add(new LiteralControl("</td>"));
                                break;
                        }
                    }
                }
                if (!blnFound)
                {
                    switch (iType)
                    {
                        case 0:
                            break;
                        case 2:
                            phListing.Controls.Add(new LiteralControl("<td align=\"left\" colspan=\"" + iLabelCols + "\">" + pPageTotalLabel + "</td>"));
                            phListing.Controls.Add(new LiteralControl("<td align=\"left\">&nbsp;</td>"));
                            break;
                        default:
                            phListing.Controls.Add(new LiteralControl("<td align=\"left\">&nbsp;</td>"));
                            break;
                    }
                }
            }

        }

        protected void AddGrandTotal()
        {
            string strStyle = pGrandTotalCssClass;
            int iType = -1;
            int iLabelCols = 0;
            string[] arrDBField = pDBField.ToString().Split(',');
            phListing.Controls.Add(new LiteralControl("<tr class=\"" + strStyle + "\">"));
            for (int i = 0; i < arrDBField.Length; i++)
            {
                if (i == 0)
                {
                    iType = 0;
                    iLabelCols++;
                }
                else if (i == iLabelCols)
                {
                    if (iType == 0)
                    {
                        iType = 0;
                        iLabelCols++;
                    }
                }
                else if (iType == 0)
                {
                    iLabelCols++;
                    iType = 2;
                }
                else
                {
                    iType = -99;
                }

                string strAlign = "";
                bool blnFound = false;

                for (int j = 0; j < arrGrandTotalField.Length; j++)
                {
                    if (Convert.ToInt32(arrGrandDisplayType[j]) < 10)
                    {
                        strAlign = "Left";
                    }
                    else if (Convert.ToInt32(arrGrandDisplayType[j]) < 20)
                    {
                        strAlign = "Center";
                    }
                    else
                    {
                        strAlign = "Right";
                    }

                    if (arrDBField[i].ToString() == arrGrandTotalField[j].ToString())
                    {
                        if (iType == 2 || iType == 0)
                        {
                            phListing.Controls.Add(new LiteralControl("<td align=\"left\" colspan=\"" + (iLabelCols -1) + "\">" + pGrandTotalLabel + "</td>"));
                            iType = -99;
                        }

                        blnFound = true;
                        Label lbl = new Label();
                        lbl.ID = pCheckBoxAttribute + "*" + "grandt" + i + "_" + i;

                        switch (Convert.ToInt32(arrGrandComputeAction[j]))
                        {
                            case (int)ComputeActionEnum.Sum:
                                object objTotal = pDataSet.Tables[0].Compute("SUM(" + arrGrandTotalField[j].ToString() + ")", "");
                                decimal decTotal = Convert.ToDecimal((objTotal is DBNull) ? 0 : objTotal);
                                switch (Convert.ToInt32(arrGrandDisplayType[j].ToString().Substring(arrGrandDisplayType[j].ToString().Length - 1)))
                                {
                                    case (int)DisplayFieldTypeEnum.DecimalType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["CurrencyDisplayFormat"] + "}", Convert.ToDecimal(decTotal));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.RateType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["RateFormat"] + "}", Convert.ToDecimal(decTotal));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.IntegerType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["NumberDisplayFormat"] + "}", Convert.ToInt32(decTotal));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    default:
                                        if (pExcelString)
                                        {
                                            phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\" x:str=\"'" + decTotal.ToString() + "\">"));
                                        }
                                        else
                                        {
                                            phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        }
                                        lbl.Text = decTotal.ToString();
                                        phListing.Controls.Add(lbl);
                                        break;
                                }
                                phListing.Controls.Add(new LiteralControl("</td>"));

                                break;
                            case (int)ComputeActionEnum.Count:
                                int intTotal = Convert.ToInt32(pDataSet.Tables[0].Compute("COUNT(" + arrGrandTotalField[j].ToString() + ")", ""));
                                phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["NumberDisplayFormat"] + "}", Convert.ToInt32(intTotal));
                                phListing.Controls.Add(lbl);
                                phListing.Controls.Add(new LiteralControl("</td>"));
                                break;
                            case (int)ComputeActionEnum.Maximum:
                                object objMax = pDataSet.Tables[0].Compute("MAX(" + arrGrandTotalField[j].ToString() + ")", "");
                                switch (Convert.ToInt32(arrGrandDisplayType[j].ToString().Substring(arrGrandDisplayType[j].ToString().Length - 1)))
                                {
                                    case (int)DisplayFieldTypeEnum.DecimalType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                    lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["CurrencyDisplayFormat"] + "}", Convert.ToDecimal((objMax is DBNull) ? 0 : objMax));
                                    phListing.Controls.Add(lbl);
                                    break;
                                case (int)DisplayFieldTypeEnum.RateType:
                                    phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                    lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["RateFormat"] + "}", Convert.ToDecimal((objMax is DBNull) ? 0 : objMax));
                                    phListing.Controls.Add(lbl);
                                    break;
                                case (int)DisplayFieldTypeEnum.DateValueType:
                                    phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                    lbl.Text = (string)Tools.toDateString(objMax, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateValueFormat"], true);
                                    phListing.Controls.Add(lbl);
                                    break;
                                case (int)DisplayFieldTypeEnum.DateType:
                                    phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                    if (pExcelString)
                                    {
                                        lbl.Text = (string)Tools.toDateString(objMax, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateValueFormat"], true);
                                    }
                                    else
                                    {
                                        lbl.Text = (string)Tools.toDateString(objMax, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateFormat"], true);
                                    }
                                    phListing.Controls.Add(lbl);
                                    break;
                                case (int)DisplayFieldTypeEnum.IntegerType:
                                    phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                    lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["NumberDisplayFormat"] + "}", Convert.ToInt32((objMax is DBNull) ? 0 : objMax));
                                    phListing.Controls.Add(lbl);
                                    break;
                                default:
                                    if (pExcelString)
                                    {
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\" x:str=\"'" + Convert.ToString((objMax is DBNull) ? "" : objMax) + "\">"));
                                    }
                                    else
                                    {
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                    }
                                    lbl.Text = Convert.ToString((objMax is DBNull) ? "" : objMax);
                                    phListing.Controls.Add(lbl);
                                    break;
                                }
                                phListing.Controls.Add(new LiteralControl("</td>"));

                                break;
                            case (int)ComputeActionEnum.Minimum:
                                object objMin = pDataSet.Tables[0].Compute("MIN(" + arrGrandTotalField[j].ToString() + ")", "");
                                switch (Convert.ToInt32(arrGrandDisplayType[j].ToString().Substring(arrGrandDisplayType[j].ToString().Length - 1)))
                                {
                                    case (int)DisplayFieldTypeEnum.DecimalType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["CurrencyDisplayFormat"] + "}", Convert.ToDecimal((objMin is DBNull) ? 0 : objMin));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.RateType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["RateFormat"] + "}", Convert.ToDecimal((objMin is DBNull) ? 0 : objMin));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.DateValueType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = (string)Tools.toDateString(objMin, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateValueFormat"], true);
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.DateType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        if (pExcelString)
                                        {
                                            lbl.Text = (string)Tools.toDateString(objMin, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateValueFormat"], true);
                                        }
                                        else
                                        {
                                            lbl.Text = (string)Tools.toDateString(objMin, new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateFormat"], true);
                                        }
                                        phListing.Controls.Add(lbl);
                                        break;
                                    case (int)DisplayFieldTypeEnum.IntegerType:
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["NumberDisplayFormat"] + "}", Convert.ToInt32((objMin is DBNull) ? 0 : objMin));
                                        phListing.Controls.Add(lbl);
                                        break;
                                    default:
                                        if (pExcelString)
                                        {
                                            phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\" x:str=\"'" + Convert.ToString((objMin is DBNull) ? "" : objMin) + "\">"));
                                        }
                                        else
                                        {
                                            phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                        }
                                        lbl.Text = Convert.ToString((objMin is DBNull) ? "" : objMin);
                                        phListing.Controls.Add(lbl);
                                        break;
                                }
                                phListing.Controls.Add(new LiteralControl("</td>"));

                                break;
                            default:
                                phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\">"));
                                lbl.Text = "";
                                phListing.Controls.Add(lbl);
                                phListing.Controls.Add(new LiteralControl("</td>"));
                                break;
                        }
                    }
                }
                if (!blnFound)
                {
                    switch (iType)
                    {
                        case 0:
                            break;
                        case 2:
                            phListing.Controls.Add(new LiteralControl("<td align=\"left\" colspan=\"" + iLabelCols + "\">" + pGrandTotalLabel + "</td>"));
                            phListing.Controls.Add(new LiteralControl("<td align=\"left\">&nbsp;</td>"));
                            break;
                        default:
                            phListing.Controls.Add(new LiteralControl("<td align=\"left\">&nbsp;</td>"));
                            break;
                    }
                }
            }

        }

        protected void AddRow(DataRow row, int idx)
        {
            string[] arrDBField = pDBField.ToString().Split(',');
            int iType = 0;
            string strStyle = pRowCssClass;

            if (pShowHighlightRow)
            {
                phListing.Controls.Add(new LiteralControl("<tr onmouseover=\"cOn(this,'" + pHighlightRowColor + "');\" onmouseout=\"cOff(this,'" + pRowColor + "');\">"));
//                phListing.Controls.Add(new LiteralControl("<tr>"));
            }
            else
            {
                phListing.Controls.Add(new LiteralControl("<tr class=\"" + strStyle + "\">"));
            }
            for (int i = 0; i < arrDBField.Length; i++)
            {
                if (i == 0)
                {
                    if (pAllowSelection)
                        iType = 0;
                    else
                        iType = -1;
                }
                else if (IsHyperLinkCol(i))
                {
                    string strLType = GetHyperLinkType(i);
                    if (strLType == "0")
                    {
                        if (pDetailURL == "")
                        {
                            iType = -99;
                        }
                        else
                        {
                            iType = 2;
                        }
                    }
                    else if (strLType == "1")
                    {
                        iType = 1;
                    }
                    else
                    {
                        iType = -99;
                    }
                }
                else
                {
                    iType = -99;
                }

                string strAlign = "";
                if (Convert.ToInt32(arrDisplayType[i]) < 10)
                {
                    strAlign = "Left";
                }
                else if (Convert.ToInt32(arrDisplayType[i]) < 20)
                {
                    strAlign = "Center";
                }
                else
                {
                    strAlign = "Right";
                }
                string strWidth = "width=\"";
                if (pColumnWidth.ToString().Length > 0)
                {
                    if (arrColumnWidth[i].ToString().Length == 0)
                    {
                        strWidth = "";
                    }
                    else
                    {
                        strWidth += arrColumnWidth[i].ToString() + "\"";
                    }
                }
                else
                {
                    strWidth = "";
                }

                switch (iType)
                {
                    case -1:
// can't display column if no selection allowed
//                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strStyle + "\">&nbsp;</td>"));
                        break;
                    case 0:
                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\"" + strWidth + ">"));
                        CheckBox cb = new CheckBox();
                        cb.ID = row[arrDBField[0]].ToString() + "*" + pCheckBoxAttribute + "*" + idx;
                        if (pShowCheckedRow)
                        {
                            cb.Attributes.Add("onclick", "highlightRow(this,'" + pCheckedRowColor + "','" + pHighlightRowColor + "','" + pRowColor + "');DisplayCount(this,'" + pCheckBoxAttribute + "');");
                        }
                        else
                        {
                            cb.Attributes.Add("onclick", "DisplayCount(this,'" + pCheckBoxAttribute + "');");
                        }
                        phListing.Controls.Add(cb);
                        phListing.Controls.Add(new LiteralControl("</td>"));
                        break;
                    case 1:
                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\"" + strWidth + ">"));
                        LinkButton lbtn = new LinkButton();
                        lbtn.ID = pCheckBoxAttribute + "*" + idx + "*" + strHyperLinkPrefix + i + strHyperLinkIdx + row[arrDBField[0]].ToString();
                        lbtn.Text = row[arrDBField[i]].ToString();
                        lbtn.Click += new EventHandler(lbtn_Click);
                        lbtn.CssClass = pRowHyperCssClass;
                        phListing.Controls.Add(lbtn);
                        phListing.Controls.Add(new LiteralControl("</td>"));
                        break;
                    case 2:
                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\"" + strWidth + ">"));
                        HyperLink href = new HyperLink();
                        href.ID = pCheckBoxAttribute + "*" + idx + "*" + strHyperLinkPrefix + i + strHyperLinkIdx + row[arrDBField[0]].ToString();
                        href.Text = row[arrDBField[i]].ToString();
                        href.NavigateUrl = "../" + pDetailURL + row[arrDBField[0]].ToString();
                        href.CssClass = pRowHyperCssClass;
                        phListing.Controls.Add(href);
                        phListing.Controls.Add(new LiteralControl("</td>"));
                        break;

                    default:

                        Label lbl = new Label();
                        lbl.ID = pCheckBoxAttribute + "*" + idx + "*" + "col" + i + "_" + row[arrDBField[0]].ToString();
                        if (row[arrDBField[i]] == null || row[arrDBField[i]].ToString().Trim().Length == 0)
                        {
                            phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\"" + strWidth + ">"));
                            lbl.Text = "";
                        }
                        else
                        {
                            switch (Convert.ToInt32(arrDisplayType[i].ToString().Substring(arrDisplayType[i].ToString().Length-1)))
                            {
                                case (int)DisplayFieldTypeEnum.DecimalType:
                                    phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\"" + strWidth + ">"));
                                    lbl.Text = String.Format("{0:"+ConfigurationManager.AppSettings["CurrencyDisplayFormat"]+"}", Convert.ToDecimal(row[arrDBField[i]]));
                                    phListing.Controls.Add(lbl);
                                    break;
                                case (int)DisplayFieldTypeEnum.RateType:
                                    phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\"" + strWidth + ">"));
                                    lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["RateFormat"] + "}", Convert.ToDecimal(row[arrDBField[i]]));
                                    phListing.Controls.Add(lbl);
                                    break;
                                case (int)DisplayFieldTypeEnum.DateValueType:
                                    phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\"" + strWidth + ">"));
                                    lbl.Text = (string)Tools.toDateString(row[arrDBField[i]], new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateValueFormat"], true);
                                    phListing.Controls.Add(lbl);
                                    break;
                                case (int)DisplayFieldTypeEnum.DateType:
                                    phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\"" + strWidth + ">"));
                                    if (pExcelString)
                                    {
                                        lbl.Text = (string)Tools.toDateString(row[arrDBField[i]], new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateValueFormat"], true);
                                    }
                                    else
                                    {
                                        lbl.Text = (string)Tools.toDateString(row[arrDBField[i]], new string[] { "yyyy/MM/dd" }, ConfigurationManager.AppSettings["DateFormat"], true);
                                    }
                                    phListing.Controls.Add(lbl);
                                    break;
                                case (int)DisplayFieldTypeEnum.IntegerType:
                                    phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\"" + strWidth + ">"));
                                    lbl.Text = String.Format("{0:" + ConfigurationManager.AppSettings["NumberDisplayFormat"] + "}", Convert.ToInt32(row[arrDBField[i]]));
                                    phListing.Controls.Add(lbl);
                                    break;
                                default:
                                    if (pExcelString)
                                    {
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\" x:str=\"'" + row[arrDBField[i]].ToString() + "\">"));
                                    }
                                    else
                                    {
                                        phListing.Controls.Add(new LiteralControl("<td class=\"" + strAlign + "_" + strStyle + "\"" + strWidth + ">"));
                                    }
                                    lbl.Text = row[arrDBField[i]].ToString();
                                    phListing.Controls.Add(lbl);
                                    break;
                            }
                        }
                        if (row[arrDBField[i]].ToString() == "")
                        {
                            phListing.Controls.Add(new LiteralControl("&nbsp;"));
                        }
                        phListing.Controls.Add(new LiteralControl("</td>"));

                        break;
                }
                //if (pShowPageTotal)
                //{
                //    for (int z = 0; z < arrPageTotalField.Length; z++)
                //    {
                //        if (arrPageTotalField[z].ToString() == arrDBField[i].ToString())
                //        {
                //            if (row[arrDBField[i]] != DBNull.Value)
                //            {
                //                arrPageTotal[z] = Convert.ToString(Convert.ToDecimal(arrPageTotal[z]) + Convert.ToDecimal(row[arrDBField[i]]));
                //            }
                //        }
                //    }
                //}

            }
            phListing.Controls.Add(new LiteralControl("</tr>"));

        }

        protected virtual void OnSort(object sender, EventArgs e)
        {
            LinkButton lbtn = (LinkButton)sender;
            int idx = lbtn.ID.IndexOf(strSortLinkIdx);
            pCurrentSortField = lbtn.ID.Substring(idx + 1).ToString().Trim();
            ResetPageNumber();
            SortData();
//            doSorting = true;
            BindData();
        }

        protected virtual void lbtn_Click(object sender, EventArgs e)
        {
            LinkButton lbtn = (LinkButton)sender;
            int idx = lbtn.ID.IndexOf(strHyperLinkIdx);
            int id = Convert.ToInt32(lbtn.ID.Substring(idx+1).ToString().Trim());

            _intSelectID = id;

            URLClickEventArgs eArgs = new URLClickEventArgs(id,"");
            URLClickEventHandler handler = URLClickEvent;
            if (handler != null)
            {
                handler(this, eArgs);
            }

        }

        public void SortData()
        {
            if (pCurrentSortField.ToString().Trim().Length > 0)
            {
                if (pCurrentSortField == txtPreviousSortField.Value)
                {
                    if (txtPreviousSortOrder.Value == "ASC")
                    {
                        txtPreviousSortOrder.Value = "DESC";
                    }
                    else
                    {
                        txtPreviousSortOrder.Value = "ASC";
                    }
                }
                else
                {
                    txtPreviousSortOrder.Value = "ASC";
                    txtPreviousSortField.Value = pCurrentSortField;
                }
            }
            else
            {
                txtPreviousSortField.Value = pCurrentSortField;
            }
        }

        public void ResetPageNumber()
        {
            intPage = 1;
            intStarRow = 0;
            txtCurrentPage.Value = "1";
            txtSelectedCount.Attributes.Add(pCheckBoxAttribute, "");
            if (intTotalPage > 0)
            {
                lblPage.Text = "Page " + intPage + " of " + intTotalPage;
            }
            else
            {
                lblPage.Text = "Page 0 of 0";
                btnNext.Visible = false;
                btnPrevious.Visible = false;
            }
        

        }

        public void BindData()
        {
            pCurrentSortField = txtPreviousSortField.Value;
            if (txtCurrentPage.Value.Trim().Length > 0)
            {
                intPage = Convert.ToInt32(txtCurrentPage.Value);
            }
            else
            {
                intPage = 1;
            }
            PageData(true);
        }

        private void PageData(bool bolPaging)
        {
            arrDisplayType = pDisplayType.ToString().Split(',');
            arrHyperLinkType = pHyperLinkType.ToString().Split(',');
            arrHyperLinkCol = pHyperLinkCol.ToString().Split(',');
            arrColumnWidth = pColumnWidth.ToString().Split(',');
            phHeader.Controls.Clear();
            phListing.Controls.Clear();
            AddHeader();
            if (pDataSet != null)
            {
                if (pDataSet.Tables.Count > 0)
                {
                    if (pCurrentSortField.ToString().Trim().Length > 0)
                    {
                        drRecords = pDataSet.Tables[0].Select("", pCurrentSortField + " " + txtPreviousSortOrder.Value);
                    }
                    else
                    {
                        drRecords = pDataSet.Tables[0].Select("");
                    }
                    if (drRecords.Length > 0)
                    {
                        if (pShowPaging)
                        {
                            intTotalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(drRecords.Length) / Convert.ToDecimal(pPageSize)));
                            if (bolPaging)
                            {
                                if (intTotalPage < intPage)
                                {
                                    ResetPageNumber();
                                }
                                if (txtCurrentPage.Value.ToString().Trim().Length > 0)
                                {
                                    intStarRow = (intPage - 1) * pPageSize;
                                    txtCurrentPage.Value = "" + intPage;
                                }
                                else
                                {
                                    intPage = 1;
                                    intStarRow = 0;
                                }
                            }
                            else
                            {
                                intPage = 1;
                                intStarRow = 0;
                            }
                            SetPageButton();
                            if (pShowPageTotal)
                            {
                                dsPageData = pDataSet.Clone();
                            }
                            for (int i = intStarRow; i < (intPage * pPageSize) && i < (drRecords.Length); i++)
                            {
                                DataRow drRow = drRecords[i];
                                if (pShowPageTotal)
                                {
                                    dsPageData.Tables[0].ImportRow(drRow);
                                }
                                AddRow(drRow,i);
                            }
                        }
                        else
                        {
                            btnPrevious.Visible = false;
                            btnNext.Visible = false;
                            lblPage.Visible = false;
                            //no paging
                            pShowPageTotal = false;
                            int i = 0;
                            foreach (DataRow drRow in drRecords)
                            {
                                i++;
                                AddRow(drRow,i);
                            }
                        }
                        if (pShowPageTotal)
                        {
                            arrPageTotalField = pPageTotalField.ToString().Split(',');
                            arrPageDisplayType = pPageDisplayType.ToString().Split(',');
                            arrPageComputeAction = pPageComputeAction.ToString().Split(',');

                            AddPageTotal();
                        }
                        if (pShowGrandTotal)
                        {
                            arrGrandTotalField = pGrandTotalField.ToString().Split(',');
                            arrGrandDisplayType = pGrandDisplayType.ToString().Split(',');
                            arrGrandComputeAction = pGrandComputeAction.ToString().Split(',');

                            AddGrandTotal();
                        }
                    }
                    else
                    {
                        btnPrevious.Visible = false;
                        btnNext.Visible = false;
                        lblPage.Visible = false;
                    }
                }
            }
        }

        private void SetPageButton()
        {
            if (pShowPaging)
            {
                if (intTotalPage > 0)
                {
                    lblPage.Text = "Page " + intPage + " of " + intTotalPage;
                    lblPage.Visible = true;
                    if (intPage == 1)
                    {
                        btnPrevious.Visible = false;
                    }
                    else
                    {
                        btnPrevious.Visible = true;
                    }
                    if (intPage == intTotalPage)
                    {
                        btnNext.Visible = false;
                    }
                    else
                    {
                        btnNext.Visible = true;
                    }
                }
                else
                {
                    lblPage.Text = "Page 0 of 0";
                    btnNext.Visible = false;
                    btnPrevious.Visible = false;
                }
            }
            else
            {
                lblPage.Text = "";
                btnNext.Visible = false;
                btnPrevious.Visible = false;
            }
        }
        public string checkedList
        {
            get { return GetCheckedList(); }
            set { SetCheckedList(value); }
        }

        protected string GetCheckedList()
        {
            string strReturn = "";
            if (pAllowSelection)
            {

                CheckBox cbSelect;
                string strTemp = "";
                foreach (Control objC in phListing.Controls)
                {
                    strTemp = objC.ID;
                    if (objC is CheckBox)
                    {
                        cbSelect = (CheckBox)objC;
                        if (cbSelect.Checked)
                        {
                            strReturn += "," + cbSelect.ID.ToString().Split('*')[0];
                        }
                    }
                }
                if (strReturn.Length > 0)
                {
                    strReturn = strReturn.Substring(1);
                }
            }

            return strReturn;
        }

        protected void SetCheckedList(string strSelected)
        {
            if (pAllowSelection && strSelected.Length > 0)
            {
                CheckBox cbSelect;
                string strTemp = "," + strSelected + ",";
                foreach (Control objC in phListing.Controls)
                {
                    if (objC is CheckBox)
                    {
                        cbSelect = (CheckBox)objC;
                        if (strTemp.IndexOf("," + cbSelect.ID.ToString().Split('*')[0] + ",") >= 0)
                        {
                            cbSelect.Checked = true;
                        }
                        else
                        {
                            cbSelect.Checked = false;
                        }
                    }
                }
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            intPage = intPage - 1;
            pCurrentSortField = txtPreviousSortField.Value;
            PageData(true);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            intPage = intPage + 1;
            pCurrentSortField = txtPreviousSortField.Value;
            PageData(true);
        }
    }
}