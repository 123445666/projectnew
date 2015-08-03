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

    public class UnitModel2
    {
        /// <summary>
        /// created : 28/07/2015
        /// Author : vietvb
        /// </summary>
        public UnitModel2()
        {
            this.POS = new HashSet<POS>();
            this.Unit1 = new HashSet<Unit>();
            this.UserInfoes = new HashSet<UserInfo>();
            this.BDiemTiepNhans = new HashSet<BDiemTiepNhan>();
            this.BPhanHuongNhuCaus = new HashSet<BPhanHuongNhuCau>();
            this.BThongTinBaos = new HashSet<BThongTinBao>();
            this.BDieuChinhPhanHuongUnits = new HashSet<BDieuChinhPhanHuongUnit>();
        }

        public UnitModel2(Unit unit)
        {
            this.UnitCode = unit.UnitCode;
            this.UnitName = unit.UnitName;
            this.ParentUnitCode = unit.ParentUnitCode;
            this.CommuneCode = unit.CommuneCode;
            this.UnitTypeCode = unit.UnitTypeCode;
            this.Commune = unit.Commune;
            this.POS = unit.POS;
            this.Unit1 = unit.Unit1;
            this.Unit2 = unit.Unit2;
            this.UnitType = unit.UnitType;
            this.UserInfoes = unit.UserInfoes;
            this.BDiemTiepNhans = unit.BDiemTiepNhans;
            this.BPhanHuongNhuCaus = unit.BPhanHuongNhuCaus;
            this.BThongTinBaos = unit.BThongTinBaos;
            this.BDieuChinhPhanHuongUnits = unit.BDieuChinhPhanHuongUnits;
        }

        public Unit toUnit()
        {
            Unit obj = new Unit();
            obj.UnitCode = this.UnitCode;
            obj.UnitName = this.UnitName;
            obj.ParentUnitCode = this.ParentUnitCode;
            obj.CommuneCode = this.CommuneCode;
            obj.UnitTypeCode = this.UnitTypeCode;
            obj.Commune = this.Commune;
            obj.POS = this.POS;
            obj.Unit1 = this.Unit1;
            obj.Unit2 = this.Unit2;
            obj.UnitType = this.UnitType;
            obj.UserInfoes = this.UserInfoes;
            obj.BDiemTiepNhans = this.BDiemTiepNhans;
            obj.BPhanHuongNhuCaus = this.BPhanHuongNhuCaus;
            obj.BThongTinBaos = this.BThongTinBaos;
            obj.BDieuChinhPhanHuongUnits = this.BDieuChinhPhanHuongUnits;
            return obj;
        }

        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string ParentUnitCode { get; set; }
        public string CommuneCode { get; set; }
        public string UnitTypeCode { get; set; }

        public virtual Commune Commune { get; set; }
        public virtual ICollection<POS> POS { get; set; }
        public virtual ICollection<Unit> Unit1 { get; set; }
        public virtual Unit Unit2 { get; set; }
        public virtual UnitType UnitType { get; set; }
        public virtual ICollection<UserInfo> UserInfoes { get; set; }
        public virtual ICollection<BDiemTiepNhan> BDiemTiepNhans { get; set; }
        public virtual ICollection<BPhanHuongNhuCau> BPhanHuongNhuCaus { get; set; }
        public virtual ICollection<BThongTinBao> BThongTinBaos { get; set; }
        public virtual ICollection<BDieuChinhPhanHuongUnit> BDieuChinhPhanHuongUnits { get; set; }
    }

    public class UnitModel2Search
    {
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
    }
}