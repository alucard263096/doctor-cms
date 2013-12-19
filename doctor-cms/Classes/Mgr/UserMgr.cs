using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SunStar_CMS.admin.Classes.ControlValues;
using SunStar_CMS.admin.Classes.Objects;
using SunStar_CMS.admin.Classes.Utils;

namespace SunStar_CMS.admin.Classes.Mgr
{
    public class UserMgr
    {
        #region Login Variable
        /*******************For Login*********************/
        public const int USER_NOT_EXISTS = 0;
        public const int INACTIVE_USER = 1;
        public const int WRONG_PASSWORD = 2;
        #endregion

        #region Add User
        /*******************For Add User*********************/
        public const int LOGIN_ID_DUPLICATED = -1;
        #endregion

        /// <summary>
        /// To get the user information
        /// </summary>
        /// <param name="username">The user login id</param>
        /// <param name="passwd">The password</param>
        /// <returns>Return a user object if the input is valid or int if the input is invalid</returns>
        public object getUser(string username, string passwd)
        {
            using (DBUtil util = new DBUtil())
            {
                DataSet dsUser = util.getDataSet(@"SELECT * 
                                                    FROM tb_user 
                                                    WHERE login_id = @login",
                                                    "tb_user",
                                                    new string[] { "@login" },
                                                    new object[] { username });

                if (dsUser.Tables["tb_user"].Rows.Count == 0)
                {
                    return USER_NOT_EXISTS;
                }

                if ((Int16)dsUser.Tables["tb_user"].Rows[0]["status"] == (int)RecordStatusEnum.Inactive)
                {
                    return INACTIVE_USER;
                }

                if (encrypt(passwd) != (string)dsUser.Tables["tb_user"].Rows[0]["password"])
                {
                    return WRONG_PASSWORD;
                }

                #region Assign User Data
                DataRow row = dsUser.Tables["tb_user"].Rows[0];
                User user = new User();
                user.UserID = (int)row["user_id"];
                user.LoginID = (string)row["login_id"];
                user.UserName = (string)row["user_name"];
                user.Email = (string)row["email"];
                user.Remarks = (row["remarks"] is DBNull) ? "" : (string)row["remarks"];
                user.Status = (Int16)row["status"];
                #endregion

                #region Assign User Function
                DataSet dsUserFunction = util.getDataSet(@"SELECT * 
                                                            FROM tb_user_function
                                                            WHERE user_id = @userId
                                                            AND status = @status",
                                                            "tb_user_function",
                                                            new string[] { "@userId", "@status" },
                                                            new object[] { user.UserID, (int)RecordStatusEnum.Active });

                ArrayList userFunction = new ArrayList();
                foreach (DataRow ufrow in dsUserFunction.Tables["tb_user_function"].Rows)
                {
                    userFunction.Add(ufrow["function_id"]);
                }

                user.UserFunction = userFunction;
                #endregion

                return user;
            }
        }

        /// <summary>
        /// To get user by user_id
        /// </summary>
        /// <param name="user_id">The user_id</param>
        /// <returns>User object of the user of the error code</returns>
        public object getUser(int user_id)
        {
            using (DBUtil util = new DBUtil())
            {
                DataSet dsUser = util.getDataSet(@"SELECT * 
                                                    FROM tb_user 
                                                    WHERE user_id = @userID",
                                                    "tb_user",
                                                    new string[] { "@userID" },
                                                    new object[] { user_id });

                if (dsUser.Tables["tb_user"].Rows.Count == 0)
                {
                    return USER_NOT_EXISTS;
                }

                #region Assign User Data
                DataRow row = dsUser.Tables["tb_user"].Rows[0];
                User user = new User();
                user.UserID = (int)row["user_id"];
                user.LoginID = (string)row["login_id"];
                user.UserName = (string)row["user_name"];
                user.Email = (string)row["email"];
                user.Remarks = (row["remarks"] is DBNull) ? "" : (string)row["remarks"];
                user.Status = (Int16)row["status"];
                #endregion

                #region Assign User Function
                DataSet dsUserFunction = util.getDataSet(@"SELECT * 
                                                            FROM tb_user_function
                                                            WHERE user_id = @userId
                                                            AND status = @status",
                                                            "tb_user_function",
                                                            new string[] { "@userId", "@status" },
                                                            new object[] { user.UserID, (int)RecordStatusEnum.Active });

                ArrayList userFunction = new ArrayList();
                foreach (DataRow ufrow in dsUserFunction.Tables["tb_user_function"].Rows)
                {
                    userFunction.Add(ufrow["function_id"]);
                }

                user.UserFunction = userFunction;
                #endregion

                return user;
            }
        }

        /// <summary>
        /// To get a list of users for the user_list
        /// </summary>
        /// <param name="user">The User who are logined to the system</param>
        /// <param name="loginId">The search criteria Login ID</param>
        /// <param name="username">The search criteria User Name</param>
        /// <param name="masterDeptId">The search criteria Master Deptement</param>
        /// <param name="status">The search criteria user status</param>
        /// <returns>Return object array of 
        /// a dataset of the users that the logined user can access
        /// and the sql.
        /// Null criteria will be excluded</returns>
        public object[] getUserList(User user, string loginId, string username, string status)
        {
            BidirHashtable<object, EnumValueAttribute> recordStatusMap = EnumConvertUtils.EnumToAttributeMap(typeof(RecordStatusEnum));

            string sql = @"SELECT a.user_id, a.login_id, a.user_name, a.email, ";
            sql += "CASE a.status ";
            foreach(string recordStatus in Enum.GetNames(typeof(RecordStatusEnum)))
            {
                sql += "WHEN " + Convert.ToString((int)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DbValue) +
                        " THEN '" + (string)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DisplayValue + "' "; 
            }
            sql += @"END AS status 
                    FROM tb_user a
                    WHERE 1=1 ";

            if (loginId != null)
            {
                sql += "AND a.login_id LIKE '%" + loginId.Replace('\'', '"') + "%' ";
            }
            if (username != null)
            {
                sql += "AND a.user_name LIKE '%" + username.Replace('\'', '"') + "%' ";
            }
            if (status != null)
            {
                sql += "AND a.status = " + status + " ";
            }

            using (DBUtil util = new DBUtil())
            {
                return new object[]{util.getDataSet(sql, "tb_user"), sql};
            }
        }

        /// <summary>
        /// To get a list of users for the user_list by sql
        /// </summary>
        /// <param name="sql">The sql that want to execute</param>
        /// <returns>Return object array of 
        /// a dataset of the users that the logined user can access
        /// and the sql.</returns>
        public object[] getUserList(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return new object[] { null, sql };
            }
            using (DBUtil util = new DBUtil())
            {
                return new object[]{util.getDataSet(sql, "tb_user"), sql};
            }
        }

        /// <summary>
        /// To get the assigned right of a user
        /// </summary>
        /// <param name="user">The login user</param>
        /// <param name="user_id">The user id of the selected user</param>
        /// <returns>DataSet of the user_function</returns>
        public DataSet getAssignedRight(User user, string user_id)
        {
            string sql = @"SELECT a.user_name, b.user_id, b.function_id
                            FROM tb_user a, tb_user_function b
                            WHERE a.user_id = b.user_id
                            AND b.status = @status ";

            if (user_id != null)
            {
                sql += "AND a.user_id = " + Convert.ToInt32(user_id) + " ";
            }

            using (DBUtil util = new DBUtil())
            {
                return util.getDataSet(sql, 
                                        "tb_user_function", 
                                        new string[] { "@status" }, 
                                        new object[] { (int)RecordStatusEnum.Active });
            }
        }

        /// <summary>
        /// 获取拥有buy request的管理员资料
        /// </summary>
        /// <returns></returns>
        public DataTable getSellRequestAdmin()
        {
            string sql = "select u.* from tb_user u Inner join tb_user_function uf ON u.user_id=uf.user_id where uf.function_id =8 and uf.status=0";

            using (DBUtil util = new DBUtil())
            {
                return util.getDataSet(sql,
                                        "tb_user_function",
                                        new string[] { "@status" },
                                        new object[] { (int)RecordStatusEnum.Active }).Tables[0];
            }
        }

        /// <summary>
        /// 获取拥有buy request的管理员资料
        /// </summary>
        /// <returns></returns>
        public DataTable getBuyRequestAdmin()
        {
            string sql = "select u.* from tb_user u Inner join tb_user_function uf ON u.user_id=uf.user_id where uf.function_id =7 and uf.status=0";

            using (DBUtil util = new DBUtil())
            {
                return util.getDataSet(sql,
                                        "tb_user_function",
                                        new string[] { "@status" },
                                        new object[] { (int)RecordStatusEnum.Active }).Tables[0];
            }
        }

        /// <summary>
        /// To delete a User
        /// </summary>
        /// <param name="userlist">A string of selected user seperated by ','</param>
        /// <returns>An arraylist of those have transactions' user id</returns>
        public ArrayList deleteUser(string userlist)
        {
            ArrayList result = new ArrayList();

            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        string[] user_id = userlist.Split(',');
                        for (int i = 0; i < user_id.Length; i++)
                        {
                            string sql = @"SELECT dbo.CheckUserTrans(@user_id)";
                            int count = (int)util.executeScalar(sql,
                                                            new string[] { "@user_id" },
                                                            new object[] { Convert.ToInt32(user_id[i]) },
                                                            tx);

                            if (count > 0)
                            {
                                result.Add(user_id[i]);
                            }
                            else
                            {
                                sql = @"DELETE FROM tb_user 
                                                WHERE user_id = @user_id";
                                util.executeNonQuery(sql, 
                                                        new string[]{"@user_id"},
                                                        new object[]{Convert.ToInt32(user_id[i])},
                                                        tx);
                                sql = @"DELETE FROM tb_user_function 
                                                WHERE user_id = @user_id";
                                util.executeNonQuery(sql,
                                                        new string[] { "@user_id" },
                                                        new object[] { Convert.ToInt32(user_id[i]) },
                                                        tx);
                            }
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

        /// <summary>
        /// To add a new user
        /// </summary>
        /// <param name="user">The login user</param>
        /// <param name="login_id">The login id of the user</param>
        /// <param name="userName">The username of the user</param>
        /// <param name="email">The user email address</param>
        /// <param name="districtID">The RCY district id</param>
        /// <param name="unitID">The RCY unit no id(null if the user is not a rcy type)</param>
        /// <param name="remark">The remarks</param>
        /// <param name="status">The status</param>
        /// <param name="right">The access right</param>
        /// <returns>The user_id if successed, the error code if fail</returns>
        public int addUser(User user, string login_id, string userName, string email,
                                string remark, int status, 
                                ArrayList right)
        {
            int user_id = -1;

            using (DBUtil util = new DBUtil()){
                using (DbTransaction tx = util.getTransaction()){
                    try
                    {
                        #region Validation
                        /****************Check the login id*****************/
                        string sql = @"SELECT COUNT(*) 
                                    FROM tb_user
                                    WHERE login_id = @loginID";

                        int i = (int)util.executeScalar(sql,
                                                    new string[] { "@loginID" },
                                                    new object[] { login_id },
                                                    tx);
                        if (i > 0)
                        {
                            tx.Rollback();
                            return LOGIN_ID_DUPLICATED;
                        }
                        #endregion

                        #region Set Passwrod
                        string password = this.getDefaultPasswd(0);
                        #endregion

                        #region Set User_id
                        sql = @"SELECT MAX(user_id) + 1
                                    FROM tb_user";

                        user_id = (int)util.executeScalar(sql, tx);
                        #endregion

                        #region Insert
                        sql = @"INSERT INTO tb_user
                                        (user_id, login_id, password, user_name, email, 
                                        remarks, status, 
                                        created_user, created_date, updated_user, updated_date) VALUES
                                        (@user_id, @loginID, @passwd, @userName, @email,
                                        @remark, @status,
                                        @user, getDate(), @user, getDate())";

                        util.executeNonQuery(sql,
                                            new string[]{"@user_id", "@loginID", "@passwd", "@userName", 
                                                "@email", "@remark", "@status", "@user"},
                                            new object[]{user_id, login_id, encrypt(password), userName,
                                                email, remark, status, user.UserID},
                                            tx);

                        

                        setRight(user, user_id, right, util, tx);
                        #endregion

                        tx.Commit();
                    }
                    catch(Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
            return user_id;
        }

        /// <summary>
        /// To edit the information of a user 
        /// </summary>
        /// <param name="user">The login user</param>
        /// <param name="user_id">The user_id of the user who want to edit</param>
        /// <param name="login_id">The login id of the user</param>
        /// <param name="userName">The username of the user</param>
        /// <param name="email">The user email address</param>
        /// <param name="districtID">The RCY district id</param>
        /// <param name="unitID">The RCY unit no id(null if the user is not a rcy type)</param>
        /// <param name="remark">The remarks</param>
        /// <param name="status">The status</param>
        /// <param name="right">The access right</param>
        public void editUser(User user, int user_id, string login_id, string userName, string email,
                                string remark, int status,
                                ArrayList right)
        {
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                       #region Edit
                        string sql = @"UPDATE tb_user 
                                        SET login_id = @loginID,
                                            user_name = @userName, 
                                            email = @email,
                                            remarks = @remark, 
                                            status = @status,
                                            updated_user = @update, 
                                            updated_date = getDate()
                                        WHERE user_id = @user_id";

                        util.executeNonQuery(sql,
                                            new string[]{"@user_id", "@loginID", "@userName",
                                                "@email", "@remark", "@status", "@update"},
                                            new object[]{user_id, login_id, userName, 
                                                email, remark, status, user.UserID},
                                            tx);

                        setRight(user, user_id, right, util, tx);
                        #endregion

                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// To check if the password correct
        /// </summary>
        /// <param name="user_id">The user's user_id</param>
        /// <param name="password">The password</param>
        /// <returns>True if the password correct, otherwise false</returns>
        public bool checkPasswd(int user_id, string password)
        {
            using (DBUtil util = new DBUtil())
            {
                DataSet dsUser = util.getDataSet(@"SELECT * 
                                                    FROM tb_user 
                                                    WHERE user_id = @userID",
                                                    "tb_user",
                                                    new string[] { "@userID" },
                                                    new object[] { user_id });

                if (dsUser.Tables["tb_user"].Rows.Count == 0)
                {
                    return false;
                }

                if ((string)dsUser.Tables["tb_user"].Rows[0]["password"] == this.encrypt(password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// To set password of a user
        /// </summary>
        /// <param name="user">The login user</param>
        /// <param name="user_id">The user_id who want to reset password</param>
        /// <param name="password">The new password</param>
        public void setPasswd(User user, int user_id, string password)
        {
            using (DBUtil util = new DBUtil())
            {
                string sql = @"UPDATE tb_user
                                SET password = @password,
                                    updated_user = @user,
                                    updated_date = getDate()
                                WHERE user_id = @user_id";
                util.executeNonQuery(sql,
                                     new string[] { "@password", "@user", "@user_id" },
                                     new object[]{this.encrypt(password), 
                                                    user.UserID,
                                                    user_id});
            }
        }

        /// <summary>
        /// To reset the password
        /// </summary>
        /// <param name="user">The login user</param>
        /// <param name="user_id">The user_id who want to reset password</param>
        /// <param name="type">The user type of the user who want to reset password</param>
        public void resetPasswd(User user, int user_id, int type)
        {
            this.setPasswd(user, user_id, this.getDefaultPasswd(type));
        }

        /// <summary>
        /// To set the access right of a user
        /// </summary>
        /// <param name="user">The login user</param>
        /// <param name="user_id">The user_id of the user who want to set</param>
        /// <param name="right">The Arraylist of string of the function_id</param>
        /// <param name="util">The DBUtil with the DBConnection using</param>
        /// <param name="tx">The transaction using</param>
        private void setRight(User user, int user_id, ArrayList right, DBUtil util, DbTransaction tx)
        {
            string sql = @"DELETE FROM tb_user_function
                                WHERE user_id = @userID";

            util.executeNonQuery(sql,
                                new string[] { "@userID" },
                                new object[] { user_id },
                                tx);

            for (int i = 0; i < right.Count; i++)
            {
                sql = @"INSERT INTO tb_user_function
                            (user_id, function_id, status, created_user, created_date) VALUES
                            (@user_id, @function_id, @status, @created, getDate())";
                util.executeNonQuery(sql,
                                    new string[]{"@user_id",
                                                    "@function_id",
                                                    "@status",
                                                    "@created"},
                                    new object[]{user_id,
                                                    Convert.ToInt32((string)right[i]),
                                                    (int)RecordStatusEnum.Active,
                                                    user.UserID},
                                    tx);
            }
        }

        /// <summary>
        /// To get the default password of the user type
        /// </summary>
        /// <param name="type">The user type</param>
        /// <returns>The default password</returns>
        private string getDefaultPasswd(int type)
        {
            return "max123";
        }

        /// <summary>
        /// To encrypt an string by using MD5 32-byte encryption
        /// </summary>
        /// <param name="value">The string want to encrypt</param>
        /// <returns>The encrypted string</returns>
        public string encrypt(string value)
        {
            string pwd = "";
            MD5 md5 = MD5.Create(); 
            byte[] s = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value)); 
            for (int i = 0; i < s.Length; i++) 
            { 
                pwd = pwd + s[i].ToString("X");
            } 
            return pwd; 
        } 
    }
}
