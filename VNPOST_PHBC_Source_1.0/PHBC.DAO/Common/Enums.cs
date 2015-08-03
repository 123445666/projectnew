using System;

namespace PHBC.DAO.Common
{
    public class Enums
    {

        public enum RecordStatusCode : byte
        {
            active = 1,
            deactive = 2,
            delete = 4,
        }
        public static string RecordStatusDesc(RecordStatusCode _kts)
        {
            switch (_kts)
            {
                case RecordStatusCode.active:
                    return "Active";
                case RecordStatusCode.deactive:
                    return "Deactive";
                case RecordStatusCode.delete:
                    return "Xóa";
                default:
                    return "";

            }
        }

        public enum KeHoachXuatBan : byte
        {
            create = 1,  // Mới tạo chưa điều chỉnh
            approved = 2, // Đã điều chỉnh
            cancel = 3, // Hủy số, báo hủy
            group = 4, // Dồn số
            add = 5, // Báo ra riêng
            changed = 6 // Điều chỉnh thông tín báo
        }
        public static KeHoachXuatBan getKeHoachXuatBan(string type)
        {
            switch (type)
            {
                case "baonghi":
                    return KeHoachXuatBan.cancel;
                case "donso":
                    return KeHoachXuatBan.group;
                case "baorarieng":
                    return KeHoachXuatBan.add;
                case "dieuchinhthongtinbao":
                    return KeHoachXuatBan.changed;
                default:
                    return KeHoachXuatBan.cancel;
            }
        }
        public static string KieuDieuChinhKHXB(KeHoachXuatBan type)
        {
            switch (type)
            {
                case KeHoachXuatBan.cancel:
                    return "Báo nghỉ";
                case KeHoachXuatBan.group:
                    return "Dồn số";
                case KeHoachXuatBan.add:
                    return "Báo ra riêng";
                case KeHoachXuatBan.changed:
                    return "Điều chỉnh thông tin báo";
                default:
                    return "";
            }
        }
        public enum KieuToaSoan : byte
        {
            TrungUong = 1,
            DiaPhuong = 2,
        }

        public enum KyXuatBan : byte
        {
            HangNgay = 0,
            HangTuan = 1,
            ThangChan = 2,
            ThangLe = 3,
            Thang = 4,
            Quy = 5,
            Nam = 6
        }


        public static string KieuKyXuatBan(int type)
        {
            return KieuKyXuatBan((KyXuatBan)type);
        }
        public static string KieuKyXuatBan(KyXuatBan type)
        {
            switch (type)
            {
                case KyXuatBan.HangNgay:
                    return "Hàng ngày";
                case KyXuatBan.HangTuan:
                    return "Hàng tuần";
                case KyXuatBan.ThangChan:
                    return "Tháng chẵn";
                case KyXuatBan.ThangLe:
                    return "Tháng lẻ";
                case KyXuatBan.Thang:
                    return "Tháng";
                case KyXuatBan.Quy:
                    return "Quý";
                case KyXuatBan.Nam:
                    return "Năm";
                default:
                    return "";
            }
        }

        public static string KieuToaSoanDesc(KieuToaSoan _kts)
        {
            switch (_kts)
            {
                case KieuToaSoan.DiaPhuong:
                    return "Tòa soạn địa phương";
                case KieuToaSoan.TrungUong:
                    return "Tòa soạn trung ương";
                default:
                    return "";
            }
        }

        public enum RoleLevel : byte
        {
            VN_POST = 1,
            PHBC_TW = 2,
            BDT = 3,
            BDH = 4,
        }
        public struct RoleLevelDesc
        {
            public const string VN_POST = "VN Post";
            public const string PHBC_TW = "CTy PHBC";
            public const string BDT = "Bưu điện tỉnh";
            public const string BDH = "Bưu điện huyện";
        }
        public enum FormAction : byte
        {
            Create = 1,
            Edit = 2,
            NoAction = 3,
        }

        public struct RegexDefine
        {
            public const string Email = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            public const string Interger = "^0|([1-9]+[0-9]*)$";
            public const string Numeric = @"^\d*\.?\d*$";
            public const string PhoneNumber = @"^\+[0-9]{1,3}\.[0-9]+\.[0-9]+$";
            public const string AscII = "^[a-zA-Z]+$";
            public const string Unicode = "^([ \u00c0-\u01ffa-zA-Z'])+$";
        }
        public struct RegexMessage
        {
            public const string Email = "";
            public const string Interger = "{0} phải là kiểu số.";
            public const string Numeric = "{0} phải là kiểu số.";
            public const string PhoneNumber = "{0} phải là .";
            public const string AscII = "";
            public const string Unicode = "";
        }
        public struct ErrorMessage
        {
            public const string Required = "{0} không được trống.";
            public const string StringLengthMax = "{0} không được quá {1} ký tự.";
            public const string WrongFormat = "{0} không đúng định dạng.";
            public const string WrongFormatString = "{0} không đúng định dạng (chỉ nhập chữ và số).";
            public const string WrongFormatMST = "{0} không đúng định dạng (chỉ nhập số - và dấu cách).";
            public const string StringLengthMinMax = "{0} phải có từ {1} tới {2} ký tự.";
            public const string SameKey = "{0} này đã được sử dụng.";
            public const string OnlyNumber = "{0} này chỉ được nhập số.";
            public const string RangeMinMax = "{0} phải có giá trị từ {1} tới {2}.";
            //Bussiness
            public const string Exists = "{0} đã tồn tại.";
            public const string NotExists = "{0} không tồn tại.";

        }
        /// <summary>
        /// Enum for Bao ngoai van
        /// </summary>
        public enum BaoNgoaiVan : byte
        {
            BaoTrongNuoc = 0,
            BaoNhapKhau = 1,
        }

        public static string BaoNgoaiVanDesc(BaoNgoaiVan _bgv)
        {
            switch (_bgv)
            {
                case BaoNgoaiVan.BaoTrongNuoc:
                    return "Báo chí trong nước";
                case BaoNgoaiVan.BaoNhapKhau:
                    return "Báo chí nhập khẩu";
                default:
                    return "";
            }
        }
        /// <summary>
        /// Enum for Bao trong danh mục
        /// </summary>
        public enum BaoTrongDanhMuc : byte
        {
            BaoTrongDanhMuc = 0,
            BaoNgoaiDanhMuc = 1,
        }

        public static string BaoTrongDanhMucDesc(BaoTrongDanhMuc _btdm)
        {
            switch (_btdm)
            {
                case BaoTrongDanhMuc.BaoTrongDanhMuc:
                    return "Báo trong danh mục";
                case BaoTrongDanhMuc.BaoNgoaiDanhMuc:
                    return "Báo ngoài danh mục";
                default:
                    return "";
            }
        }
        /// <summary>
        /// Enum for báo trung ương - báo địa phương
        /// </summary>
        public enum BaoTrungUongDiaPhuong : byte
        {
            BaoTrungUong = 0,
            BaoDiaPhuong = 1,
        }

        public static string BaoTrungUongDiaPhuongDesc(BaoTrungUongDiaPhuong _btwdp)
        {
            switch (_btwdp)
            {
                case BaoTrungUongDiaPhuong.BaoTrungUong:
                    return "Báo trung ương";
                case BaoTrungUongDiaPhuong.BaoDiaPhuong:
                    return "Báo địa phương";
                default:
                    return "";
            }
        }
        /// <summary>
        /// Enum for báo có thuế - bảo không thuế
        /// </summary>
        public enum BaoCoThueKhongThue : byte
        {
            BaoCoThue = 0,
            BaoKhongThue = 1,
        }

        public static string BaoCoThueKhongThueDesc(BaoCoThueKhongThue _btwdp)
        {
            switch (_btwdp)
            {
                case BaoCoThueKhongThue.BaoCoThue:
                    return "Báo có thuế";
                case BaoCoThueKhongThue.BaoKhongThue:
                    return "Báo không thuế";
                default:
                    return "";
            }
        }
        /// <summary>
        /// Enum for báo công ích ngoài công ích
        /// </summary>
        public enum BaoCongIchNgoaiCongIch : byte
        {
            BaoCongIch = 1,
            BaoNgoaiCongIch = 0,
        }

        public static string BaoCongIchNgoaiCongIchDesc(BaoCongIchNgoaiCongIch _btwdp)
        {
            switch (_btwdp)
            {
                case BaoCongIchNgoaiCongIch.BaoCongIch:
                    return "Báo công ích";
                case BaoCongIchNgoaiCongIch.BaoNgoaiCongIch:
                    return "Báo ngoài công ích";
                default:
                    return "";
            }
        }

        public enum DanhMucDungChung : byte
        {
            ThongTinBao = 3
        }

        public enum KieuDuLieu : byte
        {
            text = 99,
            Interger = 0,
            Numeric = 1,
            Date = 2,
            Bool = 3,
            Email = 4,
            PhoneNumber = 5,
        }

        public enum LoaiGia : byte
        {
            GiaMua = 1,
            GiaBan = 0,
        }
        public struct LoaiGiaDesc
        {
            public const string GiaMua = "Giá Mua";
            public const string GiaBan = "Giá Bán";
        }

        public struct FormatType
        {
            public const string ForMatDateVN = "dd/MM/yyyy";
        }

    }
}