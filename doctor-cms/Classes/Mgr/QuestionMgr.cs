using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SunStar_CMS.admin.Classes.Utils;
using SunStar_CMS.admin.Classes.Objects;
using SunStar_CMS.admin.Classes.ControlValues;
using System.Data.Common;
using System.Collections;

namespace SunStar_CMS.admin.Classes.Mgr
{
    public class QuestionMgr
    {
        internal object[] getQuestionList(string sql)
        {

            if (string.IsNullOrEmpty(sql))
            {
                return new object[] { null, sql };
            }
            using (DBUtil util = new DBUtil())
            {
                return new object[] { util.getDataSet(sql, "tb_type"), sql };
            }
        }

        internal object[] getQuestionList(SunStar_CMS.admin.Classes.Objects.User user, string question, string name,string answer_name, string status)
        {
            BidirHashtable<object, EnumValueAttribute> recordStatusMap = EnumConvertUtils.EnumToAttributeMap(typeof(QuestionStatusEnum));

            string sql = @"SELECT a.question_id,a.question, a.name, a.question_date,a.answer_name,a.answer_date,  ";
            sql += "CASE a.status ";
            foreach (string recordStatus in Enum.GetNames(typeof(QuestionStatusEnum)))
            {
                sql += "WHEN " + Convert.ToString((int)recordStatusMap[Enum.Parse(typeof(QuestionStatusEnum), recordStatus)].DbValue) +
                        " THEN '" + (string)recordStatusMap[Enum.Parse(typeof(QuestionStatusEnum), recordStatus)].DisplayValue + "' ";
            }
            sql += @"END AS status 
                    FROM tb_question a
                    WHERE 1=1 ";

            if (question != null)
            {
                sql += "AND a.question LIKE '%" + question.Replace('\'', '"') + "%' ";
            }
            if (name != null)
            {
                sql += "AND a.name LIKE '%" + name.Replace('\'', '"') + "%' ";
            }
            if (answer_name != null)
            {
                sql += "AND a.answer_name LIKE '%" + answer_name.Replace('\'', '"') + "%' ";
            }
            if (!string.IsNullOrEmpty(status ))
            {
                sql += "AND a.status = " + status + " ";
            }
            sql += " order by question_date desc";
            using (DBUtil util = new DBUtil())
            {
                return new object[] { util.getDataSet(sql, "tb_question"), sql };
            }

        }

        internal System.Collections.ArrayList ignoreQuestion(string questionList)
        {
            ArrayList result = new ArrayList();
            string sql = "";
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        string[] question_id = questionList.Split(',');
                        for (int i = 0; i < question_id.Length; i++)
                        {
                            sql = @"update tb_question set status=2
                                                WHERE question_id = @question_id";
                            util.executeNonQuery(sql,
                                                    new string[] { "@question_id" },
                                                    new object[] { Convert.ToInt32(question_id[i]) },
                                                    tx);
                        }
                        tx.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        internal object getQuetionObject(int question_id)
        {
            string sql = "select * from tb_question where question_id=" + question_id.ToString();

            DataTable dt = null;
            using (DBUtil util = new DBUtil())
            {
                dt = util.getDataSet(sql, "tb_question").Tables[0];
            }

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ObjQuestion n = new ObjQuestion();
                n.QuestionId = (int)dr["question_id"];
                n.Question = dr["question"].ToString();
                n.Description = dr["description"].ToString();
                n.Name = dr["name"].ToString();
                n.QuestionDate = (DateTime)dr["question_date"];
                n.Status = (int)dr["status"];
                n.Answer = dr["answer"].ToString();
                n.AnswerName = dr["answer_name"].ToString();
                n.Remarks = dr["remarks"].ToString();
                if (!Convert.IsDBNull(dr["answer_date"]))
                {
                    n.AnswerDate = (DateTime)dr["answer_date"];
                }
                else
                {
                    n.AnswerDate = null;
                }

                return n;
            }
            else
            {
                return null;
            }
        }


        internal int getNextQuestionByQuestionDate(DateTime question_date)
        {
            using (DBUtil util = new DBUtil())
            {
                string sql = "select * from tb_question where question_date>@question_date and status=0 order by question_date";

                string[] param_name = new string[] { "@question_date" };
                object[] param_value = new object[] { question_date };
                DataSet ds = util.getDataSet(sql, "tb_question", param_name, param_value);

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return 0;
                }
                return (int)ds.Tables[0].Rows[0]["question_id"];

            }
        }

        internal void replyQuestion(User user, ObjQuestion question)
        {
            using (DBUtil util = new DBUtil())
            {

                string sql = @"update tb_question set 
question=@question,
description=@description,
name=@name,
remarks=@remarks,
status=@status,
answer=@answer,
answer_name=@answer_name,
answer_date=getdate(),
answer_user=@user_id 
where question_id=@question_id ";
                string[] param_name = new string[] { "@question", "@description", "@name", 
                    "@remarks", "@status", "@answer", "@answer_name", "@user_id", "@question_id"};
                object[] param_value = new object[] { question.Question,question.Description,question.Name,question.Remarks
                ,question.Status,question.Answer,question.AnswerName,user.UserID,question.QuestionId};
                util.executeNonQuery(sql, param_name, param_value);
            }
        }
    }
}
