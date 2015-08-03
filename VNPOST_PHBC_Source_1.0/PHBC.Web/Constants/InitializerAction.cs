using PHBC.DAO.Models;
using System.Collections.Generic;
namespace PHBC.Web.Constants
{
    public static class InitializerAction
    {

        public static List<ActionDefine> getActionDefine()
        {
            List<ActionDefine> result = new List<ActionDefine>();
            //DieuChinhKHXB
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.DieuChinhKHXB,
                AreaDes = AreaDesc.DieuChinhKHXB,
                ControllerName = ControllerName.DieuChinhKHXB,
                ControllerDes = ControllerDesc.DieuChinhKHXB,
                ControllerIcon = "<i class=\"fa fa-users\"></i>",
                ActionCustom = "TaoMoi|ChinhSua|View|Delete|IndexDieuChinhPHNC|DetailsDieuChinhPHNC|CreateDieuChinhPHNC|EditDieuChinhPHNC|EditPHNCDieuChinhPHNC",
                ActionParam = "type,id|id|id|id|DieuChinhKHXBDetailId|provincecode,districtcode,DieuChinhKHXBDetailId|DieuChinhKHXBDetailId,provincecode|provincecode,DieuChinhKHXBDetailId|id",
                ActionCustomDes = "Tạo mới điều chỉnh kế hoạch xuất bản|Chỉnh sửa điều chỉnh kế hoạch xuất bản|Chi tiết điều chỉnh|Xóa điều chỉnh|Danh sách phân hướng điều chỉnh KHXB|Chi tiết phân hướng điều chỉnh KHXB|Thiết lập phân hướng điều chỉnh KHXB|Sửa thiết lập phân hướng điều chỉnh KHXB|Sửa thiết lập phân hướng điều chỉnh KHXB cho bưu cục",
                ActionCustomMenu = "0|0|0|0|0|0|0|0|0",
                ActionCustomIcon = "|",
                ActionIgnore = ActionType.GroupAdvance
            });
            //UserManager
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.Admin,
                AreaDes = AreaDesc.Admin,
                ControllerName = ControllerName.UserManager,
                ControllerDes = ControllerDesc.UserManager,
                ControllerIcon = "<i class=\"fa fa-users\"></i>",
                ActionCustom = "AddRole|SetPassword",
                ActionParam = "id|id",
                ActionCustomDes = "Cấu hình vai trò cho người dùng|Đặt mật khẩu cho người dùng",
                ActionCustomMenu = "0|0",
                ActionCustomIcon = "|",
                ActionIgnore = ActionType.GroupAdvance
            });
            //Role
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.Admin,
                AreaDes = AreaDesc.Admin,
                ControllerName = ControllerName.Role,
                ControllerDes = ControllerDesc.Role,
                ControllerIcon = "<i class=\"fa fa-university\"></i>",
                ActionCustom = "AddAction|AddUser",
                ActionParam = "id|id",
                ActionCustomDes = "Cấu hình chức năng cho vai trò|Cấu hình người dùng cho vai trò",
                ActionCustomMenu = "0|0",
                ActionCustomIcon = "|",
                ActionIgnore = ActionType.GroupAdvance
            });
            //Menu
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.Admin,
                AreaDes = AreaDesc.Admin,
                ControllerName = ControllerName.SysMenu,
                ControllerDes = ControllerDesc.SysMenu,
                ControllerIcon = "<i class=\"fa fa-list\"></i>",
                //ActionCustom = "AddAction|AddUser",
                //ActionParam = "id|id",
                //ActionCustomDes = "Cấu hình chức năng cho vai trò|Cấu hình người dùng cho vai trò",
                //ActionCustomMenu = "0|0",
                //ActionCustomIcon = "|",
                ActionIgnore = ActionType.GroupAdvance
            });

            //DiemIn
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.Admin,
                AreaDes = AreaDesc.Admin,
                ControllerName = ControllerName.DiemIn,
                ControllerDes = ControllerDesc.DiemIn,
                ControllerIcon = "",
                //ActionCustom = "AddAction|AddUser",
                //ActionParam = "id|id",
                //ActionCustomDes = "Cấu hình chức năng cho vai trò|Cấu hình người dùng cho vai trò",
                //ActionCustomMenu = "0|0",
                //ActionCustomIcon = "|",
                ActionIgnore = ActionType.GroupAdvance
            });

            //DMToaSoan
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.Admin,
                AreaDes = AreaDesc.Admin,
                ControllerName = ControllerName.DMToaSoan,
                ControllerDes = ControllerDesc.DMToaSoan,
                ControllerIcon = "",
                //ActionCustom = "AddAction|AddUser",
                //ActionParam = "id|id",
                //ActionCustomDes = "Cấu hình chức năng cho vai trò|Cấu hình người dùng cho vai trò",
                //ActionCustomMenu = "0|0",
                //ActionCustomIcon = "|",
                ActionIgnore = ActionType.Import + "|" + ActionType.ExportPdf
            });

            //DanhMucCon
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = "",
                AreaDes = "",
                ControllerName = ControllerName.DanhMucCon,
                ControllerDes = ControllerDesc.DanhMucCon,
                ControllerIcon = "",
                //ActionCustom = "AddAction|AddUser",
                //ActionParam = "id|id",
                //ActionCustomDes = "Cấu hình chức năng cho vai trò|Cấu hình người dùng cho vai trò",
                //ActionCustomMenu = "0|0",
                //ActionCustomIcon = "|",
                ActionIgnore = ActionType.GroupAdvance
            });


            //DMloaianpham
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.Admin,
                AreaDes = AreaDesc.Admin,
                ControllerName = ControllerName.DMLoaiAnPham,
                ControllerDes = ControllerDesc.DMLoaiAnPham,
                ControllerIcon = "",
                //ActionCustom = "AddAction|AddUser",
                //ActionParam = "id|id",
                //ActionCustomDes = "Cấu hình chức năng cho vai trò|Cấu hình người dùng cho vai trò",
                //ActionCustomMenu = "0|0",
                //ActionCustomIcon = "|",
                ActionIgnore = ActionType.GroupAdvance
            });

            //PHNC
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.Admin,
                AreaDes = AreaDesc.Admin,
                ControllerName = ControllerName.PHNC,
                ControllerDes = ControllerDesc.PHNC,
                ControllerIcon = "",
                ActionCustom = "EditPHNC",
                ActionParam = "id",
                ActionCustomDes = "Sửa Phân Hướng Nhu Cầu",
                ActionCustomMenu = "0",
                ActionCustomIcon = Application.ButtonIcon.ToEdit,
                ActionIgnore = ActionType.GroupAdvance + "|" + ActionType.Delete,
            });

            //PHNCBao
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.Admin,
                AreaDes = AreaDesc.Admin,
                ControllerName = ControllerName.PHNCBao,
                ControllerDes = ControllerDesc.PHNCBao,
                ControllerIcon = "",
                ActionCustom = "EditPHNC",
                ActionParam = "id",
                ActionCustomDes = "Sửa Phân Hướng Nhu Cầu Cho Báo",
                ActionCustomMenu = "0",
                ActionCustomIcon = Application.ButtonIcon.ToEdit,
                ActionIgnore = ActionType.GroupAdvance + "|" + ActionType.Delete,
            });

            //Thong tin bao
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.Bao,
                AreaDes = AreaDesc.Bao,
                ControllerName = ControllerName.ThongTinBao,
                ControllerDes = ControllerDesc.ThongTinBao,
                ControllerIcon = "<i class=\"fa fa-newspaper-o\"></i>",
                ActionCustom = "Index|ChiTiet|DieuChinhKeHoachXuatBan|Edit",
                //ActionParam = "id|id",
                ActionCustomDes = "Kế hoạch xuất bản|Chi tiết kế hoạch xuất bản|Điều chỉnh kế hoạch xuất bản|Chạy lại kế hoạch xuất bản",
                ActionCustomMenu = "1|0|0|0",
                //ActionCustomIcon = "|",
                ActionIgnore = ActionType.GroupAdvance
            });

            //Ke hoach xuat ban
            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.Bao,
                AreaDes = AreaDesc.Bao,
                ControllerName = ControllerName.KeHoachXuatBan,
                ControllerDes = ControllerDesc.KeHoachXuatBan,
                ControllerIcon = "<i class=\"fa fa-calendar\"></i>",
                //ActionCustom = "AddAction|AddUser",
                //ActionParam = "id|id",
                //ActionCustomDes = "Cấu hình chức năng cho vai trò|Cấu hình người dùng cho vai trò",
                //ActionCustomMenu = "0|0",
                //ActionCustomIcon = "|",
                ActionIgnore = ActionType.GroupAdvance
            });

            result.Add(new ActionDefine()
            {
                Component = Component.Admin,
                ComponentDesc = ComponentDesc.Admin,
                AreaName = AreaName.Bao,
                AreaDes = AreaDesc.Bao,
                ControllerName = ControllerName.ThongTinGiaBao,
                ControllerDes = ControllerDesc.ThongTinGiaBao,
                ControllerIcon = "<i class=\"fa fa-calendar\"></i>",
                //ActionCustom = "AddAction|AddUser",
                //ActionParam = "id|id",
                //ActionCustomDes = "Cấu hình chức năng cho vai trò|Cấu hình người dùng cho vai trò",
                //ActionCustomMenu = "0|0",
                //ActionCustomIcon = "|",
                ActionIgnore = ActionType.GroupAdvance
            });

            return result;
        }
    }
}