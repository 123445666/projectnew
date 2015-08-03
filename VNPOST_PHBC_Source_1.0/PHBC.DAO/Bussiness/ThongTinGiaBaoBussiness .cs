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

    public class ThongTinGiaBaoBussiness : IThongTinGiaBaoBussiness
    {
        private DB_PHBCEntities db;
        public ThongTinGiaBaoBussiness()
        {
            this.db = new DB_PHBCEntities();
        }
        public ThongTinGiaBaoBussiness(DB_PHBCEntities _db)
        {
            this.db = _db;
        }
        public List<BThongTinGiaBao> getAllBThongTinGiaBao()
        {
            return db.BThongTinGiaBaos.ToList();
            //throw new NotImplementedException();
        }
        private IQueryable<ThongTinGiaBaoModel> BuildQuery(ThongTinGiaBaoSearchModel search, string idBao, int status = 1, List<string> lstInclude = null)
        {
            string include = string.Empty;
            IQueryable<BThongTinGiaBao> qThongTinGiaBao = db.BThongTinGiaBaos;
            if (lstInclude != null && lstInclude.Count > 0)
                foreach (string item in lstInclude)
                    qThongTinGiaBao = qThongTinGiaBao.Include(item);

            IQueryable<ThongTinGiaBaoModel> query = (from u in qThongTinGiaBao//.Include(a=>a.)
                                                      where u.Status == status && u.ThongTinBaoId == idBao
                                                      select new ThongTinGiaBaoModel()
                                                      {
                                                          Id = u.Id,
                                                          ThongTinBaoId = u.ThongTinBaoId,
                                                          NgayHieuLuc = u.NgayHieuLuc,
                                                          NgayHetHieuLuc = u.NgayHetHieuLuc,
                                                          ProvinceCode = u.ProvinceCode,
                                                          QuyetDinh = u.QuyetDinh,
                                                          ValueType = u.ValueType,
                                                          Value = u.Value.ToString(),
                                                          MaBao = u.BThongTinBao != null ? u.BThongTinBao.MaBao : "",
                                                          TenBao = u.BThongTinBao != null ? u.BThongTinBao.TenBao : "", 
                                                          BaoTrungUongDiaPhuong =  u.BThongTinBao.BaoTrungUongDiaPhuong,
                                                          BaoTrongMucLuc = u.BThongTinBao.BaoTrongMucLuc
                                                      }).OrderBy(a => a.Id);
            //Neu co include thi them include vao 
            if (lstInclude != null && lstInclude.Count > 0)
                foreach (string item in lstInclude)
                    qThongTinGiaBao = qThongTinGiaBao.Include(item);
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Search))
                {
                    //string giaFrom = search.Search.Trim().Split(',')[0];
                    //string giaTo = search.Search.Trim().Split(',')[1];
                    //query = query.Where(a => Convert.ToDecimal(a.Value) >= Convert.ToDecimal(giaFrom) && Convert.ToDecimal(a.Value) <= Convert.ToDecimal(giaTo));
                    query = query.Where(a => a.MaBao.Contains(search.Search) || a.TenBao.Contains(search.Search));
                }
                if (!string.IsNullOrWhiteSpace(search.Value))
                    query = query.Where(a=>a.Value.CompareTo(search.Value)==0);                
                if (!string.IsNullOrWhiteSpace(search.Id))
                    query = query.Where(a => a.Id.CompareTo(search.Id) == 0);
                if (!string.IsNullOrWhiteSpace(search.ThongTinBaoId))
                    query = query.Where(a => a.ThongTinBaoId.Contains(search.ThongTinBaoId));
               
            }
            return query;
        }
        public List<ThongTinGiaBaoModel> getAllThongTinGiaBaoModel(string idBao)
        {
            return BuildQuery(null,idBao,1,null).ToList();            
        }
        public List<BThongTinGiaBao> getAll(int page, int pageSize, out int pageCount)
        {
            return Utils.buildPage(db.BThongTinGiaBaos, page, ref pageSize, out pageCount);            
        }
        public List<ThongTinGiaBaoModel> getAllModel(int page, int pageSize, out int pageCount, out int totalitem, string idBao)
        {
            var query = BuildQuery(null, idBao, 1, null);
            List<ThongTinGiaBaoModel> lst = Utils.buildPage(query, page, ref pageSize, out totalitem, out pageCount);
            foreach (var item in lst)
            {
                buildThongTinGiaBao(item);
            }
            return lst;            
        }
        private ErrorObject checkThongTinGiaBao(string Id, string ngayHieuLuc, string ngayHetHieuLuc)
        {
            ErrorObject err = new ErrorObject();
            if( Convert.ToDateTime(ngayHieuLuc) < DateTime.Now)
            {
                err.HasError = true;
                err.LstError.Add("_NgayHieuLuc", "Ngày hiệu lực phải lớn hơn ngày hiện tại");
            }
            if (!string.IsNullOrWhiteSpace(ngayHetHieuLuc) && Convert.ToDateTime(ngayHetHieuLuc) < Convert.ToDateTime(ngayHieuLuc))
            {
                err.HasError = true;
                err.LstError.Add("_NgayHetHieuLuc", "Ngày hết hiệu lực phải lớn hơn ngày hiệu lực");
            }
            if (string.IsNullOrEmpty(Id))
            {
                if (db.BThongTinGiaBaos.Any(r => r.ThongTinBaoId.CompareTo(Id.Trim()) == 0))
                {
                    err.HasError = true;
                    err.LstError.Add("_NgayHetHieuLuc", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã Giá Báo"));
                }
                
            }
            else
            {
                if (db.BThongTinGiaBaos.Any(r => r.ThongTinBaoId.CompareTo(Id.Trim()) == 0))
                {
                    err.HasError = true;
                    err.LstError.Add("", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã Giá Báo"));
                }
                
            }
            return err;
        }
        public ErrorObject Create(ThongTinGiaBaoModel bthongtingiabaoModel)
        {
            ErrorObject err = new ErrorObject();
            err = checkThongTinGiaBao(bthongtingiabaoModel.Id, bthongtingiabaoModel._NgayHieuLuc, bthongtingiabaoModel._NgayHetHieuLuc);
            if (err.HasError)
            {
                return err;
            }
            else
            {
                db.BThongTinGiaBaos.Add(bthongtingiabaoModel.toCreate());
                int i = db.SaveChanges();

                if (i < 1)
                {
                    err.HasError = false;
                }
            }
            return err;            
        }

        /// <summary>
        /// Lấy danh sách giá báo theo id báo và mã tỉnh
        /// </summary>
        /// <param name="id"> id báo</param>
        /// <param name="provinceCode">Mã tỉnh(Nếu là trung ương thì không có mã tỉnh)</param>
        /// <returns>Thông tin báo và danh sách giá mua, giá bán</returns>
        /// <modify>
        /// Author  Date    comment
        /// Long            Tạo mới
        /// </modify>
        public DanhSachGiaBaoModel getDanhSachGiaBaoModel(string id, string provinceCode)
        {
            int active = (int)Enums.RecordStatusCode.active;
            BThongTinBao objBao = db.BThongTinBaos.Find(id);
            if (objBao == null || objBao.Status != active)
                return null;
            DanhSachGiaBaoModel result = new DanhSachGiaBaoModel() { 
                ThongTinBaoId = objBao.Id,
                TenBao = objBao.TenBao,
                MaBao = objBao.MaBao,
                BaoTrongMucLuc = objBao.BaoTrongMucLuc,
                BaoTrungUongDiaPhuong = objBao.BaoTrungUongDiaPhuong
            };
            short typeGiaBan = (short)Enums.LoaiGia.GiaBan;
            short typeGiaMua = (short)Enums.LoaiGia.GiaMua;
            if(string.IsNullOrWhiteSpace(provinceCode))
            {
                result.LstGiaBan = (from gb in db.BThongTinGiaBaos
                                    where gb.Status == active && gb.ThongTinBaoId == id
                                    && gb.ValueType == typeGiaBan
                                    && (gb.ProvinceCode == null || gb.ProvinceCode == "")
                                    select new ThongTinGiaBaoModel()
                                    {
                                        ThongTinBaoId = gb.ThongTinBaoId,
                                        Value = gb.Value.ToString(),
                                        ValueType = gb.ValueType,
                                        NgayHieuLuc = gb.NgayHieuLuc,
                                        NgayHetHieuLuc = gb.NgayHetHieuLuc,
                                        QuyetDinh = gb.QuyetDinh
                                    }).OrderByDescending(a => a.NgayHieuLuc).ToList();
                result.LstGiaMua = (from gb in db.BThongTinGiaBaos
                                    where gb.Status == active && gb.ThongTinBaoId == id
                                    && gb.ValueType == typeGiaMua
                                    && (gb.ProvinceCode == null || gb.ProvinceCode == "")
                                    select new ThongTinGiaBaoModel()
                                    {
                                        ThongTinBaoId = gb.ThongTinBaoId,
                                        Value = gb.Value.ToString(),
                                        ValueType = gb.ValueType,
                                        NgayHieuLuc = gb.NgayHieuLuc,
                                        NgayHetHieuLuc = gb.NgayHetHieuLuc,
                                        QuyetDinh = gb.QuyetDinh
                                    }).OrderByDescending(a => a.NgayHieuLuc).ToList();
            }
            else
            {

                result.LstGiaBan = (from gb in db.BThongTinGiaBaos
                                    where gb.Status == active && gb.ThongTinBaoId == id
                                    && gb.ValueType == typeGiaBan
                                    && gb.ProvinceCode == provinceCode
                                    select new ThongTinGiaBaoModel()
                                    {
                                        ThongTinBaoId = gb.ThongTinBaoId,
                                        Value = gb.Value.ToString(),
                                        ValueType = gb.ValueType,
                                        NgayHieuLuc = gb.NgayHieuLuc,
                                        NgayHetHieuLuc = gb.NgayHetHieuLuc,
                                        QuyetDinh = gb.QuyetDinh
                                    }).OrderByDescending(a => a.NgayHieuLuc).ToList();
                result.LstGiaMua = (from gb in db.BThongTinGiaBaos
                                    where gb.Status == active && gb.ThongTinBaoId == id
                                    && gb.ValueType == typeGiaBan
                                    && (gb.ProvinceCode == null || gb.ProvinceCode == "")
                                    select new ThongTinGiaBaoModel()
                                    {
                                        ThongTinBaoId = gb.ThongTinBaoId,
                                        Value = gb.Value.ToString(),
                                        ValueType = typeGiaMua,
                                        NgayHieuLuc = gb.NgayHieuLuc,
                                        NgayHetHieuLuc = gb.NgayHetHieuLuc,
                                        QuyetDinh = gb.QuyetDinh
                                    }).OrderByDescending(a => a.NgayHieuLuc).ToList();
            }

            return result;
        }
        public List<ThongTinGiaBaoModel> searchThongTinGiaBao(ThongTinGiaBaoSearchModel thongTinGiaBaoSearchModel)
        {           
            int active = (int)Enums.RecordStatusCode.active;
            return BuildQuery(thongTinGiaBaoSearchModel, null, active, null).ToList();
        }        
        public ErrorObject GiaMua(ThongTinGiaBaoModel bthongtingiabaoModel)
        {
            ErrorObject err = new ErrorObject();
            err = checkThongTinGiaBao(bthongtingiabaoModel.Id, bthongtingiabaoModel._NgayHieuLuc, bthongtingiabaoModel._NgayHetHieuLuc);
            if (err.HasError)
            {
                return err;
            }
            else
            {
                db.BThongTinGiaBaos.Add(bthongtingiabaoModel.toCreate());
                int i = db.SaveChanges();
                if (i < 1)
                {
                    err.HasError = false;
                }
            }
            return err;            
        }
        public ErrorObject Delete(string id)
        {
            ErrorObject err = new ErrorObject();
            BThongTinGiaBao bThongTinGiaBao = db.BThongTinGiaBaos.Find(id);
            bThongTinGiaBao.Status = (int)Enums.RecordStatusCode.delete;
            db.Entry(bThongTinGiaBao).State = EntityState.Modified;
            int i = db.SaveChanges();
            if (i < 1)
            {
                err.HasError = false;
            }
            return err;
        }
        public ErrorObject Edit(ThongTinGiaBaoModel thongTinGiaBaoModel)
        {
            ErrorObject err = new ErrorObject();
            err = checkThongTinGiaBao(thongTinGiaBaoModel.Id, thongTinGiaBaoModel._NgayHieuLuc, thongTinGiaBaoModel._NgayHetHieuLuc);
            if (err.HasError)
            {
                return err;
            }
            else
            {
                BThongTinGiaBao bThongTinGiaBao = db.BThongTinGiaBaos.Find(thongTinGiaBaoModel.Id);
                thongTinGiaBaoModel.changeEdit(bThongTinGiaBao);
                db.Entry(bThongTinGiaBao).State = EntityState.Modified;
                int i = db.SaveChanges();
                if (i < 1)
                {
                    err.HasError = false;
                }
            }
            return err;
        }
        private void buildThongTinGiaBao(ThongTinGiaBaoModel item)
        {
            if (item != null)
            {
                if (item.ValueType == 0) { item.ValueTypeDesc = Enums.LoaiGiaDesc.GiaBan; }
                else { item.ValueTypeDesc = Enums.LoaiGiaDesc.GiaMua; }
            }
        }
        private void mapThongTinGiaBaoModel(ThongTinGiaBaoModel thongTinGiaBaoModel, BThongTinGiaBao bThongTinGiaBao)
        {
            thongTinGiaBaoModel.Id = bThongTinGiaBao.Id;
            thongTinGiaBaoModel.ValueType = bThongTinGiaBao.ValueType;
            thongTinGiaBaoModel.Value = bThongTinGiaBao.Value.ToString();
            thongTinGiaBaoModel.NgayHieuLuc = bThongTinGiaBao.NgayHieuLuc;
            thongTinGiaBaoModel.NgayHetHieuLuc = bThongTinGiaBao.NgayHetHieuLuc;
            thongTinGiaBaoModel.QuyetDinh = bThongTinGiaBao.QuyetDinh;
        }
        public ThongTinBaoModel getThongTinBaoById(string Id)
        {
            ThongTinBaoSearchModel search = new ThongTinBaoSearchModel() { Id = Id };
            int active = (int)Enums.RecordStatusCode.active;
            ThongTinBaoModel item = BuildThongTinBaoQuery(search, active, null).FirstOrDefault();
            buildThongTinBao(item);
            return item;
        }
        private void buildThongTinBao(ThongTinBaoModel item)
        {
            if (item != null)
            {
                item.BaoNgoaiVanDesc = Enums.BaoNgoaiVanDesc(item.BaoNgoaiVan ? Enums.BaoNgoaiVan.BaoNhapKhau : Enums.BaoNgoaiVan.BaoNhapKhau);
                item.BaoCongIchNgoaiCongIchDesc = Enums.BaoCongIchNgoaiCongIchDesc(item.BaoCongIchNgoaiCongIch ? Enums.BaoCongIchNgoaiCongIch.BaoCongIch : Enums.BaoCongIchNgoaiCongIch.BaoNgoaiCongIch);
                item.BaoTrongMucLucDesc = Enums.BaoTrongDanhMucDesc(item.BaoTrongMucLuc ? Enums.BaoTrongDanhMuc.BaoNgoaiDanhMuc : Enums.BaoTrongDanhMuc.BaoTrongDanhMuc);
                item.BaoTrungUongDiaPhuongDesc = Enums.BaoTrungUongDiaPhuongDesc(item.BaoTrungUongDiaPhuong ? Enums.BaoTrungUongDiaPhuong.BaoDiaPhuong : Enums.BaoTrungUongDiaPhuong.BaoTrungUong);
                item.CoThueDesc = Enums.BaoCoThueKhongThueDesc(item.CoThue ? Enums.BaoCoThueKhongThue.BaoCoThue : Enums.BaoCoThueKhongThue.BaoKhongThue);
                //item.ParentName = getParentNameByParentId(item.ParentId);
            }
        }
        private IQueryable<ThongTinBaoModel> BuildThongTinBaoQuery(ThongTinBaoSearchModel search, int status = 1, List<string> lstInclude = null)
        {
            string include = string.Empty;
            IQueryable<BThongTinBao> qThongTinBao = db.BThongTinBaos;
            //Neu co include thi them include vao 
            if (lstInclude != null && lstInclude.Count > 0)
                foreach (string item in lstInclude)
                    qThongTinBao = qThongTinBao.Include(item);

            IQueryable<ThongTinBaoModel> query = (from u in qThongTinBao.Include(a => a.DMLoaiAnPham).Include(a => a.DMToaSoan)//.Include(a=>a.)
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
        public void Dispose()
        {
            db.Dispose();
        }
        public ThongTinGiaBaoModel createThongTinGiaBaoModel(string idBao)
        {
            int active = (int)Enums.RecordStatusCode.active;
            BThongTinBao objBao = db.BThongTinBaos.Find(idBao);
            if (objBao == null || objBao.Status != active)
                return null;
            return new ThongTinGiaBaoModel()
            {
                ThongTinBaoId = objBao.Id,
                TenBao = objBao.TenBao,
                MaBao = objBao.MaBao,
                BaoTrongMucLuc = objBao.BaoTrongMucLuc,
                BaoTrungUongDiaPhuong = objBao.BaoTrungUongDiaPhuong
            };
            
        }
        public ThongTinGiaBaoModel getThongTinGiaBaoById(string id)
        {
            int active = (int)Enums.RecordStatusCode.active;
            ThongTinGiaBaoModel result = (from gb in db.BThongTinGiaBaos
                                             where gb.Status == active && gb.Id == id
                                             select new ThongTinGiaBaoModel()
                                             {
                                                 ThongTinBaoId = gb.ThongTinBaoId,
                                                 Value = gb.Value.ToString(),
                                                 ValueType = gb.ValueType,
                                                 NgayHieuLuc = gb.NgayHieuLuc,
                                                 NgayHetHieuLuc = gb.NgayHetHieuLuc,
                                                 QuyetDinh = gb.QuyetDinh
                                             }).OrderByDescending(a => a.NgayHieuLuc).FirstOrDefault();
            if (result == null)
                return null;
            BThongTinBao objBao = db.BThongTinBaos.Find(result.ThongTinBaoId);
            if (objBao == null || objBao.Status != active)
                return null;
            result.TenBao = objBao.TenBao;
            result.MaBao = objBao.MaBao;
            result.BaoTrongMucLuc = objBao.BaoTrongMucLuc;
            result.BaoTrungUongDiaPhuong = objBao.BaoTrungUongDiaPhuong;
            return result;
        }
    }
}