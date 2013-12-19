using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SunStar_CMS.admin.Classes.Objects
{
    public class ObjQuestion
    {
        int _QuestionId;

        public int QuestionId
        {
            get { return _QuestionId; }
            set { _QuestionId = value; }
        }
        string _Question;

        public string Question
        {
            get { return _Question; }
            set { _Question = value; }
        }
        string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        DateTime _QuestionDate;

        public DateTime QuestionDate
        {
            get { return _QuestionDate; }
            set { _QuestionDate = value; }
        }
        int _Status;

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        string _Answer;

        public string Answer
        {
            get { return _Answer; }
            set { _Answer = value; }
        }
        string _AnswerName;

        public string AnswerName
        {
            get { return _AnswerName; }
            set { _AnswerName = value; }
        }
        string _Remarks;

        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        DateTime? _AnswerDate;

        public DateTime? AnswerDate
        {
            get { return _AnswerDate; }
            set { _AnswerDate = value; }
        }

        int? _AnswerUser;

        public int? AnswerUser
        {
            get { return _AnswerUser; }
            set { _AnswerUser = value; }
        }

    }
}
