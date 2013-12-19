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
using SunStar_CMS.admin.Classes.Utils;
using System.Data.Common;
using SunStar_CMS.admin.Classes.Objects;
using SunStar_CMS.admin.Classes.ControlValues;
using System.IO;

namespace SunStar_CMS.admin.Classes.Mgr
{
    public class ProductMgr
    {
        internal object[] getProductList(SunStar_CMS.admin.Classes.Objects.User user,string modelNo, string engName, string chnName,  string category_id, string status)
        {
            BidirHashtable<object, EnumValueAttribute> recordStatusMap = EnumConvertUtils.EnumToAttributeMap(typeof(RecordStatusEnum));

            string sql = @"SELECT a.product_id,a.model_no, a.eng_name, a.chn_name,b.chn_name category_name, a.remarks, ";
            sql += "CASE a.status ";
            foreach (string recordStatus in Enum.GetNames(typeof(RecordStatusEnum)))
            {
                sql += "WHEN " + Convert.ToString((int)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DbValue) +
                        " THEN '" + (string)recordStatusMap[Enum.Parse(typeof(RecordStatusEnum), recordStatus)].DisplayValue + "' ";
            }
            sql += @"END AS status 
                    FROM tb_product a
inner join tb_category b on a.category_id=b.category_id
                    WHERE 1=1 ";

            if (modelNo != null)
            {
                sql += "AND a.model_no LIKE '%" + modelNo.Replace('\'', '"') + "%' ";
            }
            if (engName != null)
            {
                sql += "AND a.eng_name LIKE '%" + engName.Replace('\'', '"') + "%' ";
            }
            if (chnName != null)
            {
                sql += "AND a.chn_name LIKE '%" + chnName.Replace('\'', '"') + "%' ";
            }
            if (category_id != null)
            {
                if (Convert.ToInt32(category_id) > 0)
                {
                    sql += "AND b.category_id = " + category_id + " ";
                }
            }
            if (status != null)
            {
                sql += "AND a.status = " + status + " ";
            }

            using (DBUtil util = new DBUtil())
            {
                return new object[] { util.getDataSet(sql, "tb_product"), sql };
            }
        }

        internal System.Collections.ArrayList deleteProduct(string productList)
        {
            ArrayList result = new ArrayList();
            string sql = "";
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        string[] product_id = productList.Split(',');
                        for (int i = 0; i < product_id.Length; i++)
                        {
                            sql = @"DELETE FROM tb_product 
                                                WHERE product_id = @product_id";
                            util.executeNonQuery(sql,
                                                    new string[] { "@product_id" },
                                                    new object[] { Convert.ToInt32(product_id[i]) },
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

        internal object getProductObject(int product_id)
        {
            string sql = "select * from tb_product where product_id=" + product_id.ToString();

            DataTable dt = null;
            using (DBUtil util = new DBUtil())
            {
                dt = util.getDataSet(sql, "tb_product").Tables[0];

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    Product product = new Product();
                    product.ProductId = (int)dr["product_id"];
                    product.ModelNo = dr["model_no"].ToString();
                    product.CategoryId = (int)dr["category_id"];
                    product.ChnName = dr["chn_name"].ToString();
                    product.EngName = dr["eng_name"].ToString();
                    product.Remarks = dr["remarks"].ToString();
                    product.Status = (int)dr["status"];
                    product.HtmlContent = dr["html_content"].ToString();

                    sql = "select * from tb_product_factor where product_id="+product_id.ToString();
                    DataTable dtFactor=util.getDataSet(sql,"tb_product_factor").Tables[0];
                    foreach (DataRow drFa in dtFactor.Rows)
                    {
                        product.FactorList.Add(drFa["factor_id"]);
                    }

                    return product;
                }
                else
                {
                    return null;
                }
            }
        }

        internal DataTable getProductByCategoryDataTable(int category_id)
        {
            using (DBUtil util = new DBUtil())
            {
                string sql = "select * from tb_product where category_id=" + category_id + " order by chn_name";
                return util.getDataSet(sql, "tb_product").Tables[0];
            }
        }

        internal DataSet getProductFactorDataSet(User user, string product_id)
        {
            using (DBUtil util = new DBUtil())
            {
                string sql = "select * from tb_product_factor where product_id="+product_id;
                return util.getDataSet(sql, "tb_product_factor");
            }
        }

        internal int addProduct(User user, Product objproduct)
        {
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        int product_id = util.getMasterId("tb_product", tx);
                        string sql = @"insert into tb_product (product_id,model_no,category_id,eng_name,chn_name,remarks,status,html_content,created_user,created_date,updated_user,updated_date) 
values (@product_id,@model_no,@category_id,@eng_name,@chn_name,@remarks,@status,@html_content,@user_id,getdate(),@user_id,getdate()) ";
                        string[] param_name = new string[] { "@product_id", "@model_no", "@category_id", "@eng_name", "@chn_name", "@remarks", "@status", "@html_content", "@user_id" };
                        object[] param_value = new object[] { product_id, objproduct.ModelNo, objproduct.CategoryId, objproduct.EngName, objproduct.ChnName, objproduct.Remarks, objproduct.Status, objproduct.HtmlContent, user.UserID };
                        util.executeNonQuery(sql, param_name, param_value, tx);

                        string _FileDir = HttpContext.Current.Server.MapPath(Path.GetDirectoryName("~/upload_files/attachments/" + product_id.ToString()+"/"));
                        if (!Directory.Exists(_FileDir))
                        {
                            Directory.CreateDirectory(_FileDir);
                        }

                        _FileDir = HttpContext.Current.Server.MapPath(Path.GetDirectoryName("~/upload_files/images/" + product_id.ToString() + "/"));
                        if (!Directory.Exists(_FileDir))
                        {
                            Directory.CreateDirectory(_FileDir);
                        }

                        for (int i = 0; i < objproduct.FactorList.Count; i++)
                        {
                            sql = @"INSERT INTO tb_product_factor
                            (product_id, factor_id) VALUES
                            (@product_id, @factor_id)";
                            util.executeNonQuery(sql,
                                                new string[]{"@product_id",
                                                    "@factor_id"},
                                                new object[]{product_id,
                                                    Convert.ToInt32((string)objproduct.FactorList[i])},
                                                tx);
                        }


                        tx.Commit();
                        return product_id;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        internal void editProduct(User user, Product objproduct)
        {
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        string sql = @" update tb_product set 
model_no=@model_no,
category_id=@category_id,
eng_name=@eng_name,
chn_name=@chn_name,
remarks=@remarks,
status=@status,
html_content=@html_content,
updated_user=@user_id,
updated_date=getdate()
where product_id=@product_id";
                        string[] param_name = new string[] { "@product_id","@model_no", "@category_id", "@eng_name", "@chn_name", "@remarks", "@status", "@html_content", "@user_id" };
                        object[] param_value = new object[] { objproduct.ProductId,objproduct.ModelNo, objproduct.CategoryId, objproduct.EngName, objproduct.ChnName, objproduct.Remarks, objproduct.Status, objproduct.HtmlContent, user.UserID };
                        util.executeNonQuery(sql, param_name, param_value,tx);


                        sql = "delete from tb_product_factor where product_id=" + objproduct.ProductId.ToString();

                        util.executeNonQuery(sql, tx);

                        for (int i = 0; i < objproduct.FactorList.Count; i++)
                        {
                            sql = @"INSERT INTO tb_product_factor
                            (product_id, factor_id) VALUES
                            (@product_id, @factor_id)";
                            util.executeNonQuery(sql,
                                                new string[]{"@product_id",
                                                    "@factor_id"},
                                                new object[]{objproduct.ProductId,
                                                    Convert.ToInt32((string)objproduct.FactorList[i])},
                                                tx);
                        }


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

        internal void insertImage(int product_id, string fileName)
        {
            using (DBUtil util = new DBUtil())
            { using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        int image_id = util.getMasterId("tb_product_image", tx);
                        string sql = "insert into tb_product_image (image_id,product_id,title,path,is_default) values ";
                        sql += "(@image_id,@product_id,'',@fileName,1)";

                        string[] param_name = new string[] { "@image_id", "@product_id", "@fileName" };
                        object[] param_value = new object[] { image_id,product_id,fileName };
                        util.executeNonQuery(sql, param_name, param_value, tx);

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

        internal void insertAttachment(int product_id, string fileName)
        {
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        int attachment_id = util.getMasterId("tb_product_attachment", tx);
                        string sql = "insert into tb_product_attachment (attachment_id,product_id,name,path) values ";
                        sql += "(@attachment_id,@product_id,@fileName,@fileName)";

                        string[] param_name = new string[] { "@attachment_id", "@product_id", "@fileName" };
                        object[] param_value = new object[] { attachment_id, product_id, fileName };
                        util.executeNonQuery(sql, param_name, param_value, tx);

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

        internal DataSet getImageDataSet(int product_id)
        {
            using (DBUtil util = new DBUtil())
            {
                string sql = "select * from tb_product_image where product_id="+product_id.ToString();
                sql += " order by is_default,priority,title,path,image_id";

                return util.getDataSet(sql, "tb_product_image");
            }
        }

        internal void UpdateImage(System.Collections.Generic.List<ProductImage> lpi)
        {
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        string sql = "";
                        for (int i = 0; i < lpi.Count; i++)
                        {
                            ProductImage pi = lpi[i];
                            if (pi.WillDelete)
                            {
                                sql = "delete from tb_product_image where image_id="+pi.ImageId.ToString();
                                util.executeNonQuery(sql, tx);
                            }
                            else
                            {
                                sql = @"update  tb_product_image set
title=@title,is_default=@is_default,priority=@priority where image_id=@image_id
";

                                string[] param_name = new string[] { "@image_id", "@title", "@is_default", "@priority" };
                                object[] param_value = new object[] { pi.ImageId, pi.Title, pi.IsDefault, pi.Priority };
                                util.executeNonQuery(sql, param_name, param_value, tx);

                            }
                        }
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


        internal string getAttachmentFile(string pid)
        {
            using (DBUtil util = new DBUtil())
            {
                string sql = "select * from tb_product_attachment where  attachment_id="+pid;
                DataTable dt = util.getDataSet(sql, "tb_product_attachment").Tables[0];
                if (dt.Rows.Count <= 0)
                {
                    return "";
                }

                return dt.Rows[0]["path"].ToString();

            }
        }

        internal DataSet getAttachmentDataSet(int product_id)
        {
            //throw new Exception("The method or operation is not implemented.");
            using (DBUtil util = new DBUtil())
            {
                string sql = "select * from tb_product_attachment where product_id=" + product_id.ToString();
                sql += " order by name,path";

                return util.getDataSet(sql, "tb_product_attachment");
            }
        }

        internal void UpdateAttachment(System.Collections.Generic.List<ProductAttachment> lpi)
        {
            using (DBUtil util = new DBUtil())
            {
                using (DbTransaction tx = util.getTransaction())
                {
                    try
                    {
                        string sql = "";
                        for (int i = 0; i < lpi.Count; i++)
                        {
                            ProductAttachment pi = lpi[i];
                            if (pi.WillDelete)
                            {
                                sql = "delete from tb_product_attachment where attachment_id=" + pi.AttachmentId.ToString();
                                util.executeNonQuery(sql, tx);
                            }
                            else
                            {
                                sql = @"update  tb_product_attachment set
name=@name where attachment_id=@attachment_id
";

                                string[] param_name = new string[] { "@attachment_id", "@name" };
                                object[] param_value = new object[] { pi.AttachmentId, pi.Name };
                                util.executeNonQuery(sql, param_name, param_value, tx);

                            }
                        }
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

        internal object[] getProductList(string sql)
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
    }
}
