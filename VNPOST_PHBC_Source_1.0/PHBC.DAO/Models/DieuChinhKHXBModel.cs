namespace PHBC.DAO.Models
{
using PHBC.DAO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
    using System.Text.RegularExpressions;
    public class DieuChinhKHXBModel
    {
        
    }


    public class DieuChinhKHXBEditModel
    {
        public DieuChinhKHXBEditModel()
        {
            lstEdit = new List<DCThongTinBaoModel>();
        }
        public List<DCThongTinBaoModel> lstEdit { get; set; }
        public string Config { get; set; }

        public int LoaiDieuChinh { get; set; }

        public string Id { get; set; }

    }

    public class DCThongTinBaoModel{
        public string Key { get; set; }
        public int KieuDuLieu { get; set; }
        public string Dislay { get; set; }
        public string Value { get; set; }
    }
    public class DCThongTinBaoEditModel
    {
        public DCThongTinBaoEditModel()
        {
            KieuDuLieu = (int)Enums.KieuDuLieu.text;
        }
        public string Key { get; set; }
        public int KieuDuLieu { get; set; }
        public bool IsActive { get; set; }
        public string Dislay { get; set; }
        
        public string getNameKieuDuLieu()
        {
            switch (KieuDuLieu)
            {
                case (int)Enums.KieuDuLieu.Interger:
                    return "IntergerValue";
                case (int)Enums.KieuDuLieu.Numeric:
                    return "NumericValue";
                case (int)Enums.KieuDuLieu.Date:
                    return "DateValue";
                case (int)Enums.KieuDuLieu.PhoneNumber:
                    return "PhoneNumberValue";
                case (int)Enums.KieuDuLieu.Email:
                    return "EmailValue";
                default:
                    return "StringValue";
            }
        }
        //danh sach kieu dữ liệu
        public void setValue(object value)
        {
            if (value == null)
            {
                this.Value = null;
                this.KieuDuLieu = (int)Enums.KieuDuLieu.text;
                return;
            }
            KieuDuLieu = (int)Utils.getKieuDuLieu(value.GetType());
            this.Value = value.ToString();
        }
        public void setValue(object value, Type _type)
        {
            if (_type == null)
            {
                value = null;
                KieuDuLieu = (int)Enums.KieuDuLieu.text;
                return;
            }
            KieuDuLieu = (int)Utils.getKieuDuLieu(_type);
            if (value == null)
            {
                value = null;
                return;
            }
            this.Value = value.ToString();
        }
        //[Required(ErrorMessage = "Không được để trống")]
        public string Value { get; set;}
        //[Required(ErrorMessage = "Không được để trống")]
        [StringLength(500, ErrorMessage = "Không được quá {1} ký tự.")]
        public string StringValue {
            get
            {
                if (KieuDuLieu == (int)Enums.KieuDuLieu.Interger)
                    return this.Value;
                return null;
            }
            set { this.Value = value; }
        }
        //[Required(ErrorMessage = "Không được để trống")]
        [RegularExpression(Enums.RegexDefine.Interger, ErrorMessage = "Bạn chỉ được nhập số nguyên")]
        [Range(0, 1000000, ErrorMessage = "Phải có giá trị từ {1} tới {2}")]
        public string IntergerValue
        {
            get { 
                if(KieuDuLieu == (int)Enums.KieuDuLieu.Interger)
                    return this.Value;
                return null;
            }
            set { this.Value = value; }
        }
        //[Required(ErrorMessage = "Không được để trống")]
        [RegularExpression(Enums.RegexDefine.Numeric, ErrorMessage = "Bạn chỉ được nhập sô")]
        [Range(0, 1000000, ErrorMessage="Phải có giá trị từ {1} tới {2}")]
        public string NumericValue
        {
            get
            {
                if (KieuDuLieu == (int)Enums.KieuDuLieu.Numeric)
                    return this.Value;
                return null;
            }
            set { this.Value = value; }
        }
        //[Required(ErrorMessage = "Không được để trống")]
        [RegularExpression(Enums.RegexDefine.PhoneNumber, ErrorMessage = "Bạn phải nhập số điện thoại")]
        public string PhoneNumberValue
        {
            get
            {
                if (KieuDuLieu == (int)Enums.KieuDuLieu.PhoneNumber)
                    return this.Value;
                return null;
            }
            set { this.Value = value; }
        }
        //[Required(ErrorMessage = "Không được để trống")]
        [EmailAddress(ErrorMessage="Bạn phải nhập email")]
        [StringLength(50, ErrorMessage = "Không được quá {1} ký tự.")]
        public string EmailValue
        {
            get
            {
                if (KieuDuLieu == (int)Enums.KieuDuLieu.Email)
                    return this.Value;
                return null;
            }
            set { this.Value = value; }
        }
        [DataType(DataType.Date, ErrorMessage="Bạn phải nhập ngày")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public string DateValue
        {
            get
            {
                if (KieuDuLieu == (int)Enums.KieuDuLieu.Date)
                    return this.Value;
                return null;
            }
            set { this.Value = value; }
        }
        public bool BooleanValue
        {
            get { 
                if (string.IsNullOrWhiteSpace(this.Value)) 
                    return false;
                if (KieuDuLieu == (int)Enums.KieuDuLieu.Bool)
                    return Convert.ToBoolean(this.Value);
                else
                    return false;
            }
            set { this.Value = value.ToString(); }
        }
    }
}
