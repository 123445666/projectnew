namespace PHBC.DAO
{
    using System;
    using System.Collections.Generic;

    public partial class v_Unit
    {
        public v_Unit()
        { }
        public 
            
            v_Unit(v_Unit vUnit, string diemTiepNhanId, string diemTiepNhanName)
        {
            UnitCode = vUnit.UnitCode;
            UnitName = vUnit.UnitName;
            UnitTypeCode = vUnit.UnitTypeCode;
            DistrictCode = vUnit.DistrictCode;
            ProvinceCode = vUnit.ProvinceCode;
            DistrictName = vUnit.DistrictName;

            DiemTiepNhanId = diemTiepNhanId;
            DiemTiepNhanName = diemTiepNhanName;
        }

        public string DiemTiepNhanId { get; set; }
        public string DiemTiepNhanCode { get; set; }
        public string DiemTiepNhanName { get; set; }
        public IEnumerable<BDiemTiepNhan> BDiemTiepNhan { get; set; }
    }
}
