using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SunStar_CMS.admin.Classes.Utils
{
    public class Tools
    {
        /// <summary>
        /// To convert a string to DateTime,
        /// in order to put into db can use prepared statement or
        /// {DateTime}.ToString("yyyy/MM/dd")
        /// </summary>
        /// <param name="value">A string in dd/MM/yyyy</param>
        /// <returns>Converted DateTime</returns>
        public static object toDate(string value, string[] from)
        {
            if (!string.IsNullOrEmpty(value))
            {
                DateTimeFormatInfo dtfi = new CultureInfo("en-US").DateTimeFormat;
                dtfi.SetAllDateTimePatterns(from, 'd');
                return Convert.ToDateTime(value, dtfi);
            }
            return null;
        }

        /// <summary>
        /// To get the DateTime formate instead of string format
        /// </summary>
        /// <param name="value">The string that want to convert</param>
        /// <param name="from">String array of the initial DateTime format</param>
        /// <param name="db">If the DateTime used in database</param>
        /// <returns>Converted DateTime</returns>
        public static object toDate(object value, string[] from, Boolean db)
        {
            if (db && value is DBNull)
                return null;

            if (value.GetType() == typeof(string) && !string.IsNullOrEmpty((string)value))
            {
                return ((DateTime)toDate((string)value, from));
            }
            else if (value.GetType() == typeof(DateTime))
            {
                return ((DateTime)value);
            }

            if (db)
                return DBNull.Value;
            else
                return null;
        }

        ///// <summary>
        ///// To convert a string of (dd/MM/yyyy) to another format of datetime
        ///// </summary>
        ///// <param name="value">A string in dd/MM/yyyy</param>
        ///// <returns>Converted DateTime string</returns>
        //public static object toDateString(string value, string format)
        //{
        //    if (!string.IsNullOrEmpty(value))
        //    {
        //        return toDateString(value, new string[] { "dd/MM/yyyy" }, format);
        //    }
        //    return null;
        //}

        /// <summary>
        /// To convert a string from [from] format to [to] format
        /// </summary>
        /// <param name="value">The string that want to convert</param>
        /// <param name="from">String array of the initial DateTime format</param>
        /// <param name="to">Target fromat</param>
        /// <param name="db">If the string used in database</param>
        /// <returns>Converted DataTime string</returns>
        public static object toDateString(object value, string[] from, string to, Boolean db)
        {
            if (db && value is DBNull)
                return null;

            if (value.GetType() == typeof(string) && !string.IsNullOrEmpty((string)value))
            {
                return ((DateTime)toDate((string)value, from)).ToString(to);
            }
            else if (value.GetType() == typeof(DateTime))
            {
                return ((DateTime)value).ToString(to);
            }

            if (db)
                return DBNull.Value;
            else
                return null;
        }

        /// <summary>
        /// To convert an ArrayList to string with ',' seperate
        /// </summary>
        /// <param name="value">ArrayList want to convert</param>
        /// <returns>A string with ',' seperate</returns>
        public static string toString(ArrayList value)
        {
            string result ="";

            foreach (object obj in value)
            {
                result += Convert.ToString(obj) + ",";
            }
            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        /// <summary>
        /// Check char array is Numric
        /// </summary>
        /// <param name="sStr"> char array </param>
        /// <returns>true or false</returns>
        public  bool IsNumeric(string sStr)
        {
            bool bReturnValue = true;
            if (sStr == null || sStr.Length == 0)
            {
                bReturnValue = false;
            }
            else
            {
                foreach (char c in sStr)
                {
                    if (!Char.IsNumber(c))
                    {
                        
                            bReturnValue = false;
                            break;
                        
                    }
                }
            }
            return bReturnValue;
        }

        public  string checkTextDecimal(string textString, int Long, int dot)
        {


            if (IsNumeric(textString) == false)
            {
                return "Please enter a numeric value.";
            }
            else if (IsNumeric(textString) == true)
            {
                string[] value = textString.Split('.');
                if (value[0].Length == textString.Length)
                {
                    double max = 10;
                    for (int i = 0; i < Long; i++)
                    {
                        if (i == 0)
                        {
                            max = 10;
                        }
                        else
                        {
                            max *= max;
                        }
                    }
                    max = max - 1;
                    if (Convert.ToDouble(textString) > max)
                    {
                        return "你输入的数值太大，请介于 0~" + max.ToString();
                    }
                    else
                    {
                        return "true";
                    }

                }
                else
                {
                    if ((value[0].Length + value[1].Length) > Long)
                    {
                        return "你输入的数值位数太大 , 请介于 0~" + Long.ToString();
                    }
                    else
                    {
                        if (value[1].Length > dot)
                        {
                            return "你输入的数值位数太大 , 请介于 0~" + dot.ToString();
                        }
                        else
                        {
                            double max = 10;
                            for (int i = 0; i < Long; i++)
                            {
                                if (i == 0)
                                {
                                    max = 10;
                                }
                                else
                                {
                                    max *= max;
                                }
                            }
                            max = max - 1;
                            if (Convert.ToDouble(textString) > max)
                            {
                                return "你输入的数值太大 ,请介于 0~" + max.ToString();
                            }
                            else
                            {
                                return "true";
                            }

                        }

                    }
                }
            }

            return "true";
        }


        public static string changeHtmlLangue(string strValue)
        {
            strValue = strValue.Replace("<", "&lt;");
            strValue = strValue.Replace(">", "&gt;");
            strValue = strValue.Replace(" ", "&nbsp;");
            strValue = strValue.Replace("\r\n", "<br />");
            strValue = strValue.Replace("\n", "<br />");
            return strValue;
        }

        public static string changeHtmlLangue(string strValue,string[] strReplaceChar)
        {
            strValue = changeHtmlLangue(strValue);
            for (int i = 0; i < strReplaceChar.Length; i++)
            {
                strValue = strValue.Replace(strReplaceChar[i], "");
            }
            return strValue;
        }

        public static string changeHtmlLangue(string strValue, int num)
        {
            strValue = changeHtmlLangue(strValue);
            try
            {
                return strValue.Substring(0, num) + "..";
            }
            catch
            {
                return strValue;
            }
        }


        
    }
}
