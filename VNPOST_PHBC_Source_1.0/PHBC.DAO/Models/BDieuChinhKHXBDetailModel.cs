﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PHBC.DAO.Models
{
    using System;
    using System.Collections.Generic;

    public partial class BDieuChinhKHXBDetailModel
    {
        public BDieuChinhKHXBDetailModel()
        {
            this.BDieuChinhPhanHuongDistricts = new HashSet<BDieuChinhPhanHuongDistrict>();
            this.BDieuChinhPhanHuongUnits = new HashSet<BDieuChinhPhanHuongUnit>();
        }

        public string Id { get; set; }
        public string DieuChinhKHXBId { get; set; }
        public int LoaiDieuChinh { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string CreateByName { get; set; }
        public string DisplayCreateBy
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ModifyBy))
                {
                    return CreateByName;
                }
                else return ModifyByName;
            }
        }
        public DateTime DisplayCreateDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ModifyBy))
                {
                    return CreateDate;
                }
                else return ModifyDate.Value;
            }
        }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyByName { get; set; }
        public string SoBao { get; set; }
        public Nullable<int> GiaBao { get; set; }
        public Nullable<int> TrongLuong { get; set; }
        public Nullable<int> SoTrang { get; set; }
        public string KichThuoc { get; set; }
        public string Config { get; set; }
        public string NoiDung { get; set; }
        public string GhiChu { get; set; }
        public string NgayOrThu { get; set; }

        public virtual ICollection<BDieuChinhPhanHuongDistrict> BDieuChinhPhanHuongDistricts { get; set; }
        public virtual ICollection<BDieuChinhPhanHuongUnit> BDieuChinhPhanHuongUnits { get; set; }
        public virtual BDieuChinhKHXB BDieuChinhKHXB { get; set; }
    }
}
