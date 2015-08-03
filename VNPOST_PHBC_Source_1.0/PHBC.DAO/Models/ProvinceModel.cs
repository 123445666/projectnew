using PHBC.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

public partial class ProvinceModel
{
    public ProvinceModel()
    {
        this.Districts = new HashSet<District>();
        this.DMDiemIns = new HashSet<DMDiemIn>();
    }

    public ProvinceModel(Province Province)
    {
        this.ProvinceCode = Province.ProvinceCode;
        this.ProvinceName = Province.ProvinceName;
        this.Description = Province.Description;
        this.RegionCode = Province.RegionCode;
        this.ProvinceListCode = Province.ProvinceListCode;
        this.Districts = Province.Districts;
        this.Region = Province.Region;
        this.DMDiemIns = Province.DMDiemIns;
    }

    public Province toProvince()
    {
        Province result = new Province();
        result.ProvinceCode = this.ProvinceCode;
        result.ProvinceName = this.ProvinceName;
        result.Description = this.Description;
        result.RegionCode = this.RegionCode;
        result.ProvinceListCode = this.ProvinceListCode;
        result.Districts = this.Districts;
        result.Region = this.Region;
        result.DMDiemIns = this.DMDiemIns;
        return result;
    }

    [Required]
    [Display(Name = "Mã Tỉnh Thành")]
    public string ProvinceCode { get; set; }
    [Required]
    [Display(Name = "Tên Tỉnh Thành")]
    public string ProvinceName { get; set; }
    public string Description { get; set; }
    public string RegionCode { get; set; }
    public string ProvinceListCode { get; set; }

    public virtual ICollection<District> Districts { get; set; }
    public virtual Region Region { get; set; }
    public virtual ICollection<DMDiemIn> DMDiemIns { get; set; }
}
