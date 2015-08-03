using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Dynamic;

namespace PHBC.DAO.Bussiness
{
    public class DieuChinhKHXBBussiness : IDieuChinhKHXBBussiness
    {
        DB_PHBCEntities db;
        public DieuChinhKHXBBussiness()
        {
            db = new DB_PHBCEntities();
        }
        public DieuChinhKHXBBussiness(DB_PHBCEntities _db)
        {
            db = _db;
        }
        public DB_PHBCEntities getDBSelect()
        {
            return db;
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
        public List<DCThongTinBaoEditModel> getListDCThongTinBao(string thongTinBaoId)
        {
            int type = (int)Enums.DanhMucDungChung.ThongTinBao;
            List<DCThongTinBaoEditModel> result = (from dm in db.SysDMPublics.Where(a => a.TypeId == type)
                                                   where dm.TypeId == type
                                                   select new DCThongTinBaoEditModel()
                                               {
                                                   Dislay = dm.Name,
                                                   Key = dm.Code
                                               }).ToList();
            BThongTinBao bao = db.BThongTinBaos.Find(thongTinBaoId);
            Type _type;
            if (bao != null)
            {
                result.ForEach(a =>
                {
                    object value = Utils.getValueProperty(bao, a.Key, out _type);
                    a.setValue(value, _type);
                });
            }
            return result;
        }

        public BDieuChinhKHXB getDieuChinhKHXB(string id)
        {
            return db.BDieuChinhKHXBs.Find(id);
        }

        public BDieuChinhKHXBDetail getDieuChinhKHXBDetail(string id)
        {
            return db.BDieuChinhKHXBDetails.Include(a => a.BDieuChinhKHXB).FirstOrDefault(a => a.Id == id);
        }

        public IQueryable<BKeHoachXuatBanDetail> getKeHoachXuatBanDetail(string baoID, int Nam, int Quy)
        {

            return db.BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(baoID) && d.Nam == Nam && d.Quy == Quy);
        }
        public void updateOldChiTiet(BKeHoachXuatBanDetail item, BDieuChinhKHXBDetail dieuChinhOld)
        {
            BDieuChinhKHXBDetail _dieuchinhDetail = db.BDieuChinhKHXBDetails.Where(d => !d.Id.Equals(dieuChinhOld.Id) && !d.DieuChinhKHXBId.Equals(dieuChinhOld.DieuChinhKHXBId) && d.SoBao.IndexOf("\"" + item.SoBao + "\"") > -1 && (d.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.changed || d.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.add || d.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.group)).OrderByDescending(d => d.ModifyDate).FirstOrDefault();
            if (_dieuchinhDetail == null)
            {
                item.GiaBao = null;
                item.TrongLuong = null;
                item.SoTrang = null;
                item.KichThuoc = null;
                item.GhiChu = null;
                item.Details = null;
            }
            else
            {
                item.GiaBao = _dieuchinhDetail.GiaBao;
                item.TrongLuong = _dieuchinhDetail.TrongLuong;
                item.SoTrang = _dieuchinhDetail.SoTrang;
                item.KichThuoc = _dieuchinhDetail.KichThuoc;
                item.GhiChu = _dieuchinhDetail.GhiChu;
                item.Details = _dieuchinhDetail.NoiDung;
            }
        }
        public void deleteDieuChinhKHXBDetail(string dcIDDetail)
        {
            BDieuChinhKHXBDetail _dieuchinhDetail = getDieuChinhKHXBDetail(dcIDDetail);
            BDieuChinhKHXB _dieuchinh = getDieuChinhKHXB(_dieuchinhDetail.DieuChinhKHXBId);
            string[] numberSoBao = JsonConvert.DeserializeObject<string[]>(_dieuchinhDetail.SoBao);
            List<BKeHoachXuatBanDetail> _kehoach = getKeHoachXuatBanDetail(_dieuchinh.ThongTinBaoId, _dieuchinh.Nam, _dieuchinh.Quy).ToList();
            if (_dieuchinhDetail.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.cancel)
            {
                foreach (var item in _kehoach.Where(d => numberSoBao.Contains(d.SoBao)))
                {
                    item.status = (int)Enums.KeHoachXuatBan.approved;
                    item.GhiChu = null;
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
            }
            else if (_dieuchinhDetail.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.group)
            {
                List<string> arrayID = new List<string>();
                foreach (var item in _kehoach.Where(d => numberSoBao.Contains(d.SoBao)))
                {
                    if (!arrayID.Contains(item.DonSo))
                        arrayID.Add(item.DonSo);
                    item.DonSo = null;
                    updateOldChiTiet(item, _dieuchinhDetail);
                    item.status = (int)Enums.KeHoachXuatBan.approved;
                }
                db.BKeHoachXuatBanDetails.RemoveRange(_kehoach.Where(d => d.Number == -1 && arrayID.Contains(d.DonSo)));
            }
            else if (_dieuchinhDetail.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.add)
            {
                db.BKeHoachXuatBanDetails.RemoveRange(_kehoach.Where(d => d.Number == -1 && numberSoBao.Contains(d.SoBao)));
            }
            else if (_dieuchinhDetail.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.changed)
            {
                foreach (var item in _kehoach.Where(d => numberSoBao.Contains(d.SoBao) && d.status == (int)Enums.KeHoachXuatBan.changed))
                {
                    updateOldChiTiet(item, _dieuchinhDetail);
                    item.status = (int)Enums.KeHoachXuatBan.approved;
                }
            }
            db.BDieuChinhKHXBDetails.Remove(_dieuchinhDetail);
            db.SaveChanges();
        }
        /// <summary>
        /// Thêm điều chỉnh chi tiết cho quy
        /// </summary>
        /// <param name="dcId">Id điều chỉnh quý</param>
        /// <param name="LoaiDieuChinh">Loại điều chỉnh - Enums.KeHoachXuatBan</param>
        /// <param name="config">Thiết lập điều chỉnh chi tiết</param>
        /// <param name="lstThongTin">Danh sách điều chỉnh thông tin báo</param>
        /// <param name="GhiChu"></param>
        /// <param name="userCreate">Id người tạo</param>
        /// <returns></returns>
        /// <modify>
        /// Author  Date    comment
        /// Thang   ?       Tạo mới
        /// Anhhn   3/8/15  Sửa-Tách các loại điểu chỉnh ra thành các hàm riêng
        /// </modify>
        public ErrorObject CreateDieuChinhKHXBDetail(string dcId, int LoaiDieuChinh, string config, List<DCThongTinBaoModel> lstThongTin, string GhiChu, string userCreate)
        {
            ErrorObject err = new ErrorObject();
            lstThongTin = lstThongTin ?? new List<DCThongTinBaoModel>();
            BDieuChinhKHXB _dieuchinh = getDieuChinhKHXB(dcId);
            //Không tìm thấy điểu chỉnh
            if(_dieuchinh == null)
            {
                err.HasError = true;
                err.LstError.Add("", "Không có điều chỉnh");
                return err;
            }
            
            //Gán dữ liệu điều chỉnh detail
            BDieuChinhKHXBDetail _dieuchinhDetail = new BDieuChinhKHXBDetail();
            _dieuchinhDetail.Id = Guid.NewGuid().ToString();
            _dieuchinhDetail.DieuChinhKHXBId = _dieuchinh.Id;
            _dieuchinhDetail.Config = config;
            _dieuchinhDetail.CreateBy = userCreate;
            _dieuchinhDetail.CreateDate = DateTime.Now;
            _dieuchinhDetail.ModifyBy = userCreate;
            _dieuchinhDetail.ModifyDate = _dieuchinhDetail.CreateDate;

            _dieuchinhDetail.GhiChu = GhiChu;
            _dieuchinhDetail.LoaiDieuChinh = LoaiDieuChinh;
            _dieuchinhDetail.NoiDung = JsonConvert.SerializeObject(lstThongTin);
            //
            if (lstThongTin.Where(d => d.Key.Equals("GiaBia")).Count() > 0)
                _dieuchinhDetail.GiaBao = Convert.ToInt32(lstThongTin.Where(d => d.Key.Equals("GiaBia")).FirstOrDefault().Value);
            if (lstThongTin.Where(d => d.Key.Equals("TrongLuong")).Count() > 0)
                _dieuchinhDetail.TrongLuong = Convert.ToInt32(lstThongTin.Where(d => d.Key.Equals("TrongLuong")).FirstOrDefault().Value);
            if (lstThongTin.Where(d => d.Key.Equals("SoTrang")).Count() > 0)
                _dieuchinhDetail.SoTrang = Convert.ToInt32(lstThongTin.Where(d => d.Key.Equals("SoTrang")).FirstOrDefault().Value);
            if (lstThongTin.Where(d => d.Key.Equals("KichThuoc")).Count() > 0)
                _dieuchinhDetail.KichThuoc = lstThongTin.Where(d => d.Key.Equals("KichThuoc")).FirstOrDefault().Value;

            //Hủy số
            if (LoaiDieuChinh == (int)Enums.KeHoachXuatBan.cancel)
            {
                DCHuySo(_dieuchinhDetail, _dieuchinh);
            }
            //Dồn số
            else if (LoaiDieuChinh == (int)Enums.KeHoachXuatBan.group)
            {
                DCDonSo(_dieuchinhDetail, _dieuchinh);
            }
            //Số ra riêng
            else if (LoaiDieuChinh == (int)Enums.KeHoachXuatBan.add)
            {
                DCSoRaRieng(_dieuchinhDetail, _dieuchinh);
            }
            //Thay đổi thông tin báo
            else
            {
                DCThongTinBao(_dieuchinhDetail, _dieuchinh);
            }
            return err;
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
        public ErrorObject EditDieuChinhKHXBDetail(string dcDetailId, List<DCThongTinBaoModel> lstThongTin, string GhiChu)
        {
            lstThongTin = lstThongTin ?? new List<DCThongTinBaoModel>();
            BDieuChinhKHXBDetail dcKHXBDetail = getDieuChinhKHXBDetail(dcDetailId);
            BDieuChinhKHXB _dieuchinh = getDieuChinhKHXB(dcKHXBDetail.DieuChinhKHXBId);

            /////////////
            dcKHXBDetail.NoiDung = JsonConvert.SerializeObject(lstThongTin);

            if (lstThongTin.Where(d => d.Key.Equals("GiaBia")).Count() > 0)
                dcKHXBDetail.GiaBao = Convert.ToInt32(lstThongTin.Where(d => d.Key.Equals("GiaBia")).FirstOrDefault().Value);
            if (lstThongTin.Where(d => d.Key.Equals("TrongLuong")).Count() > 0)
                dcKHXBDetail.TrongLuong = Convert.ToInt32(lstThongTin.Where(d => d.Key.Equals("TrongLuong")).FirstOrDefault().Value);
            if (lstThongTin.Where(d => d.Key.Equals("SoTrang")).Count() > 0)
                dcKHXBDetail.SoTrang = Convert.ToInt32(lstThongTin.Where(d => d.Key.Equals("SoTrang")).FirstOrDefault().Value);
            if (lstThongTin.Where(d => d.Key.Equals("KichThuoc")).Count() > 0)
                dcKHXBDetail.KichThuoc = lstThongTin.Where(d => d.Key.Equals("KichThuoc")).FirstOrDefault().Value;
            dcKHXBDetail.GhiChu = GhiChu;
            /////////////

            if (dcKHXBDetail.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.add)
            {
                BKeHoachXuatBanDetail item = getDBSelect().BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(_dieuchinh.ThongTinBaoId) && d.Nam == _dieuchinh.Nam && d.Quy == d.Quy && dcKHXBDetail.SoBao.IndexOf("\"" + d.SoBao + "\"") > -1).FirstOrDefault();
                item.GiaBao = dcKHXBDetail.GiaBao;
                item.TrongLuong = dcKHXBDetail.TrongLuong;
                item.SoTrang = dcKHXBDetail.SoTrang;
                item.KichThuoc = dcKHXBDetail.KichThuoc;
                item.GhiChu = dcKHXBDetail.GhiChu;
                item.Details = dcKHXBDetail.NoiDung;
            }
            else
            {
                List<BKeHoachXuatBanDetail> arrNumber = getDBSelect().BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(_dieuchinh.ThongTinBaoId) && d.Nam == _dieuchinh.Nam && d.Quy == d.Quy && (dcKHXBDetail.SoBao.IndexOf("\"" + d.SoBao + "\"") > -1 || (d.Number == -1 && d.status == (int)Enums.KeHoachXuatBan.group))).ToList<BKeHoachXuatBanDetail>();
                dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(dcKHXBDetail.Config);
                foreach (var item in config.content.data)
                {
                    BKeHoachXuatBanDetail soBao = (dcKHXBDetail.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.group ? arrNumber.Where(d => d.SoBao.Equals(item.id)).FirstOrDefault() : arrNumber.Where(d => d.SoBao.Equals(item.number + "")).FirstOrDefault());
                    soBao.GiaBao = dcKHXBDetail.GiaBao;
                    soBao.TrongLuong = dcKHXBDetail.TrongLuong;
                    soBao.SoTrang = dcKHXBDetail.SoTrang;
                    soBao.KichThuoc = dcKHXBDetail.KichThuoc;
                    soBao.GhiChu = dcKHXBDetail.GhiChu;
                    soBao.Details = dcKHXBDetail.NoiDung;
                    if (dcKHXBDetail.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.group)
                    {
                        foreach (var itemNew in arrNumber.Where(d => d.DonSo.Equals(soBao.SoBao)))
                        {
                            itemNew.GiaBao = dcKHXBDetail.GiaBao;
                            itemNew.TrongLuong = dcKHXBDetail.TrongLuong;
                            itemNew.SoTrang = dcKHXBDetail.SoTrang;
                            itemNew.KichThuoc = dcKHXBDetail.KichThuoc;
                            itemNew.GhiChu = dcKHXBDetail.GhiChu;
                            itemNew.Details = dcKHXBDetail.NoiDung;
                        }
                    }
                }
            }
            db.SaveChanges();
            return new ErrorObject();
        }
        public List<BDieuChinhKHXBDetailModel> getBKeHoachXuatBanDetailModel(string id)
        {
            List<BDieuChinhKHXBDetailModel> lst = new List<BDieuChinhKHXBDetailModel>();
            lst = (from sc in db.BDieuChinhKHXBDetails.Where(a => a.DieuChinhKHXBId.Equals(id))
                   join soc in db.UserInfoes on sc.CreateBy equals soc.Id
                   join soc1 in db.UserInfoes on sc.ModifyBy equals soc1.Id into _soc
                   from soc2 in _soc.DefaultIfEmpty()
                   select new BDieuChinhKHXBDetailModel()
                   {
                       Id = sc.Id,
                       DieuChinhKHXBId = sc.DieuChinhKHXBId,
                       LoaiDieuChinh = sc.LoaiDieuChinh,
                       CreateDate = sc.CreateDate,
                       CreateBy = sc.CreateBy,
                       CreateByName = soc.DislayName,
                       ModifyDate = sc.ModifyDate,
                       ModifyBy = sc.ModifyBy,
                       ModifyByName = soc2 == null ? "" : soc2.DislayName,
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

        //Ham huy
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
