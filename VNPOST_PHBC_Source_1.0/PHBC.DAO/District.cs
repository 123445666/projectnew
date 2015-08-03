//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PHBC.DAO
{
    using System;
    using System.Collections.Generic;
    
    public partial class District
    {
        public District()
        {
            this.Communes = new HashSet<Commune>();
            this.DMDiemIns = new HashSet<DMDiemIn>();
            this.BDieuChinhPhanHuongDistricts = new HashSet<BDieuChinhPhanHuongDistrict>();
            this.BPhanHuongNhuCauDistricts = new HashSet<BPhanHuongNhuCauDistrict>();
        }
    
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string Description { get; set; }
        public string ProvinceCode { get; set; }
    
        public virtual ICollection<Commune> Communes { get; set; }
        public virtual Province Province { get; set; }
        public virtual ICollection<DMDiemIn> DMDiemIns { get; set; }
        public virtual ICollection<BDieuChinhPhanHuongDistrict> BDieuChinhPhanHuongDistricts { get; set; }
        public virtual ICollection<BPhanHuongNhuCauDistrict> BPhanHuongNhuCauDistricts { get; set; }
    }
}
