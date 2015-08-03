using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PHBC.DAO.Bussiness
{
    public class KeHoachXuatBanBussiness : IKeHoachXuatBanBussiness
    {
        private DB_PHBCEntities db;
        public KeHoachXuatBanBussiness()
        {
            this.db = new DB_PHBCEntities();
        }
        public KeHoachXuatBanBussiness(DB_PHBCEntities _db)
        {
            this.db = _db;

        }


        public DB_PHBCEntities getDBSelect()
        {
            return db;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public List<dynamic> getKHXBByID(string id)
        {
            var query = (from k in db.BKeHoachXuatBans
                         from b in db.BThongTinBaos
                         from c in db.UserInfoes
                         where k.ThongTinBaoId.Equals(id) && k.ThongTinBaoId.Equals(b.Id) && k.ModifyBy.Equals(c.Id)
                         orderby k.Nam descending
                         select new
                         {
                             k.Id,
                             k.ThongTinBaoId,
                             k.Nam,
                             SoBatDau = k.SoBatDau == -1 ? (from p in db.BKeHoachXuatBanDetails where p.ThongTinBaoId.Equals(k.ThongTinBaoId) && p.Nam == (k.Nam - 1) orderby p.Number descending select p.Number).Count() == 0 ? 1 : (from p in db.BKeHoachXuatBanDetails where p.ThongTinBaoId.Equals(k.ThongTinBaoId) && p.Nam == (k.Nam - 1) orderby p.Number descending select p.Number).FirstOrDefault() : k.SoBatDau,
                             k.ModifyDate,
                             c.DislayName,
                             b.MaBao,
                             b.TenBao,
                             quyNew = (from n in db.BDieuChinhKHXBs where n.ThongTinBaoId.Equals(k.ThongTinBaoId) && n.Nam == k.Nam orderby n.Quy descending select n.Quy).FirstOrDefault(),
                             quyNow = DateTime.Now.Month >= 1 && DateTime.Now.Month <= 3 ? 1 : DateTime.Now.Month > 3 && DateTime.Now.Month <= 6 ? 2 : DateTime.Now.Month > 6 && DateTime.Now.Month <= 9 ? 3 : 4
                             
                         });
            
            return query.ToList<dynamic>();
        }

        public List<dynamic> getAll(int page, int pageSize, out int pageCount, out int totalitem, string search, int year)
        {
            var query = (from k in db.BKeHoachXuatBans
                         from b in db.BThongTinBaos
                         from c in db.UserInfoes
                         where k.ThongTinBaoId.Equals(b.Id) && k.ModifyBy.Equals(c.Id)
                         orderby b.MaBao ascending
                         select new
                         {
                             k.Id,
                             k.ThongTinBaoId,
                             k.Nam,
                             SoBatDau = k.SoBatDau == -1 ? (from p in db.BKeHoachXuatBanDetails where p.ThongTinBaoId.Equals(k.ThongTinBaoId) && p.Nam == (k.Nam - 1) orderby p.Number descending select p.Number).Count() == 0 ? 1 : (from p in db.BKeHoachXuatBanDetails where p.ThongTinBaoId.Equals(k.ThongTinBaoId) && p.Nam == (k.Nam - 1) orderby p.Number descending select p.Number).FirstOrDefault() : k.SoBatDau,
                             k.ModifyDate,
                             c.DislayName,
                             b.MaBao,
                             b.TenBao,
                             quyNew = (from n in db.BDieuChinhKHXBs where n.ThongTinBaoId.Equals(k.ThongTinBaoId) && n.Nam == k.Nam orderby n.Quy descending select n.Quy).FirstOrDefault(),
                             quyNow = DateTime.Now.Month >= 1 && DateTime.Now.Month <= 3 ? 1 : DateTime.Now.Month > 3 && DateTime.Now.Month <= 6 ? 2 : DateTime.Now.Month > 6 && DateTime.Now.Month <= 9 ? 3 : 4
                         });
            if (year != -1)
                query = (from p in query where p.Nam == year select p);
            if (!String.IsNullOrEmpty(search))
                query = (from p in query where DB_PHBCEntities.sosanhstring(search, p.MaBao) || DB_PHBCEntities.sosanhstring(search, p.TenBao) select p);
            totalitem = query.Count();
            if (totalitem == 0)
            {
                pageCount = 0;
                return new List<dynamic>();
            }
            pageCount = totalitem / pageSize;
            if (totalitem % pageSize > 0) pageCount++;
            if (page >= pageCount)
                page = pageCount - 1;
            else
                page = page - 1;

            return query.Skip(page * pageSize).Take(pageSize).ToList<dynamic>();
        }
        /// <summary>
        /// Them dieu chinh quy, copy cùng kỳ năm trước khi copy =1
        /// </summary>
        /// <param name="id">Id của báo</param>
        /// <param name="year">Nam điều chỉnh</param>
        /// <param name="quy">Quý điều chỉnh</param>
        /// <param name="userID">Id người điều chỉnh</param>
        /// <param name="copy">1-copy cùng kỳ năm trc, 0-Nấy theo kế hoạch XB năm</param>
        /// <returns>1- Nếu thêm điều chỉnh thành công, 0- Nếu thêm thất bại</returns>
        /// <modify>
        /// Author  Date    comment
        /// Thang   ?       Tạo mới
        /// Anhhn   3/8/15  Thêm copy cùng kỳ năm trước
        /// </modify>
        public int addDieuChinhKHXB(string id, int year, int quy, string userID, string copy)
        {
            BKeHoachXuatBan objKeHoach = db.BKeHoachXuatBans.Where(a => a.ThongTinBaoId == id && a.Nam == year).FirstOrDefault();
            BDieuChinhKHXB _dieuchinh = new BDieuChinhKHXB();
            _dieuchinh.CreateBy = userID;
            _dieuchinh.CreateDate = DateTime.Now;
            _dieuchinh.Id = Guid.NewGuid().ToString().Trim();
            _dieuchinh.ModifyBy = userID;
            _dieuchinh.ModifyDate = _dieuchinh.CreateDate;
            _dieuchinh.Nam = year;
            _dieuchinh.Quy = quy;
            _dieuchinh.ThongTinBaoId = id;
            _dieuchinh.LoaiKy = objKeHoach.LoaiKy;
            _dieuchinh.ChiTietKy = objKeHoach.ChiTietKy;
            db.BDieuChinhKHXBs.Add(_dieuchinh);
            
            List<BKeHoachXuatBanDetail> _chinhSua = db.BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(id) && d.Nam == year && d.Quy == quy).ToList();
            foreach (var item in _chinhSua)
            {
                item.status = (int)Enums.KeHoachXuatBan.approved;
            }
            db.SaveChanges();
            //Anhhn->
            //Nếu copy = 1 thì copy những điều chỉnh của năm trước
            if (copy.Equals("1"))
            {
                CopyDCCungKyNamTruoc(_dieuchinh, userID);
            }
            return 1;
        }
        /// <summary>
        /// Copy điều chỉnh cùng kỳ năm trước
        /// </summary>
        /// <param name="dcId">Id điều chỉnh của quý đang điều chỉnh</param>
        /// <param name="baoId">Id của báo đang điều chỉnh</param>
        /// <param name="year">Nam điều chỉnh</param>
        /// <param name="quy">Tháng điều chỉnh</param>
        /// <param name="userId">Người điều chỉnh</param>
        /// <modify>
        /// Author  Date    Commnet
        /// Anhhn   3/8/15  Tạo mới    
        /// </modify>
        private void CopyDCCungKyNamTruoc(BDieuChinhKHXB _dieuchinh, string userId)
        {
            //Lấy điều chỉnh cùng ký năm trước
            BDieuChinhKHXB dcOld = db.BDieuChinhKHXBs.Include(dc => dc.BDieuChinhKHXBDetails).Where(dc => dc.ThongTinBaoId == _dieuchinh.ThongTinBaoId 
                && dc.Nam == (_dieuchinh.Nam - 1) && dc.Quy == _dieuchinh.Quy).FirstOrDefault();
            if (dcOld == null || dcOld.BDieuChinhKHXBDetails == null || dcOld.BDieuChinhKHXBDetails.Count == 0)
                return;
            foreach (BDieuChinhKHXBDetail dcDeatil in dcOld.BDieuChinhKHXBDetails)
            {
                BDieuChinhKHXBDetail newDCDetail = new BDieuChinhKHXBDetail()
                {
                    Id = Guid.NewGuid().ToString(),
                    DieuChinhKHXBId = _dieuchinh.Id,
                    LoaiDieuChinh = dcDeatil.LoaiDieuChinh,
                    GiaBao = dcDeatil.GiaBao,
                    KichThuoc = dcDeatil.KichThuoc,
                    TrongLuong = dcDeatil.TrongLuong,
                    SoTrang = dcDeatil.SoTrang,
                    GhiChu = dcDeatil.GhiChu,
                    Config = dcDeatil.Config,
                    NoiDung = dcDeatil.NoiDung,
                    NgayOrThu = dcDeatil.NgayOrThu,
                    SoBao = dcDeatil.SoBao
                };

                //Hủy số
                if (newDCDetail.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.cancel)
                {
                    DCHuySo(newDCDetail, _dieuchinh);
                }
                //Dồn số
                else if (newDCDetail.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.group)
                {
                    DCDonSo(newDCDetail, _dieuchinh);
                }
                //Số ra riêng
                else if (newDCDetail.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.add)
                {
                    DCSoRaRieng(newDCDetail, _dieuchinh);
                }
                //Thay đổi thông tin báo
                else
                {
                    DCThongTinBao(newDCDetail, _dieuchinh);
                }
            }
        }

        /// <summary>
        /// Điều chỉnh thay đổi thông tin báo
        /// </summary>
        /// <param name="_dieuchinhDetail"></param>
        /// <param name="_dieuchinh"></param>
        /// <param name="_config"></param>
        /// <modify>
        /// Author  Date    Comment
        /// Anhhn   3/8/15  Tách từ CreateDieuChinhKHXBDetail
        /// </modify>
        private void DCThongTinBao(BDieuChinhKHXBDetail _dieuchinhDetail, BDieuChinhKHXB _dieuchinh)
        {
            //Đọc dữ liệu thiếp lập điều chỉnh
            dynamic _config = JsonConvert.DeserializeObject<ExpandoObject>(_dieuchinhDetail.Config);
            //Nếu là thay đổi định kỳ thì lưu thứ, hoạc ngày điều chỉnh định kỳ
            if (_config.config.type == 1)
            {
                int[] numberConfig = ((List<dynamic>)_config.config.data).Select(d => d).ToList().ConvertAll(d => (int)Convert.ToInt32(d.ToString())).ToArray();
                _dieuchinhDetail.NgayOrThu = JsonConvert.SerializeObject(numberConfig);
            }
            string[] numberSoBao = ((List<dynamic>)_config.content.data).Select(d => d.number).ToList().ConvertAll(d => (string)d.ToString()).ToArray();
            _dieuchinhDetail.SoBao = JsonConvert.SerializeObject(numberSoBao);
            List<BKeHoachXuatBanDetail> _kehoach = getKeHoachXuatBanDetail(_dieuchinh.ThongTinBaoId, _dieuchinh.Nam, _dieuchinh.Quy).ToList();
            foreach (var item in _kehoach.Where(d => numberSoBao.Contains(d.SoBao)))
            {
                item.GiaBao = _dieuchinhDetail.GiaBao;
                item.TrongLuong = _dieuchinhDetail.TrongLuong;
                item.SoTrang = _dieuchinhDetail.SoTrang;
                item.KichThuoc = _dieuchinhDetail.KichThuoc;
                item.GhiChu = _dieuchinhDetail.GhiChu;
                item.Details = _dieuchinhDetail.NoiDung;
                if (item.status == (int)Enums.KeHoachXuatBan.approved)
                    item.status = (int)Enums.KeHoachXuatBan.changed;
            }
            db.BDieuChinhKHXBDetails.Add(_dieuchinhDetail);
            db.SaveChanges();
        }

        /// <summary>
        /// Điều chỉnh số ra riêng
        /// </summary>
        /// <param name="_dieuchinhDetail"></param>
        /// <param name="_dieuchinh"></param>
        /// <param name="_config"></param>
        /// <modify>
        /// Author  Date    Comment
        /// Anhhn   3/8/15  Tách từ CreateDieuChinhKHXBDetail
        /// </modify>
        private void DCSoRaRieng(BDieuChinhKHXBDetail _dieuchinhDetail, BDieuChinhKHXB _dieuchinh)
        {
            //Đọc dữ liệu thiếp lập điều chỉnh
            dynamic _config = JsonConvert.DeserializeObject<ExpandoObject>(_dieuchinhDetail.Config);

            BKeHoachXuatBanDetail _detailNew = new BKeHoachXuatBanDetail();
            _detailNew.ThongTinBaoId = _dieuchinh.ThongTinBaoId;
            _detailNew.Nam = _dieuchinh.Nam;
            _detailNew.Quy = _dieuchinh.Quy;


            _detailNew.GiaBao = _dieuchinhDetail.GiaBao;
            _detailNew.TrongLuong = _dieuchinhDetail.TrongLuong;
            _detailNew.SoTrang = _dieuchinhDetail.SoTrang;
            _detailNew.KichThuoc = _dieuchinhDetail.KichThuoc;
            _detailNew.GhiChu = _dieuchinhDetail.GhiChu;
            _detailNew.Details = _dieuchinhDetail.NoiDung;
            _detailNew.status = (int)Enums.KeHoachXuatBan.add;

            int[] _dateS = ((string)_config.content.data.time).Split('/').Select(d => Convert.ToInt32(d)).ToArray();
            DateTime _time = new DateTime(_dateS[2], _dateS[1], _dateS[0]);
            _detailNew.Number = -1;
            _detailNew.SoXuatBan = _config.content.data.data;
            _detailNew.Thang = _dateS[1];
            _detailNew.Ngay = _dateS[0];
            _detailNew.Thu = ((int)_time.DayOfWeek) == 0 ? 8 : ((int)_time.DayOfWeek) + 1;
            _detailNew.SoBao = Guid.NewGuid().ToString().Trim();
            _dieuchinhDetail.SoBao = "[\"" + _detailNew.SoBao + "\"]";

            db.BDieuChinhKHXBDetails.Add(_dieuchinhDetail);
            db.BKeHoachXuatBanDetails.Add(_detailNew);
            db.SaveChanges();
        }

        /// <summary>
        /// Điểu chỉnh dồn số
        /// </summary>
        /// <param name="_dieuchinhDetail"></param>
        /// <param name="_dieuchinh"></param>
        /// <param name="_config"></param>
        /// <modify>
        /// Author  Date    Comment
        /// Anhhn   3/8/15  Tách từ CreateDieuChinhKHXBDetail
        /// </modify>
        private void DCDonSo(BDieuChinhKHXBDetail _dieuchinhDetail, BDieuChinhKHXB _dieuchinh)
        {
            //Đọc dữ liệu thiếp lập điều chỉnh
            dynamic _config = JsonConvert.DeserializeObject<ExpandoObject>(_dieuchinhDetail.Config);

            if (_config.config.type == 1)
            {
                int[] numberConfig = ((List<dynamic>)_config.config.data).Select(d => d).ToList().ConvertAll(d => (int)Convert.ToInt32(d.ToString())).ToArray();
                _dieuchinhDetail.NgayOrThu = JsonConvert.SerializeObject(numberConfig);
            }
            List<BKeHoachXuatBanDetail> _kehoach = getKeHoachXuatBanDetail(_dieuchinh.ThongTinBaoId, _dieuchinh.Nam, _dieuchinh.Quy).ToList();
            List<BKeHoachXuatBanDetail> khxbdetailAdd = new List<BKeHoachXuatBanDetail>();
            List<string> numberSoBao = new List<string>();
            foreach (var item in _config.content.data)
            {
                string[] arNum = ((List<object>)item.data).Select(d => d.ToString()).ToArray();
                numberSoBao.AddRange(arNum);
                BKeHoachXuatBanDetail _detailNew = new BKeHoachXuatBanDetail();
                foreach (var itemNumber in _kehoach.Where(d => arNum.Contains(d.SoBao)))
                {
                    itemNumber.DonSo = item.id;
                    itemNumber.GiaBao = _dieuchinhDetail.GiaBao;
                    itemNumber.TrongLuong = _dieuchinhDetail.TrongLuong;
                    itemNumber.SoTrang = _dieuchinhDetail.SoTrang;
                    itemNumber.KichThuoc = _dieuchinhDetail.KichThuoc;
                    itemNumber.GhiChu = _dieuchinhDetail.GhiChu;
                    itemNumber.Details = _dieuchinhDetail.NoiDung;
                    itemNumber.status = (int)Enums.KeHoachXuatBan.group;

                    _detailNew.ThongTinBaoId = itemNumber.ThongTinBaoId;
                    _detailNew.Nam = itemNumber.Nam;
                    _detailNew.Quy = itemNumber.Quy;
                    _detailNew.DonSo = item.id;

                    _detailNew.GiaBao = _dieuchinhDetail.GiaBao;
                    _detailNew.TrongLuong = _dieuchinhDetail.TrongLuong;
                    _detailNew.SoTrang = _dieuchinhDetail.SoTrang;
                    _detailNew.KichThuoc = _dieuchinhDetail.KichThuoc;


                    _detailNew.GhiChu = _dieuchinhDetail.GhiChu;
                    _detailNew.Details = _dieuchinhDetail.NoiDung;
                    _detailNew.status = (int)Enums.KeHoachXuatBan.group;
                    _detailNew.SoBao = item.id;

                }
                int[] _dateS = ((string)item.time).Split('/').Select(d => Convert.ToInt32(d)).ToArray();
                DateTime _time = new DateTime(_dateS[2], _dateS[1], _dateS[0]);
                _detailNew.Number = -1;
                _detailNew.SoXuatBan = string.Join(",", arNum);
                _detailNew.Thang = _dateS[1];
                _detailNew.Ngay = _dateS[0];
                _detailNew.Thu = ((int)_time.DayOfWeek) == 0 ? 8 : ((int)_time.DayOfWeek) + 1;
                khxbdetailAdd.Add(_detailNew);
            }
            _dieuchinhDetail.SoBao = JsonConvert.SerializeObject(numberSoBao);
            db.BDieuChinhKHXBDetails.Add(_dieuchinhDetail);
            db.BKeHoachXuatBanDetails.AddRange(khxbdetailAdd);
            db.SaveChanges();
        }

        /// <summary>
        /// Điều chỉnh hủy số
        /// </summary>
        /// <param name="_dieuchinhDetail"></param>
        /// <param name="_dieuchinh"></param>
        /// <param name="_config"></param>
        /// <modify>
        /// Author  Date    Comment
        /// Anhhn   3/8/15  Tách từ CreateDieuChinhKHXBDetail
        /// </modify>
        private void DCHuySo(BDieuChinhKHXBDetail _dieuchinhDetail, BDieuChinhKHXB _dieuchinh)
        {
            //Đọc dữ liệu thiếp lập điều chỉnh
            dynamic _config = JsonConvert.DeserializeObject<ExpandoObject>(_dieuchinhDetail.Config);

            if (_config.config.type == 1)
            {
                int[] numberConfig = ((List<dynamic>)_config.config.data).Select(d => d).ToList().ConvertAll(d => (int)Convert.ToInt32(d.ToString())).ToArray();
                _dieuchinhDetail.NgayOrThu = JsonConvert.SerializeObject(numberConfig);
            }
            string[] numberSoBao = ((List<dynamic>)_config.content.data).Select(d => d.number).ToList().ConvertAll(d => (string)d.ToString()).ToArray();
            _dieuchinhDetail.SoBao = JsonConvert.SerializeObject(numberSoBao);
            List<BKeHoachXuatBanDetail> _kehoach = getKeHoachXuatBanDetail(_dieuchinh.ThongTinBaoId, _dieuchinh.Nam, _dieuchinh.Quy).ToList();
            foreach (var item in _kehoach.Where(d => numberSoBao.Contains(d.SoBao)))
            {
                item.status = (int)Enums.KeHoachXuatBan.cancel;
                item.GhiChu = _dieuchinhDetail.GhiChu;
            }
            var _keHoachNoGroup = _kehoach.Where(d => d.Number != -1).ToList();
            List<BKeHoachXuatBanDetail> _kehoachUpateNumber = _keHoachNoGroup.Where(d => d.status != (int)Enums.KeHoachXuatBan.cancel).ToList();
            if (_kehoachUpateNumber.Count > 0)
            {
                int numberCancelFirst = _keHoachNoGroup.Where(d => d.status == (int)Enums.KeHoachXuatBan.group).Count() > 0 ? _keHoachNoGroup.Where(d => d.status == (int)Enums.KeHoachXuatBan.group).FirstOrDefault().Number : -1;
                int numberStart = numberCancelFirst == -1 ? _kehoachUpateNumber[0].Number : numberCancelFirst < _kehoachUpateNumber[0].Number ? numberCancelFirst : _kehoachUpateNumber[0].Number;
                foreach (var item in _kehoachUpateNumber)
                {
                    item.Number = numberStart;
                    numberStart++;
                }
                foreach (var item in _kehoach.Where(d => d.Number == -1 && d.status == (int)Enums.KeHoachXuatBan.group).ToList())
                {
                    item.SoXuatBan = string.Join(",", _kehoachUpateNumber.Where(d => d.DonSo != null && d.DonSo.Equals(item.SoBao)).Select(d => d.Number));
                }
            }
            db.BDieuChinhKHXBDetails.Add(_dieuchinhDetail);
            db.SaveChanges();
        }
        public List<BDieuChinhKHXB> getBDieuChinhKHXB(string id, int year)
        {
            return db.BDieuChinhKHXBs.Where(d => d.ThongTinBaoId.Equals(id) && d.Nam == year).OrderByDescending(d => d.Quy).ToList();
        }
        public List<BKeHoachXuatBanDetail> getBKeHoachXuatBanDetail(string id, int year)
        {
            return db.BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(id) && d.Nam == year).OrderBy(d => d.Number).ToList();
        }
        public List<BDieuChinhKHXBDetailModel> getBKeHoachXuatBanDetailModel(string id)
        {
            List<BDieuChinhKHXBDetailModel> lst = new List<BDieuChinhKHXBDetailModel>();
            lst = (from sc in db.BDieuChinhKHXBDetails.Where(a=>a.DieuChinhKHXBId.Equals(id))
                  join soc in db.UserInfoes on sc.CreateBy equals soc.Id
                  join soc1 in db.UserInfoes on sc.ModifyBy equals soc1.Id into _soc
                  from soc2 in _soc.DefaultIfEmpty()
                  select  new BDieuChinhKHXBDetailModel()
                    {
                        Id = sc.Id,
                        DieuChinhKHXBId = sc.DieuChinhKHXBId,
                        LoaiDieuChinh = sc.LoaiDieuChinh,
                        CreateDate = sc.CreateDate,
                        CreateBy = sc.CreateBy,
                        CreateByName = soc.DislayName,
                        ModifyDate = sc.ModifyDate,
                        ModifyBy = sc.ModifyBy,
                        ModifyByName = soc2== null? "" : soc2.DislayName,
                        SoBao = sc.SoBao,
                        GiaBao = sc.GiaBao,
                        TrongLuong = sc.TrongLuong,
                        SoTrang = sc.SoTrang,
                        KichThuoc = sc.KichThuoc,
                        Config = sc.Config,
                        NoiDung = sc.NoiDung,
                        GhiChu = sc.GhiChu,
                        NgayOrThu = sc.NgayOrThu,
                        BDieuChinhPhanHuongDistricts = sc.BDieuChinhPhanHuongDistricts,
                        BDieuChinhPhanHuongUnits = sc.BDieuChinhPhanHuongUnits,
                        BDieuChinhKHXB = sc.BDieuChinhKHXB
                    }).ToList();
            
            return lst;
            //return db.BDieuChinhKHXBDetails.Where(d => d.DieuChinhKHXBId.Equals(id)).Select(new BDieuChinhKHXBDetailModel { }).ToList();
        }
        public IQueryable<BKeHoachXuatBanDetail> getKeHoachXuatBanDetail(string baoID, int Nam, int Quy)
        {

            return db.BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(baoID) && d.Nam == Nam && d.Quy == Quy);
        }
        public int getBKeHoachXuatBanYearLast(string id)
        {
            int year = db.BKeHoachXuatBans.Where(d => d.ThongTinBaoId.Equals(id)).OrderByDescending(d => d.Nam).Select(d => d.Nam).FirstOrDefault();
            year = year != 0 ? Convert.ToInt32(year) + 1 : DateTime.Now.Year;
            return year;
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
                result.ModifyDate = Convert.ToDateTime(objKXB.ModifyDate);
            }
            else
            {
                result.LoaiKy = -1;
                result.KyConfig = "[\"id\":0,\"data\":[]]";

            }
            return result;
        }
        dynamic getInForDay(int year, int month, int day)
        {
            DateTime date = new DateTime(year, month, day);
            dynamic data = new ExpandoObject();
            data.year = year;
            data.month = month;
            data.day = day;
            data.quy = month >= 1 && month <= 3 ? 1 : month > 3 && month <= 6 ? 2 : month > 6 && month <= 9 ? 3 : 4;
            data.thu = ((int)date.DayOfWeek) == 0 ? 8 : ((int)date.DayOfWeek) + 1;
            return data;
        }
        public BKeHoachXuatBanDetail addDataXuatBanDetail(string id, dynamic time, int number, int status)
        {
            BKeHoachXuatBanDetail data = new BKeHoachXuatBanDetail();
            data.ThongTinBaoId = id;
            data.Nam = Convert.ToInt32(time.year);
            data.Thang = Convert.ToInt32(time.month);
            data.Ngay = Convert.ToInt32(time.day);
            data.Quy = Convert.ToInt32(time.quy);
            data.Thu = Convert.ToInt32(time.thu);
            data.SoBao = number.ToString();
            data.Number = number;
            data.status = status;
            return data;
        }
        public void insertKeHoachChiTiet(string id, int year, int month, int type, string dataConfig, int startSoBao, int status,int day)
        {
            startSoBao = startSoBao <= 0 ? 1 : startSoBao;
            startSoBao = startSoBao - 1;
            List<BKeHoachXuatBanDetail> arrayInsert = new List<BKeHoachXuatBanDetail>();
            if (type == 0)
            {
                for (int i = month; i <= 12; i++)
                {
                    int _day = i == month ? day == -1 ? 1 : day : 1;
                    for (int j = _day; j <= DateTime.DaysInMonth(year, i); j++)
                    {
                        ++startSoBao;
                        arrayInsert.Add(addDataXuatBanDetail(id, getInForDay(year, i, j), startSoBao, status));
                    }
                }
            }
            else if (type == 1)
            {
                int[] inThu = JsonConvert.DeserializeObject<string[]>(dataConfig).Select(d => Convert.ToInt32(d)).ToArray();
                for (int i = month; i <= 12; i++)
                {
                    int _day = i == month ? day == -1 ? 1 : day : 1;
                    for (int j = _day; j <= DateTime.DaysInMonth(year, i); j++)
                    {
                        dynamic timeinfo = getInForDay(year, i, j);
                        int Thu = Convert.ToInt32(timeinfo.thu);
                        if (inThu.Contains(Thu))
                        {
                            ++startSoBao;
                            arrayInsert.Add(addDataXuatBanDetail(id, getInForDay(year, i, j), startSoBao, status));
                        }

                    }
                }
            }
            else if (type == 2)
            {
                int[] inNgay = JsonConvert.DeserializeObject<string[]>(dataConfig).Select(d => Convert.ToInt32(d)).ToArray();
                for (int i = 2; i <= 12; i += 2)
                {
                    if (i >= month)
                    {
                        int _day = i == month ? day == -1 ? 1 : day : 1;
                        for (int j = _day; j <= DateTime.DaysInMonth(year, i); j++)
                        {
                            if (inNgay.Contains(j))
                            {
                                ++startSoBao;
                                arrayInsert.Add(addDataXuatBanDetail(id, getInForDay(year, i, j), startSoBao, status));
                            }

                        }
                    }
                }
            }
            else if (type == 3)
            {
                int[] inNgay = JsonConvert.DeserializeObject<string[]>(dataConfig).Select(d => Convert.ToInt32(d)).ToArray();
                for (int i = 1; i <= 11; i += 2)
                {
                    if (i >= month)
                    {
                        int _day = i == month ? day == -1 ? 1 : day : 1;
                        for (int j = _day; j <= DateTime.DaysInMonth(year, i); j++)
                        {
                            if (inNgay.Contains(j))
                            {
                                ++startSoBao;
                                arrayInsert.Add(addDataXuatBanDetail(id, getInForDay(year, i, j), startSoBao, status));
                            }

                        }
                    }
                }
            }
            else if (type >= 4 && type <= 6)
            {
                dynamic _data = JsonConvert.DeserializeObject<ExpandoObject>(dataConfig);
                List<dynamic> arraySelect = (List<dynamic>)_data.data;
                int _number = type == 4 ? Convert.ToInt32(_data.number) : type == 5 ? Convert.ToInt32(_data.number) * 3 : 12;
                int _kyBao = Convert.ToInt32(_data.ky);
                int loopFor = 12 / _number;
                loopFor = 12 % _number == 0 ? loopFor : loopFor + 1;
                for (int l = 0; l < loopFor; l++)
                {
                    int iNext = _number * (l + 1) > 12 ? 12 : _number * (l + 1);
                    for (int i = 1 + (l * _number); i <= iNext; i++)
                    {
                        if (i >= month)
                        {
                            int _day = i == month ? day == -1 ? 1 : day : 1;
                            int iDung = i - (l * _number);
                            int maxDay = DateTime.DaysInMonth(year, i);
                            dynamic[] _dataSelect = (from p in arraySelect where Convert.ToInt32(p.month) == iDung select p).ToArray();
                            foreach (dynamic item in _dataSelect)
                            {
                                int daySelect = Convert.ToInt32(item.day);
                                if (i == month && day!=-1)
                                {
                                    if (maxDay >= daySelect && daySelect>day)
                                    {
                                        ++startSoBao;
                                        arrayInsert.Add(addDataXuatBanDetail(id, getInForDay(year, i, daySelect), startSoBao, status));
                                    }
                                }
                                else
                                {
                                    if (maxDay >= daySelect)
                                    {
                                        ++startSoBao;
                                        arrayInsert.Add(addDataXuatBanDetail(id, getInForDay(year, i, daySelect), startSoBao, status));
                                    }
                                }
                            }
                        }
                    }
                }
            }


            if (arrayInsert.Count > 0)
            {
                db.BKeHoachXuatBanDetails.AddRange(arrayInsert);
                db.SaveChanges();
            }
        }
        public int updateBKeHoachXuatBan(string id, int year, int soBatdau, string userID)
        {
            BaoKyXuatBanModel _ky = getBaoKyXuatBanModel(id);
            BKeHoachXuatBan _keHoach = (from p in db.BKeHoachXuatBans where p.ThongTinBaoId.CompareTo(id) == 0 && p.Nam == year select p).FirstOrDefault();
            int numberStart = 1;
            if (_ky.LoaiKy > -1)
            {
                if (_keHoach == null)
                {
                    _keHoach = new BKeHoachXuatBan();
                    _keHoach.CreateBy = userID;
                    _keHoach.CreateDate = DateTime.Now;
                    _keHoach.Id = Guid.NewGuid().ToString().Trim();
                    _keHoach.ModifyBy = userID;
                    _keHoach.ModifyDate = DateTime.Now;
                    _keHoach.Nam = year;
                    _keHoach.ThongTinBaoId = id;
                    if (soBatdau >= 0){
                        _keHoach.SoBatDau = soBatdau == 0 ? 1 : soBatdau;
                        numberStart = _keHoach.SoBatDau;
                    }
                    else
                    {
                        _keHoach.SoBatDau = -1;
                        numberStart = (from p in db.BKeHoachXuatBanDetails where p.ThongTinBaoId.Equals(id) && p.Nam == (year - 1) orderby p.Number descending select p.Number).FirstOrDefault();
                        numberStart = numberStart == 0 ? 1 : numberStart + 1;
                    }
                    _keHoach.LoaiKy = _ky.LoaiKy;
                    _keHoach.ChiTietKy = _ky.KyConfig;
                    db.BKeHoachXuatBans.Add(_keHoach);
                    db.SaveChanges();

                    //Tạo ngày xuất bản
                    insertKeHoachChiTiet(id, year, 1, _ky.LoaiKy, JsonConvert.SerializeObject(JsonConvert.DeserializeObject<dynamic>(_ky.KyConfig).data), numberStart, (int)Enums.KeHoachXuatBan.create, -1);
                    //
                }
                else
                {
                    if (_ky.ModifyDate > _keHoach.ModifyDate)
                    {
                        int quy = db.BDieuChinhKHXBs.Where(d => d.ThongTinBaoId.Equals(id) && d.Nam == year).Select(d => d.Quy).OrderByDescending(d => d).FirstOrDefault();
                        quy = quy == 0 ? 1 : quy+1;
                        int month = ((quy - 1) * 3) + 1;
                        if (month <= 12)
                        {
                            int day = 0;
                            if (DateTime.Now.Year==year && month <= DateTime.Now.Month)
                            {
                                day = DateTime.Now.Day;
                                month = DateTime.Now.Month;
                            }
                            var arra_delete = db.BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(id) && d.Nam == year && ( (d.Thang==month && d.Ngay>day) || d.Thang>month )).OrderBy(d => d.Number);
                            List<BKeHoachXuatBanDetail> ac = arra_delete.ToList();
                            
                            int soNext = 1;
                            if (_keHoach.SoBatDau == -1 && month == 1 && day==0)
                            {
                                soNext = (from p in db.BKeHoachXuatBanDetails where p.ThongTinBaoId.Equals(id) && p.Nam == (year - 1) orderby p.Number descending select p.Number).FirstOrDefault();
                                soNext = soNext == 0 ? 1 : soNext + 1;
                            }
                            else
                            {
                                soNext = arra_delete.Select(d => d.Number).FirstOrDefault();
                                soNext = soNext == 0 ? 1 : soNext;
                            }
                            db.BKeHoachXuatBanDetails.RemoveRange(arra_delete);
                            db.SaveChanges();
                            insertKeHoachChiTiet(id, year, month, _ky.LoaiKy, JsonConvert.SerializeObject(JsonConvert.DeserializeObject<dynamic>(_ky.KyConfig).data), soNext, (int)Enums.KeHoachXuatBan.create,day+1);
                        }
                    }

                    _keHoach.LoaiKy = _ky.LoaiKy;
                    _keHoach.ChiTietKy = _ky.KyConfig;
                    _keHoach.ModifyBy = userID;
                    _keHoach.ModifyDate = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return 0;
        }

        public BKeHoachXuatBan getBKeHoachXuatBan(string id)
        {
            return db.BKeHoachXuatBans.Where(p => p.Id.Equals(id)).FirstOrDefault();
        }

    }
}
