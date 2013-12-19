using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections;

namespace SunStar_CMS.admin.Classes.Objects
{
    public class Product
    {
        int _ProductId;

        public int ProductId
        {
            get { return _ProductId; }
            set { _ProductId = value; }
        }
        int _CategoryId;

        public int CategoryId
        {
            get { return _CategoryId; }
            set { _CategoryId = value; }
        }
        string _ModelNo;

        public string ModelNo
        {
            get { return _ModelNo; }
            set { _ModelNo = value; }
        }

        string _EngName;

        public string EngName
        {
            get { return _EngName; }
            set { _EngName = value; }
        }
        string _ChnName;

        public string ChnName
        {
            get { return _ChnName; }
            set { _ChnName = value; }
        }
        string _HtmlContent;

        public string HtmlContent
        {
            get { return _HtmlContent; }
            set { _HtmlContent = value; }
        }
        string _Remarks;

        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        int _Status;

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        ArrayList _FactorList=new ArrayList();

        public ArrayList FactorList
        {
            get { return _FactorList; }
            set { _FactorList = value; }
        }

        

        List<ProductAttachment> _Attachement = new List<ProductAttachment>();

        public List<ProductAttachment> Attachement
        {
            get { return _Attachement; }
            set { _Attachement = value; }
        }
        List<ProductImage> _Image = new List<ProductImage>();

        public List<ProductImage> Image
        {
            get { return _Image; }
            set { _Image = value; }
        }
        
    }

    public class ProductAttachment
    {
        int _AttachmentId;

        public int AttachmentId
        {
            get { return _AttachmentId; }
            set { _AttachmentId = value; }
        }
        int _ProductId;

        public int ProductId
        {
            get { return _ProductId; }
            set { _ProductId = value; }
        }
        string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        string _Path;

        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }

        private bool _WillDelete;

        public bool WillDelete
        {
            get { return _WillDelete; }
            set { _WillDelete = value; }
        }

        internal static ProductAttachment getObjectByDr(DataRow dr)
        {
            ProductAttachment s = new ProductAttachment();
            s.AttachmentId = Convert.ToInt32(dr["attachment_id"]) + 0;
            s.ProductId = (int)dr["product_id"];
            s.Name = dr["title"].ToString();
            s.Path = dr["path"].ToString();

            return s;
        }
    }

    public class ProductImage
    {
        int _ImageId;

        public int ImageId
        {
            get { return _ImageId; }
            set { _ImageId = value; }
        }

        int _ProductId;

        public int ProductId
        {
            get { return _ProductId; }
            set { _ProductId = value; }
        }
        string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        string _Path;

        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }
        int _IsDefault;

        public int IsDefault
        {
            get { return _IsDefault; }
            set { _IsDefault = value; }
        }

        string _Priority;

        public string Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

        bool _WillDelete = false;

        public bool WillDelete
        {
            get { return _WillDelete; }
            set { _WillDelete = value; }
        }



        internal static ProductImage getObjectByDr(DataRow dr)
        {
            ProductImage s = new ProductImage();
            s.ImageId = Convert.ToInt32(dr["image_id"])+0;
            s.ProductId = (int)dr["product_id"];
            s.Title = dr["title"].ToString();
            s.Path = dr["path"].ToString();
            s.IsDefault = Convert.ToInt32(dr["is_default"]) + 0;
            s.Priority = dr["priority"].ToString();

            return s;
        }
    }
}
