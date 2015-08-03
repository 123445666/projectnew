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
    public class ThongTinBaoBussiness : IThongTinBaoBussiness
    {
        private DB_PHBCEntities db;
        public ThongTinBaoBussiness()
        {
            this.db = new DB_PHBCEntities();
        }
        public ThongTinBaoBussiness(DB_PHBCEntities _db)
        {
            this.db = _db;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BThongTinBao> getAllThongTinBao()
        {
            return db.BThongTinBaos.ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ThongTinBaoModel> getAllThongTinBaoModel()
        {
            return BuildQuery(null).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public List<BThongTinBao> getAll(int page, int pageSize, out int pageCount)
        {
            return Utils.buildPage(db.BThongTinBaos, page, ref pageSize, out pageCount);
        }
        public List<ThongTinBaoModel> getAllModel(int page, int pageSize, out int pageCount, out int totalitem)
        {
            var query = BuildQuery(null);
            List<ThongTinBaoModel> lst = Utils.buildPage(query, page, ref pageSize, out totalitem, out pageCount);
            foreach (var item in lst)
            {
                buildThongTinBao(item);
            }
            return lst;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="MaBao"></param>
        /// <param name="TenBao"></param>
        /// <returns></returns>
        private ErrorObject checkThongTinToBao(string Id, string maBao, string tenBao)
        {
            ErrorObject err = new ErrorObject();
            if (string.IsNullOrEmpty(Id))
            {
                if (db.BThongTinBaos.Any(r => r.MaBao.CompareTo(maBao.Trim()) == 0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("MaBao", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã báo"));
                }
                if (db.BThongTinBaos.Any(r => r.TenBao.CompareTo(tenBao.Trim()) == 0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("TenBao", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Tên báo"));
                }
            }
            else
            {
                if (db.BThongTinBaos.Any(r => r.MaBao.CompareTo(maBao.Trim()) == 0 && r.Id.CompareTo(Id.Trim()) !=0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("MaBao", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã báo"));
                }
                if (db.BThongTinBaos.Any(r => r.TenBao.CompareTo(tenBao.Trim()) == 0 && r.Id.CompareTo(Id.Trim()) != 0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("TenBao", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Tên báo"));
                }
            }
            return err;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thongTinBaoModel"></param>
        /// <returns></returns>
        public ErrorObject Create(ThongTinBaoModel thongTinBaoModel)
        {
            ErrorObject err = new ErrorObject();
            err = checkThongTinToBao(thongTinBaoModel.Id, thongTinBaoModel.MaBao, thongTinBaoModel.TenBao);
            if (err.HasError)
            {
                return err;
            }
            else
            {
                ////BThongTinBao bThongTinBao = new BThongTinBao();
                //mapObject(thongTinBaoModel, bThongTinBao);
                db.BThongTinBaos.Add(thongTinBaoModel.toCreate());                
                int i = db.SaveChanges();
                if (i < 1)
                {
                    err.HasError = false;
                }
            }
            return err;
        }
        /// <summary>    
        /// Edit
        /// </summary>
        /// <returns></returns>
        /// <Modify>
        /// Author  Date      Comment
        /// Longth  02/07/2015  Create new
        /// </Modify>
        public ErrorObject Edit(ThongTinBaoModel thongTinBaoModel)
        {
            ErrorObject err = new ErrorObject();
            err = checkThongTinToBao(thongTinBaoModel.Id, thongTinBaoModel.MaBao, thongTinBaoModel.TenBao);
            if (err.HasError)
            {
                return err;
            }
            else
            {
                BThongTinBao bThongTinBao = db.BThongTinBaos.Find(thongTinBaoModel.Id);
                thongTinBaoModel.changeEdit(bThongTinBao);
                db.Entry(bThongTinBao).State = EntityState.Modified;     
                int i = db.SaveChanges();
                if (i < 1)
                {
                    err.HasError = false;
                }
            }
            return err;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="status"></param>
        /// <param name="lstInclude"></param>
        /// <returns></returns>
        private IQueryable<ThongTinBaoModel> BuildQuery(ThongTinBaoSearchModel search, int status = 1, List<string> lstInclude = null)
        {
            string include = string.Empty;
            IQueryable<BThongTinBao> qThongTinBao = db.BThongTinBaos;
            //Neu co include thi them include vao 
            if (lstInclude != null && lstInclude.Count > 0)
                foreach (string item in lstInclude)
                    qThongTinBao = qThongTinBao.Include(item);

            IQueryable<ThongTinBaoModel> query = (from u in qThongTinBao.Include(a => a.DMLoaiAnPham).Include(a=>a.DMToaSoan)//.Include(a=>a.)
                                                  where u.Status == status
                                                  select new ThongTinBaoModel()
                                                   {
                                                       Id = u.Id,
                                                       MaBao = u.MaBao,
                                                       MaToaSoan = u.MaToaSoan,
                                                       TenBao = u.TenBao,
                                                       CoThue = u.CoThue,
                                                       MucThue = u.MucThue.ToString(),
                                                       BaoTrungUongDiaPhuong = u.BaoTrungUongDiaPhuong,
                                                       BaoCongIchNgoaiCongIch = u.BaoCongIch,
                                                       LoaiAnPham = u.LoaiAnPham,
                                                       ParentId = u.ParentId,                                                       
                                                       BaoTrongMucLuc = u.BaoTrongMucLuc,
                                                       BaoNgoaiVan = u.BaoNgoaiVan,
                                                       SoTrang = u.SoTrang.ToString(),
                                                       KichThuoc = u.KichThuoc,
                                                       TrongLuong = u.TrongLuong.ToString(),
                                                       GiayPhep = u.GiayPhep,
                                                       GiaVon = u.GiaVon,
                                                       GiaBia = u.GiaBia.ToString(),
                                                       GiaBan = u.GiaBan,
                                                       Status = u.Status,
                                                       GhiChu = u.GhiChu,
                                                       TenLoaiAnPham = u.DMLoaiAnPham != null ? u.DMLoaiAnPham.TenLoaiAnPham : "",
                                                       TenToaSoan = u.DMToaSoan != null ? u.DMToaSoan.TenToaSoan : ""
                                                   }).OrderBy(a => a.TenBao);
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Search))
                {
                    query = query.Where(a => a.MaBao.Contains(search.Search) || a.TenBao.Contains(search.Search));
                }
                if (!string.IsNullOrWhiteSpace(search.Id))
                    query = query.Where(a => a.Id.CompareTo(search.Id) == 0);
                if (!string.IsNullOrWhiteSpace(search.MaBao))
                    query = query.Where(a => a.MaBao.Contains(search.MaBao));
                if (!string.IsNullOrWhiteSpace(search.MaToaSoan))
                    query = query.Where(a => a.MaBao.Contains(search.MaToaSoan));  
                if (!string.IsNullOrWhiteSpace(search.TenBao))
                    query = query.Where(a => a.TenBao.Contains(search.TenBao));
                if (!string.IsNullOrWhiteSpace(search.GiaBia))
                    query = query.Where(a => a.GiaBia.Contains(search.GiaBia));
                if (!string.IsNullOrWhiteSpace(search.TrongLuong))
                    query = query.Where(a => a.TrongLuong.Contains(search.TrongLuong));
                if (!string.IsNullOrWhiteSpace(search.KichThuoc))
                    query = query.Where(a => a.KichThuoc.Contains(search.KichThuoc));
                if (!string.IsNullOrWhiteSpace(search.GiayPhep))
                    query = query.Where(a => a.KichThuoc.Contains(search.GiayPhep));
            }
            return query;
        }
        /// <summary>    
        /// Delete
        /// </summary>
        /// <returns></returns>
        /// <Modify>
        /// Author  Date      Comment
        /// Longth  02/07/2015  Create new
        /// </Modify>
        public ErrorObject Delete(string id)
        {
            ErrorObject err = new ErrorObject();
            BThongTinBao bThongTinBao = db.BThongTinBaos.Find(id);
            bThongTinBao.Status = (int)Enums.RecordStatusCode.delete;
            db.Entry(bThongTinBao).State = EntityState.Modified;            
            int i = db.SaveChanges();
            if (i < 1)
            {
                err.HasError = false;
            }
            return err;
        }
        public BaoDiemInModel getBaoDiemInModel(string id)
        {
            BThongTinBao thongtinbao = db.BThongTinBaos.Include(a => a.DMDiemIns).Include("DMDiemIns.Province").FirstOrDefault(a => a.Id.CompareTo(id) == 0);
            if (thongtinbao == null)
                return null;
            BaoDiemInModel result = new BaoDiemInModel();
            result.Id = thongtinbao.Id;
            result.MaBao = thongtinbao.MaBao;
            result.TenBao = thongtinbao.TenBao;
            int status = (int)Enums.RecordStatusCode.active;
            //lay danh sach da co quyen
            result.DiemIn_Current = thongtinbao.DMDiemIns.Where(a => a.Status == status).OrderBy(a => a.Id).ToList();
            //Lay danh sach chua co quyen
            List<DMDiemIn> lstDiemInAll = db.DMDiemIns.Include(a => a.Province).Where(a => a.Status == status).ToList();
            result.DiemIn_NoMap = lstDiemInAll.Where(a => !thongtinbao.DMDiemIns.Any(c => c.Id.CompareTo(a.Id) == 0)).OrderBy(a => a.Province.ProvinceName).OrderBy(a => a.TenDiemIn).ToList();
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lstDiemIn"></param>
        /// <returns></returns>
        public int CapNhatBaoDiemIn(string id, string lstDiemIn)
        {
            //1- Có nên return ve error object khong ?
            //2- Với việc ExecuteSqlCommand nen dung try - catch bat exception va return err ???

            int result = 0;
            try
            {
                string sqlDelete = "Delete BBaoDiemIn Where ThongTinBaoId = {0}";
                db.Database.ExecuteSqlCommand(sqlDelete, id);
                if (!string.IsNullOrEmpty(lstDiemIn))
                {
                    string[] _lstDiemIn = lstDiemIn.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    string sqlInsert = "Insert into BBaoDiemIn (ThongTinBaoId, DiemInId) VALUES ({0}, {1});";
                    foreach (string item in _lstDiemIn)
                        result += db.Database.ExecuteSqlCommand(sqlInsert, id, item);
                }
            }
            catch(Exception ex)
            {
                ErrorObject err = new ErrorObject();
                err.HasError = true;
                err.LstError.Add("Cập nhật điểm in",ex.Message);
            }
            finally
            {
                Dispose();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ThongTinBaoModel getThongTinBaoById(string Id)
        {
            ThongTinBaoSearchModel search = new ThongTinBaoSearchModel() { Id = Id };
            int active = (int)Enums.RecordStatusCode.active;
            ThongTinBaoModel item = BuildQuery(search, active, null).FirstOrDefault();
            buildThongTinBao(item);                                   
            return item;
        }        
        
        private void buildThongTinGiaBao(ThongTinGiaBaoModel item)
        {
            if (item != null)
            {
                if (item.ValueType == 0) { item.ValueTypeDesc = Enums.LoaiGiaDesc.GiaBan; }
                else { item.ValueTypeDesc = Enums.LoaiGiaDesc.GiaMua; }
            }
        }    
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DefineSelectItem> getListLoaiAnPham()
        {
            List<DefineSelectItem> result = new List<DefineSelectItem>();
            List<DMLoaiAnPham> lstLoaiAnPham;
            lstLoaiAnPham = db.DMLoaiAnPhams.Where(t => t.Status == 1).ToList();
            foreach (DMLoaiAnPham anPham in lstLoaiAnPham)
            {
                result.Add(new DefineSelectItem() { Value = anPham.Id, Text = anPham.TenLoaiAnPham });
            }
            return result;
        }
        public List<DefineSelectItem> getListBaoNgoaiVan(int currentValue)
        {
            List<DefineSelectItem> result = new List<DefineSelectItem>();

            int sValue = (int)Enums.BaoNgoaiVan.BaoTrongNuoc;
            if (sValue >= currentValue)
                result.Add(new DefineSelectItem() { Value = sValue.ToString(), Text = Enums.BaoNgoaiVanDesc(Enums.BaoNgoaiVan.BaoTrongNuoc) });
            sValue = (int)Enums.BaoNgoaiVan.BaoNhapKhau;
            if (sValue >= currentValue)
                result.Add(new DefineSelectItem() { Value = sValue.ToString(), Text = Enums.BaoNgoaiVanDesc(Enums.BaoNgoaiVan.BaoNhapKhau) });

            return result;
        }
        public List<DefineSelectItem> getListTWDP(int currentValue)
        {
            List<DefineSelectItem> result = new List<DefineSelectItem>();

            int sValue = (int)Enums.BaoTrungUongDiaPhuong.BaoTrungUong;
            if (sValue >= currentValue)
                result.Add(new DefineSelectItem() { Value = sValue.ToString(), Text = Enums.BaoTrungUongDiaPhuongDesc(Enums.BaoTrungUongDiaPhuong.BaoTrungUong) });
            sValue = (int)Enums.BaoTrungUongDiaPhuong.BaoDiaPhuong;
            if (sValue >= currentValue)
                result.Add(new DefineSelectItem() { Value = sValue.ToString(), Text = Enums.BaoTrungUongDiaPhuongDesc(Enums.BaoTrungUongDiaPhuong.BaoDiaPhuong) });
            return result;
        }
        public List<DefineSelectItem> getListBaoTrongDanhMuc(int currentValue)
        {
            List<DefineSelectItem> result = new List<DefineSelectItem>();

            int sValue = (int)Enums.BaoTrongDanhMuc.BaoTrongDanhMuc;
            if (sValue >= currentValue)
                result.Add(new DefineSelectItem() { Value = sValue.ToString(), Text = Enums.BaoTrongDanhMucDesc(Enums.BaoTrongDanhMuc.BaoTrongDanhMuc) });
            sValue = (int)Enums.BaoTrongDanhMuc.BaoNgoaiDanhMuc;
            if (sValue >= currentValue)
                result.Add(new DefineSelectItem() { Value = sValue.ToString(), Text = Enums.BaoTrongDanhMucDesc(Enums.BaoTrongDanhMuc.BaoNgoaiDanhMuc) });

            return result;
        }
        public List<DefineSelectItem> getListBaoCoThueKhongThue(int currentValue)
        {
            List<DefineSelectItem> result = new List<DefineSelectItem>();

            int sValue = (int)Enums.BaoCoThueKhongThue.BaoCoThue;
            if (sValue >= currentValue)
                result.Add(new DefineSelectItem() { Value = sValue.ToString(), Text = Enums.BaoCoThueKhongThueDesc(Enums.BaoCoThueKhongThue.BaoCoThue) });
            sValue = (int)Enums.BaoCoThueKhongThue.BaoKhongThue;
            if (sValue >= currentValue)
                result.Add(new DefineSelectItem() { Value = sValue.ToString(), Text = Enums.BaoCoThueKhongThueDesc(Enums.BaoCoThueKhongThue.BaoKhongThue) });

            return result;
        }
        public List<DefineSelectItem> getListBaoCongIchNgoaiCongIch(int currentValue)
        {
            List<DefineSelectItem> result = new List<DefineSelectItem>();

            int sValue = (int)Enums.BaoCongIchNgoaiCongIch.BaoCongIch;
            if (sValue >= currentValue)
                result.Add(new DefineSelectItem() { Value = sValue.ToString(), Text = Enums.BaoCongIchNgoaiCongIchDesc(Enums.BaoCongIchNgoaiCongIch.BaoCongIch) });
            sValue = (int)Enums.BaoCongIchNgoaiCongIch.BaoNgoaiCongIch;
            if (sValue >= currentValue)
                result.Add(new DefineSelectItem() { Value = sValue.ToString(), Text = Enums.BaoCongIchNgoaiCongIchDesc(Enums.BaoCongIchNgoaiCongIch.BaoNgoaiCongIch) });

            return result;
        }
        public List<DefineSelectItem> getListDMToanSoan()
        {
            List<DefineSelectItem> result = new List<DefineSelectItem>();
            List<DMToaSoan> lstToaSoan;
            lstToaSoan = db.DMToaSoans.Where(a=> a.Status ==1).OrderBy(a=> a.MaToaSoan).ToList();
            foreach (DMToaSoan toaSoan in lstToaSoan)
            {
                result.Add(new DefineSelectItem() { Value = toaSoan.Id, Text = toaSoan.TenToaSoan });
            }
            return result;
        }   
        public BaoKyXuatBanModel getBaoKyXuatBanModel(string id)
        {
            BThongTinBao thongtinbao = db.BThongTinBaos.Include(a => a.BKyXuatBans).FirstOrDefault(a => a.Id.CompareTo(id) == 0);
            if (thongtinbao == null)
                return null;
            BaoKyXuatBanModel result = new BaoKyXuatBanModel();
            result.Id = thongtinbao.Id;
            result.MaBao = thongtinbao.MaBao;
            result.TenBao = thongtinbao.TenBao;
            int active = (int)Enums.RecordStatusCode.active;
            BKyXuatBan objKXB = thongtinbao.BKyXuatBans.FirstOrDefault(a => a.status == active);
            if (objKXB != null)
            {
                result.LoaiKy = objKXB.LoaiKy;
                result.KyConfig = objKXB.ChiTiet;
            }
            else
            {
                result.LoaiKy = -1;
                result.KyConfig = "[\"id\":0,\"data\":[]]";
            }
            return result;
        }
        public string getParentNameByParentId(string parentId)
        {
            string parentName = db.BThongTinBaos.Where(a => a.ParentId.Equals(parentId)).Select(a => a.TenBao).FirstOrDefault();            
            return parentName;
        }
        private void buildThongTinBao(ThongTinBaoModel item)
        {
            if (item != null)
            {
                item.BaoNgoaiVanDesc = Enums.BaoNgoaiVanDesc(item.BaoNgoaiVan ? Enums.BaoNgoaiVan.BaoNhapKhau : Enums.BaoNgoaiVan.BaoNhapKhau);
                item.BaoCongIchNgoaiCongIchDesc = Enums.BaoCongIchNgoaiCongIchDesc(item.BaoCongIchNgoaiCongIch ? Enums.BaoCongIchNgoaiCongIch.BaoCongIch : Enums.BaoCongIchNgoaiCongIch.BaoNgoaiCongIch);
                item.BaoTrongMucLucDesc = Enums.BaoTrongDanhMucDesc(item.BaoTrongMucLuc?Enums.BaoTrongDanhMuc.BaoNgoaiDanhMuc:Enums.BaoTrongDanhMuc.BaoTrongDanhMuc);
                item.BaoTrungUongDiaPhuongDesc = Enums.BaoTrungUongDiaPhuongDesc(item.BaoTrungUongDiaPhuong ? Enums.BaoTrungUongDiaPhuong.BaoDiaPhuong : Enums.BaoTrungUongDiaPhuong.BaoTrungUong);
                item.CoThueDesc = Enums.BaoCoThueKhongThueDesc(item.CoThue?Enums.BaoCoThueKhongThue.BaoCoThue:Enums.BaoCoThueKhongThue.BaoKhongThue);
                item.ParentName = getParentNameByParentId(item.ParentId);
            }
        }
        /// <summary>
        /// Cập nhật kỳ xuất bản
        /// </summary>
        /// <returns></returns>
        public int UpdateKyXuatBan(string id, int loaiky, string chitiet, string userID)
        {
            int active = (int)Enums.RecordStatusCode.active;
            BThongTinBao thongtinbao = db.BThongTinBaos.Include(a => a.BKyXuatBans).FirstOrDefault(a => a.Id.CompareTo(id) == 0);
            BKyXuatBan _ban = thongtinbao.BKyXuatBans.FirstOrDefault(a => a.ThongTinBaoId.CompareTo(id) == 0 && a.status == active);
            if (thongtinbao != null && _ban == null)
            {
                _ban = new BKyXuatBan();
                _ban.Id = Guid.NewGuid().ToString().Trim();
                _ban.ThongTinBaoId = id;
                _ban.ChiTiet = chitiet;
                _ban.LoaiKy = loaiky;

                _ban.CreateBy = userID;
                _ban.CreateDate = DateTime.Now;
                _ban.ModifyBy = userID;
                _ban.ModifyDate = DateTime.Now;
                _ban.status = (int)Enums.RecordStatusCode.active;
                db.BKyXuatBans.Add(_ban);
                return db.SaveChanges();

            }
            else if (_ban != null)
            {
                _ban.ChiTiet = chitiet;
                _ban.LoaiKy = loaiky;
                _ban.ModifyBy = userID;
                _ban.ModifyDate = DateTime.Now;
                db.SaveChanges();
                return 1;
            }
            return 0;
        }
        
        public List<ThongTinBaoModel> searchThongTinBao(ThongTinBaoSearchModel thongTinBaoSearchModel)
        { 
            int active = (int)Enums.RecordStatusCode.active;
            return BuildQuery(thongTinBaoSearchModel, active, null).ToList();
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}

