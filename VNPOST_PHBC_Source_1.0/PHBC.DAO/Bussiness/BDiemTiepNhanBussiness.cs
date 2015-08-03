using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    /// <summary>
    /// created : 28/07/2015
    /// Author : vietvb
    /// </summary>
    public class BDiemTiepNhanBussiness : IBDiemTiepNhanBussiness
    {
        private DB_PHBCEntities db;
        public BDiemTiepNhanBussiness()
        {
            db = new DB_PHBCEntities();
        }
        public BDiemTiepNhanBussiness(DB_PHBCEntities _db)
        {
            db = _db;
        }
        public List<BDiemTiepNhanModel> getAllModel()
        {
            return db.BDiemTiepNhans.Where(x => x.Status == (int)Enums.RecordStatusCode.active).Include(b => b.Unit).Select(t => new BDiemTiepNhanModel()
            { 
                Id = t.Id,
                Code = t.Code,
                Name = t.Name,
                UnitCode = t.UnitCode,
                Unit = t.Unit,
                CreateBy = t.CreateBy,
                CreateDate = t.CreateDate,
                ModifyBy = t.ModifyBy,
                ModifyDate = t.ModifyDate,
                BPhanHuongNhuCaus = t.BPhanHuongNhuCaus,
                BDieuChinhPhanHuongDistricts = t.BDieuChinhPhanHuongDistricts,
                BDieuChinhPhanHuongUnits = t.BDieuChinhPhanHuongUnits
            }).ToList();
        }
        public BDiemTiepNhanModel getModelById(string id)
        {
            BDiemTiepNhanModel bDiemTiepNhanModel = new BDiemTiepNhanModel(db.BDiemTiepNhans.Include(t => t.Unit).FirstOrDefault(a => a.Id.Equals(id) && a.Status == (int)Enums.RecordStatusCode.active));
            return bDiemTiepNhanModel;
        }
        public int Add(BDiemTiepNhanModel bDiemTiepNhanModel)
        {
            BDiemTiepNhanModel objCheck = new BDiemTiepNhanModel();
            objCheck = this.getModelByCode(bDiemTiepNhanModel.Code.Trim());
            if (string.IsNullOrWhiteSpace(objCheck.UnitName))
            {
                bDiemTiepNhanModel.Status = 1;
                db.BDiemTiepNhans.Add(bDiemTiepNhanModel.toBDiemTiepNhan());
                return db.SaveChanges();
            }
            else return -1;
        }
        public int Update(BDiemTiepNhanModel bDiemTiepNhanModel)
        {
            BDiemTiepNhanModel objCheck = new BDiemTiepNhanModel();
            objCheck = this.getModelByCode(bDiemTiepNhanModel.Code.Trim(), bDiemTiepNhanModel.Id);
            if (objCheck != null)
            {
                db.Entry(bDiemTiepNhanModel.toBDiemTiepNhan()).State = EntityState.Modified;
                return db.SaveChanges();
            }
            else return -1;
        }
        public int Delete(string id)
        {
            BDiemTiepNhan bDiemTiepNhan = this.getById(id);
            bDiemTiepNhan.Status = (int)Enums.RecordStatusCode.delete;
            db.Entry(bDiemTiepNhan).State = EntityState.Modified;
            return db.SaveChanges();
        }
        public BDiemTiepNhanModel getModelByCode(string code, string id = null)
        {
            if(string.IsNullOrWhiteSpace(id))
                return new BDiemTiepNhanModel(db.BDiemTiepNhans.FirstOrDefault(t => t.Code.Equals(code) && t.Status == (int)Enums.RecordStatusCode.active));
            else
                return new BDiemTiepNhanModel(db.BDiemTiepNhans.FirstOrDefault(t => t.Code.Equals(code) && !t.Id.Equals(id) && t.Status == (int)Enums.RecordStatusCode.active));
        }
        public BDiemTiepNhan getById(string id)
        {
            return db.BDiemTiepNhans.Include(t => t.Unit).FirstOrDefault(a => a.Id.Equals(id) && a.Status == (int)Enums.RecordStatusCode.active);
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
