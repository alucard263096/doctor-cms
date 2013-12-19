using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;


namespace SunStar_CMS.admin.Classes.Utils
{
    public class UploadFile
    {
        public int DeleteFile(string strSavePath, string strFileName)
        {
            string strCheckFile = strSavePath + strFileName;
            try
            {
                if (File.Exists(strCheckFile))
                {
                    File.Delete(strCheckFile);
                    return 0;
                }
                else
                {
                    return -2;
                }

            }
            catch
            {
                return -1;
            }

        }

        public string SaveFile(string strSavePath, FileUpload objFile, string strNewName, bool bolOverwrite)
        {
            if (objFile.HasFile)
            {
                if (objFile.FileName.Length > 0)
                {
                    string strTempFileName = "";
                    string strFileName = objFile.FileName;
                    if (strNewName.Length > 0)
                    {
                        strFileName = strNewName + Path.GetExtension(objFile.FileName);
                    }
                    //replace &
                    strFileName = strFileName.Replace('&', '_');
                    strFileName = strFileName.Replace('%', '_');

                    string strCheckFile = strSavePath + strFileName;
                    try
                    {
                        if (bolOverwrite)
                        {
                            if (File.Exists(strCheckFile))
                            {
                                int intReturn = DeleteFile(strSavePath, strCheckFile);
                            }
                        }
                        else
                        {
                            if (File.Exists(strCheckFile))
                            {
                                int intCounter = 2;
                                while (File.Exists(strCheckFile))
                                {
                                    strTempFileName = intCounter.ToString() + "_" + strFileName;
                                    strCheckFile = strSavePath + strTempFileName;
                                    intCounter++;
                                }
                                strFileName = strTempFileName;
                            }
                        }
                        strSavePath += strFileName;
                        objFile.SaveAs(strSavePath);
                        return strFileName;
                    }
                    catch (Exception e)
                    {
                        return "Error : " + e.ToString();
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }


    }
}
