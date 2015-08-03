using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace PHBC.DAO.Models
{
   public partial class DMLoaiAnPhamModel
    {
       public DMLoaiAnPhamModel()
       {

       }
      
         public DMLoaiAnPhamModel(DMLoaiAnPham DMLoaiAnPham)
       {
           this.Id = DMLoaiAnPham.Id;
           this.TenLoaiAnPham = DMLoaiAnPham.TenLoaiAnPham;
           this.CoKyXuatBan = DMLoaiAnPham.CoKyXuatBan;
           this.Status = DMLoaiAnPham.Status;
       }
       
       [Key]
       [StringLength(128)]
       public string Id { get; set; }

       [Required]
       [StringLength(256)]
       [Display(Name = "Tên loại ấn phẩm")]
       public string TenLoaiAnPham { get; set; }

       [Required]
       [Display(Name = "Có kỳ xuất bản")]
       public bool CoKyXuatBan { get; set; }

       [ScaffoldColumn(false)]
       [Display(Name = "Trạng thái")]
       public int Status { get; set; }

       public DMLoaiAnPham toDMLoaiAnPham()
       {
           DMLoaiAnPham result = new DMLoaiAnPham();
           result.Id = this.Id;
           result.TenLoaiAnPham = this.TenLoaiAnPham;
           result.CoKyXuatBan = this.CoKyXuatBan;
           result.Status = this.Status;
           return result;
       }

       public virtual IEnumerable<DMLoaiAnPham> DMLoaiAnPhams { get; set; } 

    }
}
