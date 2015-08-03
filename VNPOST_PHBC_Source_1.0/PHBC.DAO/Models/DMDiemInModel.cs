using PHBC.DAO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;

namespace PHBC.DAO.Models
{
    public class DMDiemInModel
    {
        public DMDiemInModel()
        {
        }

        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Huyện")]
        public string DistrictCode { get; set; }

        [Display(Name = "Tên Huyện")]
        public string DistrictName { get; set; }
       
        public DMDiemInModel(DMDiemIn DMDiemIn)
        {
            this.DistrictCode = DMDiemIn.DistrictCode;
            this.MaDiemIn = DMDiemIn.MaDiemIn;
            this.TenDiemIn = DMDiemIn.TenDiemIn;
            this.ProvinceCode = DMDiemIn.ProvinceCode;
            this.DistrictCode = DMDiemIn.DistrictCode;
            this.DiaChi = DMDiemIn.DiaChi;
            this.ProvinceName = DMDiemIn.Province != null ? DMDiemIn.Province.ProvinceName : "";
            this.Province = DMDiemIn.Province;
            this.DistrictName = DMDiemIn.District != null ? DMDiemIn.District.DistrictName : "";
            this.Province = DMDiemIn.Province;
            this.Id = DMDiemIn.Id;
            this.Status = DMDiemIn.Status;
            this.CreateBy = DMDiemIn.CreateBy;
            this.CreateDate = DMDiemIn.CreateDate;
            this.ModifyBy = DMDiemIn.ModifyBy;
            this.ModifyDate = DMDiemIn.ModifyDate;
        }
        
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(50, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Mã điểm in")]
        public string MaDiemIn { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(100, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Tên điểm in")]
        public string TenDiemIn { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(256, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(256)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Mã tỉnh")]
        public string ProvinceCode { get; set; }
        [Display(Name = "Tên tỉnh")]
        public string ProvinceName { set; get; }
        public virtual Province Province { get; set; }
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        public int Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public string userId { get; set; }
        public DMDiemIn toDMDiemIn()
        {
            DMDiemIn result = new DMDiemIn();
            result.Id = string.IsNullOrEmpty(this.Id) ? Guid.NewGuid().ToString() : this.Id;
            result.DistrictCode = this.DistrictCode;
            result.MaDiemIn = this.MaDiemIn;
            result.TenDiemIn = this.TenDiemIn;
           
            result.DiaChi = this.DiaChi;
            result.ProvinceCode = this.ProvinceCode;
            result.Province = this.Province;
            result.Status = this.Status;
            result.CreateBy = this.CreateBy;
            result.CreateDate = this.CreateDate;
            result.ModifyBy = this.ModifyBy;
            result.ModifyDate = this.ModifyDate;
            return result;
        }
        public DMDiemIn toCreate()
        {
            DMDiemIn result = new DMDiemIn();
            result.Id = string.IsNullOrEmpty(this.Id) ? Guid.NewGuid().ToString() : this.Id;
            result.MaDiemIn = this.MaDiemIn;
            result.TenDiemIn = this.TenDiemIn;

            result.DiaChi = this.DiaChi;
            result.ProvinceCode = this.ProvinceCode;
            result.DistrictCode = this.DistrictCode;


            result.CreateBy = this.userId;
            result.CreateDate = DateTime.Now;
            result.Status = (int)Enums.RecordStatusCode.active;
            return result;
        }
        public void changeEdit(DMDiemIn diemInEdit)
        {
            diemInEdit.MaDiemIn = this.MaDiemIn;
            diemInEdit.TenDiemIn = this.TenDiemIn;
            diemInEdit.DiaChi = this.DiaChi;

            diemInEdit.ProvinceCode = this.ProvinceCode;
            diemInEdit.DistrictCode = this.DistrictCode;

            diemInEdit.ModifyBy = this.userId;
            diemInEdit.ModifyDate = DateTime.Now;
        }
    }

    public class DMDiemInSearchModel
    {
        public DMDiemInSearchModel()
        {
        }

        [Display(Name = "Mã điểm in")]
        public string MaDiemIn { get; set; }
        [Display(Name = "Tên điểm in")]
        public string TenDiemIn { get; set; }
    } 
}
