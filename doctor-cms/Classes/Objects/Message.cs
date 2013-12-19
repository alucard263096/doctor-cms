using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace SunStar_CMS.admin.Classes.Objects
{
    public class Message
    {
        private Hashtable _message;
        public Message()
        {
            _message = new Hashtable();

            _message.Add("default", "Message (<font color=\"blue\">提示</font> / <font color=\"red\">警告</font>) 将在此显示");
            _message.Add("sessionExpired", "会话已经过期.<br /> 请重新登录");
            _message.Add("noCriteria", "请至少选择一个搜索条件");
            _message.Add("added", "记录已经生成");
            _message.Add("saved", "记录已经保存");
            _message.Add("replied", "回复成功");
        }

        /// <summary>
        /// To get message from the Hashtable without any formatting
        /// </summary>
        /// <param name="name">The key in the Hashtable</param>
        /// <returns>The message</returns>
        public string getMessage(object name)
        {
            return Convert.ToString(_message[name]);
        }

        /// <summary>
        /// To format a message with the message type
        /// </summary>
        /// <param name="type">The message type in Web.config</param>
        /// <param name="message">The message which want to format</param>
        /// <returns>The formatted message</returns>
        public string getMessage(string type, string message)
        {
            return "<font color=\"" + ConfigurationManager.AppSettings[type] + "\">" + message + "</font>";
        }

        /// <summary>
        /// To format a message in the Hashtable in warning format
        /// </summary>
        /// <param name="name">The key in the Hashtable</param>
        /// <returns>The formatted message</returns>
        public string getError(object name)
        {
            return "<font color=\"" + ConfigurationManager.AppSettings["Error"] + "\">" + Convert.ToString(_message[name]) + "</font>";
        }

        /// <summary>
        /// To format a message int the Hashtable in Information format
        /// </summary>
        /// <param name="name">The key in the Hashtable</param>
        /// <returns>The formatting message</returns>
        public string getInformation(object name)
        {
            return "<font color=\"" + ConfigurationManager.AppSettings["Information"] + "\">" + Convert.ToString(_message[name]) + "</font>";
        }
    }
}
